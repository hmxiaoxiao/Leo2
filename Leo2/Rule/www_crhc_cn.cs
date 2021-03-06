﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Leo2.Model;
using HtmlAgilityPack;
using Leo2.Helper;

namespace Leo2.Rule
{
    public class www_crhc_cn : BaseRule
    {
        private static readonly string list_xpath = "html/body/table[3]/tr/td[2]/table[3]/tr/td/table/tr[1]/td/table/tr/td/a";
        private static readonly string page_xpath = "html/body/table[3]/tr/td/table[2]/tr/td";

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="url">列表的起始页</param>
        public www_crhc_cn(Web web)
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
            HtmlNodeCollection lists = doc.DocumentNode.SelectNodes("//option");

            // 循环加入所有的页
            foreach (HtmlNode node in lists)
            {
                string href = node.Attributes["value"].Value;    //  /Category.aspx?nodeid=29&page=107
                string url = "http://" + u.Authority;
                m_list.Add(string.Format(@"{0}{1}", url, href));
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
