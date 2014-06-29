using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Leo2.Model;
using HtmlAgilityPack;
using Leo2.Helper;

namespace Leo2.Rule
{
    public class www_crmg_com_cn : BaseRule
    {
        private static readonly string list_xpath = ".//*[@class='Crmg-second-news-item-title']/a";
        private static readonly string page_xpath = ".//*[@id='zhd_ctr925_ModuleContent']/div/table";

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="url">列表的起始页</param>
        public www_crmg_com_cn(Web web)
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
            HtmlNodeCollection lists = doc.DocumentNode.SelectNodes("//a[@class='i-pager-last']");

            // 先加上自己这一页
            m_list.Add(CurrentWeb.URL);

            // 循环加入所有的页
            foreach (HtmlNode node in lists)
            {
                string href = node.Attributes["href"].Value;    //  /Page/398/MoreModuleID/936/article936/25/default.aspx
                string url = "http://" + u.Authority + href;
                Uri last = new Uri(url);


                int count = int.Parse(Regex.Match(last.Segments[last.Segments.Count() - 2], @"[\d]+").Value);
                string pre_url = "http://" + u.Authority;
                for (int i = 0; i < last.Segments.Count() - 2; i++)
                {
                    pre_url += last.Segments[i];
                }

                // 生成所有的列表联接
                for (int i = 2; i <= count; i++)
                {
                    m_list.Add(string.Format(@"{0}{1}/{2}", pre_url, i, last.Segments[last.Segments.Count()-1]));
                }

            }
            m_index = 1;
            return m_list.Count;
        }

        private int m_index = 0;
        private List<string> m_list = new List<string>(); 

        protected override string GetNextLink(string list_url)
        {
            if (m_index < m_list.Count)
                return m_list[m_index++];
            else
                return "";                
        }
    }
}
