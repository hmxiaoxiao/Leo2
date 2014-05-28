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
        private static HtmlDocument GetHtmlDocument(string url)
        {
            // 如果之前已经读取过相同的网页就直接返回
            if (m_pagecache.ContainsKey(url))
                return (HtmlDocument)m_pagecache[url];

            HtmlWeb webpage = new HtmlWeb();
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
        public static bool GetAndSavePagesOnList(Web web, string list_page_url, bool update_all = false)
        {
            // 如果下载规则一切正常，就开始下载
            if (!(web != null && web.Is_Search && !string.IsNullOrEmpty(web.List_URL_XPath)))
                return false;

            bool find_exist_url = false;

            HtmlDocument doc = GetHtmlDocument(list_page_url);
            Uri u = new Uri(web.URL);
            string WEB_ROOT = "http://" + u.Authority;

            // 获取方式
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
                        // 补全地址！
                        string url = node.Attributes["href"].Value;
                        if (url.Substring(0, 1) == "/")
                            url = WEB_ROOT + url;

                        // 判断是否重复
                        XPQuery<Page> pageQuery = new XPQuery<Page>(XpoDefault.Session);
                        var pages = from page in pageQuery
                                    where page.URL == url
                                    select page;
                        if (pages.Count() == 0)
                        {

                            string title;
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
                    uow.CommitChanges();
#if DEBUG
                    TimeSpan sp = new TimeSpan(DateTime.Now.Ticks - dt_now.Ticks);
                    Console.WriteLine(sp.TotalMilliseconds);
#endif
                }
            }

            // 如果已经找到有重复的记录，且不需全部更新话，就不再扫描下一页了
            if (!(find_exist_url == true && update_all == false))
            {
                string next_url = GetNextLinkOnList(web, list_page_url);
                if (!string.IsNullOrEmpty(next_url))
                    GetAndSavePagesOnList(web, next_url);
            }

            return true;
        }


        /// <summary>
        /// 根据列表地址，取出下一页的列表地址
        /// </summary>
        /// <param name="web">WEB规则</param>
        /// <param name="list_page_url">当前列表的地址</param>
        /// <returns>下一页的地址</returns>
        public static string GetNextLinkOnList(Web web, string list_page_url)
        {
            Uri u = new Uri(web.URL);
            string web_root = "http://" + u.Authority;
            string next_url = "";

            HtmlDocument doc = GetHtmlDocument(list_page_url);
            HtmlNodeCollection lists;
            if (string.IsNullOrEmpty(web.Next_URL_XPath))
            {
                lists = doc.DocumentNode.SelectNodes("//a");
                foreach (HtmlNode node in lists)
                {
                    if (node.InnerText == "下一页")
                    {
                        if (node.Attributes["href"] != null)        // 如果没有href属性的话，就跳过
                        {
                            next_url = node.Attributes["href"].Value;
                            if (next_url.Substring(0, 1) == "/")
                                next_url = web_root + next_url;
                        }
                    }
                }
            }
            else
            {
                lists = doc.DocumentNode.SelectNodes(web.Next_URL_XPath);
                if (lists != null)
                {
                    foreach (HtmlNode node in lists)
                    {
                        if (node.Attributes["href"] != null)        // 如果没有href属性的话，就跳过
                        {
                            next_url = node.Attributes["href"].Value;
                            if (next_url.Substring(0, 1) == "/")
                                next_url = web_root + next_url;
                        }
                    }
                }
            }
            return next_url;
        }
    }
}
