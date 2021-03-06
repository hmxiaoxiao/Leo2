﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Leo2.Model;
using HtmlAgilityPack;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using System.Text.RegularExpressions;
using Leo2.Helper;

namespace Leo2.Rule
{
    public class www_cnnc_com_cn : BaseRule
    {
        private static readonly string list_xpath = "//td[@id='newslist']/a";
        private static readonly string page_xpath = "//div[@id='ess_ctr2251_ModuleContent']/table";

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="url">列表的起始页</param>
        public www_cnnc_com_cn(Web web)
            : base(web)
        {
            // 设置取数规则
            base.List_XPath = list_xpath;
            base.Page_XPath = page_xpath;
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
            HtmlDocument doc = WebHelper.GetHtmlDocument(CurrentWeb.URL);

            string next_url = "";
            HtmlNodeCollection lists = doc.DocumentNode.SelectNodes("//a");
            if (lists != null)
            {
                foreach (HtmlNode node in lists)
                {
                    // 找到下一页
                    if (node.InnerText == "尾页")
                    {
                        next_url = node.Attributes["href"].Value;
                        break;
                    }
                }
            }

            // 补全地址
            Uri u = new Uri(this.CurrentWeb.URL);
            string web_root = "http://" + u.Authority;
            next_url = web_root + next_url;


            // 分析出页数
            u = new Uri(next_url);
            string temp = u.Segments[u.Segments.Count() - 1];

            string count = "";
            count = Regex.Match(temp, @"\d+").Value;

            return int.Parse(count);
        }

    }
}
