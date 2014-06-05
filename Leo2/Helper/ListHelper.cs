using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

using Leo2.Model;
using HtmlAgilityPack;
using DevExpress.Xpo;

namespace Leo2.Helper
{
    public class ListHelper
    {
        // 通过GetPagesOnListPage读取到的网页地址全部保存在此处
        // 注意每次调用GetPagesOnListPage前，先将此列表清空
        public static List<Page> PageList = new List<Page>();

        // 网页的缓存，如果有地址已经读取过，则会在这里保存，不用再读网页了。   
        private static Hashtable m_pagecache = new Hashtable();

        /// <summary>
        /// 从指定的网址上读取网页的html
        /// 这里采用了cache,之前读过的网页不会重复读
        /// </summary>
        /// <param name="url">网址(必须以http为前缀)</param>
        /// <returns>网页的HtmlDocument</returns>
        private static HtmlDocument GetHtmlDocument(Web web, string url)
        {
            // 如果之前已经读取过相同的网页就直接返回
            if (m_pagecache.ContainsKey(url))
                return (HtmlDocument)m_pagecache[url];

            HtmlWeb webpage = new HtmlWeb();
            if (!string.IsNullOrEmpty(web.Encoding))
                webpage.OverrideEncoding = Encoding.GetEncoding(web.Encoding);
            HtmlDocument doc = webpage.Load(url);   // 设置要读取的网页地址
            m_pagecache.Add(url, doc);

            return doc;
        }


        /// <summary>
        /// 取得列表界面上的每个页面的联接
        /// </summary>
        /// <param name="web"></param>
        /// <param name="list_page_url">列表页面的URL</param>
        /// <param name="update_all">
        /// 是否要更新全部数据，是的话，不管数据库是否存在，都会从第一页到最后一页全部进行扫描，
        /// 如果为否的话，则扫描到存在的记录后，不会再扫描了，直接返回。
        /// </param>
        /// <returns>
        /// 成功或者失败
        /// 成功后，分析出来的页面联接会放在静态变量PageList里。
        /// </returns>
        public static void GetAndSavePagesOnList(Web web, string list_page_url, bool update_all = false)
        {
            // 如果下载规则一切正常，就开始下载
            if (!VerifyWebInfo(web))
                return;

            // 下载该列表下的page联接
            bool find_exist_url = GetAndSavePagesOnListPage(web, list_page_url);


            // 如果已经找到有重复的记录，且不需全部更新话，就不再扫描下一页了
            if (!(find_exist_url == true && update_all == false))
            {
                string next_url = GetNextLinkOnList(web, list_page_url);
                if (!string.IsNullOrEmpty(next_url))
                    GetAndSavePagesOnList(web, next_url, update_all);
            }

            return;
        }

        /// <summary>
        /// 校验WEB的数据是否正确
        /// </summary>
        /// <param name="web"></param>
        /// <returns></returns>
        public static bool VerifyWebInfo(Web web)
        {
            // 如果下载规则一切正常，就开始下载
            if (!(web != null && web.Is_Search && !string.IsNullOrEmpty(web.List_URL_XPath)))
                return false;
            return true;
        }

        /// <summary>
        /// 保存指定列表中的所有的页面数据
        /// </summary>
        /// <param name="web"></param>
        /// <param name="list_url"></param>
        /// <returns>有已经存在的，返回真，否则返回假</returns>
        public static bool GetAndSavePagesOnListPage(Web web, string list_url)
        {
            // 先取得网站的前缀，有些联接是不加域名的。
            Uri u = new Uri(web.URL);
            string WEB_ROOT = "http://" + u.Authority;

            // 是否有已经存在的页面
            bool find_exist_url = false;

            // 取得列表面的内容
            HtmlDocument doc = GetHtmlDocument(web, list_url);
            HtmlNodeCollection lists = doc.DocumentNode.SelectNodes(web.List_URL_XPath);

            // 如果没有内容的话，直接返回
            if (lists != null)
            {
                // 判断时间
#if DEBUG
                DateTime dt_now = DateTime.Now;
#endif
                using (UnitOfWork uow = new UnitOfWork())
                {
                    // 取所有的列表
                    foreach (HtmlNode node in lists)
                    {
                        // 取得联接，如果需要补全地址！
                        string url = node.Attributes["href"].Value;
                        if (url.Substring(0, 1) == "/")
                            url = WEB_ROOT + url;
                        if (url.Substring(0, 2) == "..")
                        {
                            url = "http://" + u.Authority;
                            for (int i = 0; i < u.Segments.Count() - 1; i++)
                                url += u.Segments[i];
                            url += node.Attributes["href"].Value;
                        }

                        // 判断是否重复，如没有重复，就保存到页面上去
                        XPQuery<Page> pageQuery = new XPQuery<Page>(XpoDefault.Session);
                        var pages = from page in pageQuery
                                    where page.URL == url
                                    select page;
                        if (pages.Count() == 0)
                        {
                            string title;       // 网页的标题 
                            if (node.Attributes["title"] != null)
                                title = node.Attributes["title"].Value;
                            else
                                title = node.InnerText;

                            Page page = new Page(uow);
                            page.Parent_ID = web.Oid;
                            page.URL = url;
                            page.Title = title;

                            PageList.Add(page);      // 加入查到的页面结点
#if DEBUG
                            Console.WriteLine(page.Title);
#endif
                        }
                        else
                        {
                            find_exist_url = true;
                        }
                    }
                    uow.CommitChanges();        //  保存所有的数据
#if DEBUG
                    TimeSpan sp = new TimeSpan(DateTime.Now.Ticks - dt_now.Ticks);
                    Console.WriteLine("总共时间为（毫秒）：" + sp.TotalMilliseconds.ToString());
#endif
                }
            }
            return find_exist_url;
        }


        public static void CustomerScan(Web web, string list_page_url)
        {
            Uri u = new Uri(list_page_url);
            HtmlDocument doc;
            HtmlNodeCollection lists;
            switch (web.Next_URL_XPath)
            {
                case "customer1":
                    doc = GetHtmlDocument(web, list_page_url);
                    lists = doc.DocumentNode.SelectNodes("//div[@style='display:none']/a");
                    foreach (HtmlNode node in lists)
                    {
                        // 生成所有的列表联接
                        string url = "http://" + u.Authority;
                        for (int i = 0; i < u.Segments.Count() - 1; i++)
                        {
                            url += u.Segments[i];
                        }
                        url += node.Attributes["href"].Value;

                        // 下载页面
                        GetAndSavePagesOnListPage(web, url);
                    }
                    break;

            }
        }

        /// <summary>
        /// 根据列表地址，取出下一页的列表地址
        /// </summary>
        /// <param name="web">WEB规则</param>
        /// <param name="list_page_url">当前列表的地址</param>
        /// <returns>下一页的地址</returns>
        public static string GetNextLinkOnList(Web web, string list_page_url)
        {
            HtmlDocument doc;
            HtmlNodeCollection lists;

            if (web.Next_URL_XPath.IndexOf("customer") >= 0)
            {
                Uri u = new Uri(list_page_url);
                switch (web.Next_URL_XPath)
                {
                    case "customer1":
                        CustomerScan(web, list_page_url);
                        return "";
                    case "customer:www.xd.com.cn":
                        #region 中国西电集团
                        doc = GetHtmlDocument(web, list_page_url);
                        lists = doc.DocumentNode.SelectNodes(".//a[text()='下一页']");
                        if (lists != null)
                        {
                            string tmpUrl = lists[0].Attributes["href"].Value;
                            if (tmpUrl != null && tmpUrl != "#")        // 如果没有href属性的话，就跳过
                            {
                                //找到页数，再合成一个URL
                                int pos1 = tmpUrl.IndexOf(",");
                                if (pos1 > -1)
                                {
                                    int pos2 = tmpUrl.IndexOf(")");
                                    if (pos2 > -1)
                                    {
                                        string tmp = tmpUrl.Substring(pos1 + 1, pos2 - pos1 - 1).Replace("'", "");
                                        //合并生成新URL
                                        pos1 = web.URL.LastIndexOf(".");
                                        // 生成所有的列表联接
                                        string url = web.URL.Substring(0, pos1) + "_" + tmp + web.URL.Substring(pos1, web.URL.Length - pos1);
                                        // 下载页面
                                        return url;
                                    }
                                }
                            }
                        }
                        return "";

                        #endregion
                }
            }


            string next_url = "";

            string[] next_title = new string[] { "下一页", "下页" };
            doc = GetHtmlDocument(web, list_page_url);
            if (string.IsNullOrEmpty(web.Next_URL_XPath))       // 如果有指定的下一页的方法，就按指定的方法取
            {
                lists = doc.DocumentNode.SelectNodes("//a");
                if (lists != null)
                {
                    foreach (HtmlNode node in lists)
                    {
                        foreach (string title in next_title)
                        {
                            if (node.InnerText.IndexOf(title) != -1)
                            {
                                if (node.Attributes["href"] != null)        // 如果没有href属性的话，就跳过
                                    next_url = GeneNextLinkURL(web, list_page_url, node.Attributes["href"].Value);
                            }
                        }
                    }
                }
            }
            else   //  按默认的方法 取下一页的联接
            {
                lists = doc.DocumentNode.SelectNodes(web.Next_URL_XPath);
                if (lists != null)
                {
                    foreach (HtmlNode node in lists)
                    {
                        if (node.Attributes["href"] != null)        // 如果没有href属性的话，就跳过
                            next_url = GeneNextLinkURL(web, list_page_url, node.Attributes["href"].Value);
                    }
                }
            }
            return next_url;
        }


        /// <summary>
        /// 取得下一页的列表的URL
        /// </summary>
        /// <param name="web"></param>
        /// <param name="current_url"></param>
        /// <param name="next_url"></param>
        /// <returns></returns>
        private static string GeneNextLinkURL(Web web, string current_url, string next_url)
        {
            Uri u = new Uri(web.URL);
            string web_root = "http://" + u.Authority;
            string real_next_url = "";

            if (next_url.Substring(0, 1) == "/")
                next_url = web_root + next_url;
            real_next_url = next_url;
            if (next_url.IndexOf("/") < 0)
            {
                real_next_url = web_root;
                for (int i = 0; i < u.Segments.Count() - 1; i++)
                {
                    real_next_url += u.Segments[i];
                }
                if (u.Segments[u.Segments.Count() - 1].IndexOf("/") >= 0)
                    real_next_url += u.Segments[u.Segments.Count() - 1];
                real_next_url += next_url;
            }

            return real_next_url;
        }
    }
}
