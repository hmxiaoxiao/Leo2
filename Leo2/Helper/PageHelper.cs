using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using HtmlAgilityPack;
using DevExpress.Xpo;
using Leo2.Model;
using System.Text.RegularExpressions;
using System.IO;

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

            XPCollection<Web> list = new XPCollection<Web>();
            Web w = list.FirstOrDefault(pp=>pp.Oid == p.Parent_ID);
            // 先取得该页面的XPath
            if (w != null)
            {
                if (!string.IsNullOrEmpty(w.Encoding))
                    htmlweb.OverrideEncoding = Encoding.GetEncoding(w.Encoding);
                doc = htmlweb.Load(p.URL);
                string[] xpaths = w.Page_XPath.Split('|');
                foreach (string xpath in xpaths)
                {
                    HtmlNodeCollection firstpage = doc.DocumentNode.SelectNodes(xpath);

                    if (firstpage != null && firstpage.Count >= 1)
                    {
                        p.CDate = GetDataFromContent(doc.DocumentNode.InnerHtml);
                        //p.CDate = Regex.Match(doc.DocumentNode.InnerHtml, @"\d{2,4}-\d{2}-\d{2}").Value;
                        //if(string.IsNullOrEmpty(p.CDate))
                        //    p.CDate = Regex.Match(doc.DocumentNode.InnerHtml, @"\d{2,4}/\d{2}/\d{2}").Value;
                        //把内容存到文件中

                        //p.Content = firstpage[0].InnerHtml;

                        p.Is_Down = SaveContentToFile(p, firstpage[0].InnerHtml); 
                        p.Save();
                    }
                }
            }
        }

        /// <summary>
        /// 把网页的内容存到文件中
        /// </summary>
        /// <param name="page"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        private static bool SaveContentToFile(Page page, string content)
        {

            try
            {
                //目录结构：当前目录/content/父ID目录/当前ID目录/content.html

                string filename = GetFilePath(page);
                FileStream fst = new FileStream(filename, FileMode.OpenOrCreate);
                //写数据到a.txt格式
                StreamWriter swt = new StreamWriter(fst, System.Text.Encoding.GetEncoding("utf-8"));
                //写入
                swt.Write(content);
                swt.Close();
                fst.Close();
                return true;
            }
            catch 
            {
                return false;
            }
        }

        /// <summary>
        /// 获得文件内容
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public static string GetContentFromFile(Page page)
        {
            string result = "";
            try
            {
                string filename = GetFilePath(page);
                //如果没有找到文件，就先下载
                if (!File.Exists(filename))
                {
                    GetSingleContentWithSave(page.Oid);
                }
                StreamReader srt = new StreamReader(filename, Convert.ToBoolean(FileMode.Open));
                //存在
                result = srt.ReadToEnd();
                srt.Close();

            }
            catch
            {
            }
            return result;
        }

        /// <summary>
        /// 返回文件的路径
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        private static string GetFilePath(Page page)
        {
            //获得当前目录
            string dir = Directory.GetCurrentDirectory();
            if (dir.Substring(dir.Length-1,1) != @"\")
            {
                dir = dir + @"\";
            }
            dir += @"Content\"+ page.Parent_ID + @"\" + page.Oid ;
            //判断目录是否存在
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            return dir + @"\Content.html";
        }

        /// <summary>
        /// 根据网页的内容取得当前的日期
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private static string GetDataFromContent(string content)
        {
            string cdate = "";
            cdate = Regex.Match(content, @"\d{2,4}-\d{2}-\d{2}").Value;
            if(string.IsNullOrEmpty(cdate))
                cdate = Regex.Match(content, @"\d{2,4}/\d{2}/\d{2}").Value;
            if(string.IsNullOrEmpty(cdate))
                cdate = Regex.Match(content, @"\d{2,4}年\d{2}月\d{2}日").Value;
            return cdate;
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
