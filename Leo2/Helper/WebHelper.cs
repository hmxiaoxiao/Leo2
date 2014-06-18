using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using HtmlAgilityPack;

namespace Leo2.Helper
{
    public class WebHelper
    {
        // 网页的缓存，如果有地址已经读取过，则会在这里保存，不用再读网页了。   
        private static Dictionary<string, HtmlDocument> m_pagecache = new Dictionary<string, HtmlDocument>();


        /// <summary>
        /// 从指定的网址上读取网页的html
        /// </summary>
        /// <param name="url">网址(必须以http为前缀)</param>
        /// <returns>网页的HtmlDocument</returns>
        public static HtmlDocument GetHtmlDocument(string url, string encoding = null, int retry = 5)
        {
            // 如果之前已经读取过相同的网页就直接返回
            if (m_pagecache.ContainsKey(url))
                return m_pagecache[url];

            // 没有读取过数据的话就直接取出
            HtmlWeb webpage = new HtmlWeb();
            HtmlDocument doc = new HtmlDocument();
            if (!string.IsNullOrEmpty(encoding))
                webpage.OverrideEncoding = Encoding.GetEncoding(encoding);

            // 读取网页内容重复五次，五次不成功，就返回空
            for (int i = 0; i < retry; i++)
            {
                try
                {
                    doc = webpage.Load(url);   // 设置要读取的网页地址
                    return doc;
                }
                catch
                {
                    if (i == retry - 1)
                        break;
                    else
                        return null;
                }
            }

            // 将读取到的内容保存到列表里面
            if (doc != null)
            {
                m_pagecache.Add(url, doc);
                return doc; // 返回读取的内容
            }
            else
            {
                return new HtmlDocument();
            }
        }
    }
}
