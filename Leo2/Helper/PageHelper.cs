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
        public static void GetContentWithSave()
        {
            XPCollection<Web> webs = new XPCollection<Web>();

            XPQuery<Page> pageQuery = new XPQuery<Page>(XpoDefault.Session);
            var pages = from p in pageQuery
                       where p.Is_Down == false
                       select p;
            HtmlWeb htmlweb = new HtmlWeb();
            HtmlDocument doc;

            using (UnitOfWork uow = new UnitOfWork())
            {
                foreach (Page p in pages)
                {
#if DEBUG
                    Console.WriteLine(p.URL);
#endif
                    // 先取得该页面的XPath
                    foreach (Web w in webs)
                    {
                        if (w.Oid == p.Parent_ID)
                        {
                            doc = htmlweb.Load(p.URL);
                            HtmlNodeCollection firstpage = doc.DocumentNode.SelectNodes(w.Page_XPath);

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
                uow.CommitChanges();
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
