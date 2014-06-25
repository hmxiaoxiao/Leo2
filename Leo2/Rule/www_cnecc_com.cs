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
    public class www_cnecc_com : BaseRule
    {
        private static readonly string list_xpath = "//div[@class='xwzx-ywzd-item-title']/a";
        private static readonly string page_xpath = "//div[@id='dnn_ctr930_ModuleContent']|//div[@id='dnn_ContentPane']";

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="url">列表的起始页</param>
        public www_cnecc_com(Web web)
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
            HtmlNodeCollection lists = doc.DocumentNode.SelectNodes("//a[@class='i-pager-last']");
            if (lists.Count != 1)
                return 0;

            next_url = lists[0].Attributes["href"].Value;
            string temp = next_url.Split('/')[next_url.Split('/').Count() - 1];
            string count = Regex.Match(temp, @"\d+").Value;

            return int.Parse(count);
        }

        /// 取得下一页的地址
        /// <summary>
        /// 根据列表的URL，取得下一页的链接地址（默认的方法）
        /// 默认方法是，取得页面上的所有的联接，然后，根据联接包含的内容是否为下一页（或者类似的词语）来判断
        /// </summary>
        /// <param name="list_url">当前的列表页面地址</param>
        /// <returns>下一页的URL</returns>
        protected override string GetNextLink(string list_url)
        {
            // 先读取内容 
            HtmlDocument doc = WebHelper.GetHtmlDocument(list_url);        // 读取网页内容
            if (doc == null)         // 出错了
                return null;

            HtmlNodeCollection lists = doc.DocumentNode.SelectNodes("//a[@class='i-pager-next']");     // 取得所有的链接
            //Debug.Assert(lists.Count() == 1);
            if (lists.Count() == 1)
                return GeneRightURL(lists[0].Attributes["href"].Value);
            else
                return "";
        }
    }
}
