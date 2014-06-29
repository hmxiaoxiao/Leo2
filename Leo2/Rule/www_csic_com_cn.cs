using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;


using Leo2.Model;
using HtmlAgilityPack;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using Leo2.Helper;

namespace Leo2.Rule
{
    class www_csic_com_cn : BaseRule
    {
        private static readonly string list_xpath = "/html/body/table[9]/tr/td[2]//a[@target='_blank']";
        private static readonly string page_xpath = "/html/body/table[9]/tr/td[2]";

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="url">列表的起始页</param>
        public www_csic_com_cn(Web web)
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
            HtmlNodeCollection lists = doc.DocumentNode.SelectNodes("//a[@target='_parent']");

            // 先加上自己这一页
            m_list.Add(CurrentWeb.URL);

            // 循环加入所有的页
            foreach (HtmlNode node in lists)
            {
                //Console.WriteLine(node.Attributes["href"]);
                if(node.InnerText.IndexOf("末页") >=0)
                {
                    string href = node.Attributes["href"].Value;
                    int count = int.Parse(Regex.Match(href, @"[\d]+").Value);
                    string url = "http://" + u.Authority;
                    for (int i = 0; i < u.Segments.Count() - 1; i++)
                    {
                        url += u.Segments[i];
                    }
                    // 生成所有的列表联接
                    for (int i = 1; i <= count; i++)
                    {
                        m_list.Add(string.Format(@"{0}index{1}.shtml", url, i));
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
            if (m_index <= m_list.Count-1)
                return m_list[m_index++];
            else
                return "";                
        }
    }
}
