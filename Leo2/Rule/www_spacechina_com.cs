using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Leo2.Model;
using HtmlAgilityPack;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using System.Text.RegularExpressions;
using System.Diagnostics;
using Leo2.Helper;


namespace Leo2.Rule
{
    public class www_spacechina_com : BaseWeb
    {
        private static readonly string list_xpath = "//a[@class='hei_x_12_a']";
        private static readonly string page_xpath = "//div[@id='mainart']/table/tr/td/table[2]";

        private static List<string> m_list = new List<string>(); 

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="url">列表的起始页</param>
        public www_spacechina_com(Web web)
            : base(web)
        {
            // 设置取数规则
            base.List_XPath = list_xpath;
            base.Page_Xpath = page_xpath;
        }


        protected override int GetPagesCount()
        {            
            // 先读取内容 
            Uri u = new Uri(m_web.URL);
            HtmlDocument doc = WebHelper.GetHtmlDocument(m_web.URL);
            HtmlNodeCollection lists = doc.DocumentNode.SelectNodes("//div[@id='content_list']/div[@style='display:none']/a");
            foreach (HtmlNode node in lists)
            {
                // 生成所有的列表联接
                string url = "http://" + u.Authority;
                for (int i = 0; i < u.Segments.Count() - 1; i++)
                {
                    url += u.Segments[i];
                }
                url += node.Attributes["href"].Value;

                m_list.Add(url);

            }

            return lists.Count + 1;
        }

        private static int index = 0;

        protected override string GetNextLink(string list_url)
        {
            return m_list[index++];
        }
    }
}
