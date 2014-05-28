using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using HtmlAgilityPack;
using DevExpress.Xpo;
using Leo2.Model;
using System.Text.RegularExpressions;

namespace Leo2.Helper
{
    public class PageHelper
    {
        /// <summary>
        /// 下载所有的未下载网页的内容
        /// </summary>
        public static void GetAllContentWithSave()
        {
            XPQuery<Page> pageQuery = new XPQuery<Page>(XpoDefault.Session);
            var pages = from p in pageQuery
                       where p.Is_Down == false
                       select p;

            foreach (Page p in pages)
            {
                GetSingleContentWithSave(p);
            }
        }

        /// <summary>
        /// 下载指定网页的内容
        /// </summary>
        /// <param name="p"></param>
        public static void GetSingleContentWithSave(Page p)
        {
#if DEBUG
            Console.WriteLine(p.URL);
#endif
            HtmlWeb htmlweb = new HtmlWeb();
            HtmlDocument doc;

            // 先取得该页面的XPath
            foreach (Web w in new XPCollection<Web>())
            {
                if (w.Oid == p.Parent_ID)
                {
                    doc = htmlweb.Load(p.URL);
                    string[] xpaths = w.Page_XPath.Split('|');
                    foreach (string xpath in xpaths)
                    {
                        HtmlNodeCollection firstpage = doc.DocumentNode.SelectNodes(xpath);

                        if (firstpage != null && firstpage.Count >= 1)
                        {
                            p.CDate = Regex.Match(doc.DocumentNode.InnerHtml, @"\d{2,4}-\d{2}-\d{2}").Value;
                            p.Content = firstpage[0].InnerHtml;
                            p.Is_Down = true;
                            p.Save();
                        }
                    }
                }
            }
        }

        public static void GetSingleContentWithSave(int page_oid)
        {
            XPQuery<Page> pageQuery = new XPQuery<Page>(XpoDefault.Session);
            var pages = from p in pageQuery
                        where p.Oid == page_oid
                        select p;

            foreach (Page page in pages)
            {
                GetSingleContentWithSave(page);
            }
        }


        /// <summary>
        /// 恢复html中的特殊字符
        /// </summary>
        /// <param name="theString">需要恢复的文本。</param>
        /// <returns>恢复好的文本。</returns>
        private static string HtmlDiscode(string theString)
        {
            theString = theString.Replace("&gt;", ">");
            theString = theString.Replace("&lt;", "<");
            theString = theString.Replace("&nbsp;", "");
            theString = theString.Replace("&quot;", "\"");
            theString = theString.Replace("&#39;", "\'");
            theString = theString.Replace("<br/> ", "\n");
            return theString;
        }
    }
}
