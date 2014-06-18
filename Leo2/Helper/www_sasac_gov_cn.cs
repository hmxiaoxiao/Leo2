using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Leo2.Model;
using HtmlAgilityPack;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;

namespace Leo2.Helper
{
    public class www_sasac_gov_cn : BaseWeb
    {
        private static readonly string list_xpath = "//td[@class='black14']/a";
        private static readonly string page_xpath = "/html[1]/body[1]/table[2]/tr[1]/td[2]/table[11]/tr[1]/td[1]";



        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="url">列表的起始页</param>
        public www_sasac_gov_cn(Web web)
            : base(web)
        {
            // 设置取数规则
            base.List_XPath = list_xpath;
            base.Page_Xpath = page_xpath;

            this.DownPage += SavePage;      // 开始下载时，自动保存
            this.ExistPages = new XPCollection<Page>(XpoDefault.Session,
                CriteriaOperator.Parse("Parent_ID = ?", web.Oid));
        }


        /// 取得国资委列表的页数
        /// <summary>
        /// 取得国资委列表的页数
        /// 总页数为下一页的联接数上+1
        /// 比如http://www.sasac.gov.cn/n1180/n20240/n20259/index_192.html为下一页的链接
        /// 则总数为192+1（还要加上当前页）共计192页
        /// </summary>
        /// <param name="doc">当前的列表内容</param>
        protected override int GetPagesCount()
        {
            // 先读取内容 
            HtmlDocument doc = WebHelper.GetHtmlDocument(m_web.URL);

            string next_url = "";
            HtmlNodeCollection lists = doc.DocumentNode.SelectNodes("//a");
            if (lists != null)
            {
                foreach (HtmlNode node in lists)
                {
                    // 找到下一页
                    if (node.InnerText == "下一页")
                    {
                        next_url = node.Attributes["href"].Value;
                        break;
                    }
                }
            }

            Uri u = new Uri(this.m_web.URL);
            string web_root = "http://" + u.Authority;
            next_url = web_root + next_url;

            u = new Uri(next_url);
            string temp = u.Segments[u.Segments.Count() - 1];
            temp = temp.Substring(temp.IndexOf('_') + 1,
                                  temp.IndexOf('.') - temp.IndexOf('_') -1);

            return int.Parse(temp) + 1;
        }


        // 保存页面到数据库里面
        protected void SavePage(object sender, BaseWeb.DownPageEventArgs e)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                foreach (Page page in e.m_pages)
                {
                    var fp = from p in ExistPages
                             where p.URL == page.URL
                             select p;
                    if (fp.Count() == 0)
                    {
                        Page newPage = new Page(uow) { Parent_ID = m_web.Oid, Title = page.Title, URL = page.URL };
                        newPage.Save();

                        ExistPages.Add(newPage);        //  加入已经找到的页面
                    }
                }
                uow.CommitChanges();
            }
        }
    }
}
