using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using HtmlAgilityPack;
using Leo2.Model;
using System.Text.RegularExpressions;
using System.IO;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;

namespace Leo2.Helper
{
    /// <summary>
    /// 基本的网页下载基类，完成整个网页的列表，下一页，以及内容的保存
    /// </summary>
    public class BaseWeb
    {

        #region 声明事件和委托

        #region 开始下载列表页面事件
        public delegate void DownPageEventHandler(object sender, DownPageEventArgs e);
        public event DownPageEventHandler DownPage;

        public class DownPageEventArgs : EventArgs
        {
            public readonly List<Page> m_pages;
            public DownPageEventArgs(List<Page> pages)
            {
                m_pages = pages;
            }
        }


        // 定义事件，触发时，调用所有关注的事件。
        // 这里采用异步的联接方式，注意
        public virtual void OnDownPage(DownPageEventArgs e)
        {
            if (DownPage != null)
            {
                Delegate[] delArray = DownPage.GetInvocationList();

                foreach (Delegate del in delArray)
                {
                    DownPageEventHandler method = (DownPageEventHandler)del;
                    method.BeginInvoke(this, e, null, null);
                }
            }
        }
        #endregion

        #region 列表全部下载完成事件
        public delegate void DownloadCompleteEventHandle(object sender, DownloadCompleteEventArgs e);
        public event DownloadCompleteEventHandle DownloadComplete;

        public class DownloadCompleteEventArgs : EventArgs
        {
            public Web m_web;
            public DownloadCompleteEventArgs(Web web)
            {
                m_web = web;
            }
        }

        public virtual void OnDownloadComplete(DownloadCompleteEventArgs e)
        {
            if (DownloadComplete != null)
            {
                DownloadComplete(this, e);
            }
        }
        #endregion

        #endregion

        protected string List_XPath { get; set; }      // 列表取数规则
        protected string Page_Xpath { get; set; }      // 页面取数规则
        public List<Page> Pages { get; set; }       // 当前的页面数
        protected XPCollection<Page> ExistPages { get; set; }  // 已经搜索到的页面

        private int m_max_page = -1;        // 记录最大的页数
        protected Web m_web;
  

        // 取得总共页数
        public int MaxPage
        {
            get
            {
                if (m_max_page == -1)
                    m_max_page = GetPagesCount();
                return m_max_page;
            }
        }


        /// <summary>
        /// 虚拟方法，用来取得最大的页数
        /// </summary>
        /// <returns></returns>
        protected virtual int GetPagesCount()
        {
            return 0;
        }



        /// <summary>
        /// 构造函数，必须加上起始列表的URL
        /// </summary>
        /// <param name="url"></param>
        public BaseWeb(Web web)
        {
            m_web = web;
            int itemp = MaxPage;        // 计算一下页面总数
            this.ExistPages = new XPCollection<Page>(XpoDefault.Session,
                    CriteriaOperator.Parse("Parent_ID = ?", web.Oid));

            this.DownPage += SavePage;      // 开始下载时，自动保存
        }


        /// 取得列表界面上的每个页面的联接
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
        public bool GetPagesOnList(string url = "", bool search_over = false)
        {
            // 如果下载的地址为空的话就取WEB的地址（第一次调用不用加的）
            if (string.IsNullOrEmpty(url))
                url = m_web.URL;

            // 下载该列表下的page联接
            GetPagesOnList(url);

            // 触发事件
            DownPageEventArgs e = new DownPageEventArgs(Pages);
            OnDownPage(e);

            // 如果该栏目没有全部扫描过，就继续扫描
            if (!(search_over &&
                NewPageInExistPages()))     // 或者新的页面没有出现在页面列表里（表明全部都是新页面，那就要扫描下一页了）
            {

                string next_url = GetNextLink(url);
                if (!string.IsNullOrEmpty(next_url))
                {
                    // 如果取得下一页地址，就继续扫描
                    return GetPagesOnList(next_url, search_over);
                }
                else
                {
                    // 没有取得下一页地址
                    if (next_url == "")     // 空 表明全部搜索过了
                    {
                        //通知已经全部下载完成了。
                        DownloadCompleteEventArgs eventComplete = new DownloadCompleteEventArgs(m_web);
                        OnDownloadComplete(eventComplete);


                        return true;
                    }
                    else
                        return false;
                }
            }
            return false;
        }


        /// 判断新取得的页面是否在新页面列表里存在
        /// <summary>
        /// 判断新取得的页面是否在新页面列表里存在
        /// </summary>
        /// <returns>存在，返回址，不存在，返回假</returns>
        private bool NewPageInExistPages()
        {
            foreach (Page page in Pages)
            {
                var fp = from p in ExistPages
                         where p.URL == page.URL
                         select p;
                if (fp.Count() > 0)
                    return true;
            }
            return false;
        }


        /// 从列表中取得页面
        /// <summary>
        /// 保存指定列表中的所有的页面数据
        /// </summary>
        /// <param name="web"></param>
        /// <param name="list_url"></param>
        /// <returns>正常返回为真，读取网页内容出错，则为假</returns>
        public bool GetPagesOnList(string list_url)
        {
            // 清空已经获得的列表
            Pages = new List<Page>();

            // 取得列表面的内容
            HtmlDocument doc = WebHelper.GetHtmlDocument(list_url);
            if (doc == null)
                return false;

            HtmlNodeCollection lists = doc.DocumentNode.SelectNodes(this.List_XPath);

            // 取所有的页面结点
            foreach (HtmlNode node in lists)
            {
                // 生成一个新的page实例 
                Page page = new Page();

                // 取得联接
                page.URL = GeneRightURL(node.Attributes["href"].Value);

                // 网页的标题 
                if (node.Attributes["title"] != null)
                    page.Title = node.Attributes["title"].Value;
                else
                    page.Title = node.InnerText;

                Pages.Add(page);      // 加入查到的页面结点
            }
            return true;
        }


        /// 取得下一页的地址
        /// <summary>
        /// 根据列表的URL，取得下一页的链接地址（默认的方法）
        /// 默认方法是，取得页面上的所有的联接，然后，根据联接包含的内容是否为下一页（或者类似的词语）来判断
        /// </summary>
        /// <param name="list_url">当前的列表页面地址</param>
        /// <returns>下一页的URL</returns>
        protected virtual string GetNextLink(string list_url)
        {
            string[] next_title = new string[] { "下一页", "下页" };     

            HtmlDocument doc = WebHelper.GetHtmlDocument(list_url);        // 读取网页内容
            if (doc == null)         // 出错了
                return null;

            HtmlNodeCollection lists = doc.DocumentNode.SelectNodes("//a");     // 取得所有的链接
            foreach (HtmlNode node in lists)
            {
                foreach (string title in next_title)        // 判断链接是否包含下一页
                {
                    // 如果链接包含“下一页"，且有href属性，则生成url(因为有些链接是不包含网址的，只有相对路径）
                    if (node.InnerText.IndexOf(title) != -1 && node.Attributes["href"] != null)        // 如果没有href属性的话，就跳过
                        return GeneRightURL(node.Attributes["href"].Value);
                }
            }
            return "";
        }


        /// 补全URL
        /// <summary>
        /// 补全URL
        /// </summary>
        /// <param name="current_url">当前的列表地址</param>
        /// <param name="next_url">取得的下一页地址</param>
        /// <returns></returns>
        protected string GeneRightURL(string next_url)
        {
            Uri u = new Uri(this.m_web.URL);
            string web_root = "http://" + u.Authority;
            string real_next_url = "";

            if (next_url.Substring(0, 1) == "/")        // 如果地址是以"/"开头，说明用的是相对地址，要加上网址
                return web_root + next_url;

            if (next_url.Substring(0, 2) == "..")       // 如果是以。。开发的，要把最后的网页文件去掉，再加路径
            {
                real_next_url = "http://" + u.Authority;
                for (int i = 0; i < u.Segments.Count() - 1; i++)
                    real_next_url += u.Segments[i];
                return real_next_url + next_url;
            }


            // 这是一种特殊情况，暂时忘了是怎么弄的了。
            //real_next_url = next_url;
            //if (next_url.IndexOf("/") < 0)
            //{
            //    real_next_url = web_root;
            //    for (int i = 0; i < u.Segments.Count() - 1; i++)
            //    {
            //        real_next_url += u.Segments[i];
            //    }
            //    if (u.Segments[u.Segments.Count() - 1].IndexOf("/") >= 0)
            //        real_next_url += u.Segments[u.Segments.Count() - 1];
            //    real_next_url += next_url;
            //}

            return real_next_url;
        }


        /// 下载指定网页的内容
        /// <summary>
        /// 下载指定网页的内容
        /// </summary>
        /// <param name="p"></param>
        public void GetSingleContentWithSave(Page p)
        {
            HtmlDocument doc = WebHelper.GetHtmlDocument(p.URL);
            string[] xpaths = Page_Xpath.Split('|');
            foreach (string xpath in xpaths)
            {
                HtmlNodeCollection firstpage = doc.DocumentNode.SelectNodes(xpath);

                if (firstpage != null && firstpage.Count >= 1)
                {
                    p.CDate = GetDataFromContent(doc.DocumentNode.InnerHtml);
                    p.Is_Down = SaveContentToFile(p, firstpage[0].InnerHtml);
                    p.Save();
                }
            }
        }


        /// 根据网页的内容取得该网页的发布日期
        /// <summary>
        /// 根据网页的内容取得该网页的发布日期
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private static string GetDataFromContent(string content)
        {
            string cdate = "";
            cdate = Regex.Match(content, @"\d{2,4}-\d{2}-\d{2}").Value;
            if (string.IsNullOrEmpty(cdate))
                cdate = Regex.Match(content, @"\d{2,4}/\d{2}/\d{2}").Value;
            if (string.IsNullOrEmpty(cdate))
                cdate = Regex.Match(content, @"\d{2,4}年\d{2}月\d{2}日").Value;
            return cdate;
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
                //目录结构：当前目录/content/父ID目录/当前ID.html
                string filename = GetFilePath(page);
                using (FileStream fst = new FileStream(filename, FileMode.Append))
                {
                    //写数据到a.txt格式
                    using (StreamWriter swt = new StreamWriter(fst, System.Text.Encoding.GetEncoding("utf-8")))
                    {
                        swt.Write(content);
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 返回页面所对应的文件
        /// 文件存放规则为content为主目录
        /// page的父结点的ID为子目录
        /// page的ID为文件名
        /// </summary>
        /// <param name="page">需要下载的页面</param>
        /// <returns></returns>
        private static string GetFilePath(Page page)
        {
            //获得当前目录
            string dir = Directory.GetCurrentDirectory();
            if (dir.Substring(dir.Length - 1, 1) != @"\")
            {
                dir = dir + @"\";
            }

            dir += String.Format(@"content\{0}", page.Parent_ID);
            if (!Directory.Exists(dir))            //判断目录是否存在
            {
                Directory.CreateDirectory(dir); 
            }

            return string.Format(@"{0}\{1}.html", dir, page.Oid);// +@"\Content.html";
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



        // 保存页面到数据库里面
        protected void SavePage(object sender, BaseWeb.DownPageEventArgs e)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                foreach (Page page in e.m_pages)
                {
                    var fp = from p in ExistPages
                             where p.URL == page.URL
                             select p;
                    if (fp.Count() == 0)
                    {
                        Page newPage = new Page(uow) { Parent_ID = m_web.Oid, Title = page.Title, URL = page.URL };
                        newPage.Save();

                        //ExistPages.Add(newPage);        //  加入已经找到的页面
                    }
                }
                uow.CommitChanges();
            }
        }
    }
}
