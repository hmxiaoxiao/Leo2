using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Leo2.Model;
using HtmlAgilityPack;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using Leo2.Helper;

namespace Leo2.Rule
{
    class www_avic_com_cn : BaseRule
    {
        private static readonly string list_xpath = "//ul[@class='lnews']/li/a";
        private static readonly string page_xpath = "//div[@class='leftevery']";

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="url">列表的起始页</param>
        public www_avic_com_cn(Web web)
            : base(web)
        {
            // 设置取数规则
            base.List_XPath = list_xpath;
            base.Page_XPath = page_xpath;
        }

        protected override int GetPagesCount()
        {
            // 先读取内容 
            Uri u = new Uri(CurrentWeb.URL);
            HtmlDocument doc = WebHelper.GetHtmlDocument(CurrentWeb.URL, this.CurrentWeb.Encoding);
            HtmlNodeCollection lists = doc.DocumentNode.SelectNodes("//a");

            // 先加上自己这一页
            m_list.Add(CurrentWeb.URL);

            foreach (HtmlNode node in lists)
            {
                //Console.WriteLine(node.Attributes["href"]);
                if(node.InnerText.IndexOf("最末页") >=0)
                {
                    string href = node.Attributes["href"].Value;
                    int count = int.Parse(href.Split('_')[1].Split('.')[0]);
                    string url = "http://" + u.Authority;
                    for (int i = 0; i < u.Segments.Count() - 1; i++)
                    {
                        url += u.Segments[i];
                    }
                    // 生成所有的列表联接
                    for (int i = 2; i <= count; i++)
                    {
                        m_list.Add(string.Format(@"{0}index_{1}.shtml", url, i));
                    }
                }
            }
            m_index = 1;
            return m_list.Count;
        }

        private int m_index = 0;
        private List<string> m_list = new List<string>(); 

        protected override string GetNextLink(string list_url)
        {
            // 因为最大的一页就是当前的页
            // <a href='../../n99188/n470321/index_1036161_34.html'> 就是
            // http://www.casic.com.cn/n99188/n470321/index.html 
            // 且不能处理34这个网页，所以，到了最大的时候，就要减去
            // 或者采用倒着减的方法来处理。
            if (m_index <= m_list.Count)
                return m_list[m_index++];
            else
                return "";                
        }
    }
}
