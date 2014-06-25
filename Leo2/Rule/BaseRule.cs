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
using Leo2.Helper;

namespace Leo2.Rule
{
    /// <summary>
    /// 基本的网页下载基类，完成整个网页的列表，下一页，以及内容的保存
    /// </summary>
    public class BaseRule
    {

        #region 声明事件和委托

        #region  单网站网页扫描开始
        public delegate void SiteScanBeginHandler(object sender, SiteScanBeginEventArgs e);
        public event SiteScanBeginHandler SiteScanBegin;
        public class SiteScanBeginEventArgs : EventArgs {
            public readonly Web web;
            public SiteScanBeginEventArgs(Web web)
            {
                this.web = web;
            }
        }
        public virtual void OnSiteScanBegin(SiteScanBeginEventArgs e)
        {
            if (SiteScanBegin != null)
            {
                Delegate[] delArray = SiteScanBegin.GetInvocationList();

                foreach (Delegate del in delArray)
                {
                    SiteScanBeginHandler method = (SiteScanBeginHandler)del;
                    method.BeginInvoke(this, e, null, null);
                }
            }
        }
        #endregion

        #region 列表页面扫描完成后的事件
        public delegate void PageScanCompleteEventHandler(object sender, PageScanCompleteEventArgs e);
        public event PageScanCompleteEventHandler PageScanComplete;

        public class PageScanCompleteEventArgs : EventArgs
        {
            public readonly List<Page> pages;
            public readonly Web web;
            public PageScanCompleteEventArgs(List<Page> pages, Web web)
            {
                this.pages = pages;
                this.web = web;
            }
        }


        // 定义事件，触发时，调用所有关注的事件。
        // 这里采用异步的联接方式，注意
        public virtual void OnPageScanComplete(PageScanCompleteEventArgs e)
        {
            if (PageScanComplete != null)
            {
                Delegate[] delArray = PageScanComplete.GetInvocationList();

                foreach (Delegate del in delArray)
                {
                    PageScanCompleteEventHandler method = (PageScanCompleteEventHandler)del;
                    method.BeginInvoke(this, e, null, null);
                }
            }
        }
        #endregion

        #region 单网站扫描完成事件
        public delegate void SiteScanCompleteEventHandle(object sender, SiteScanCompleteEventArgs e);
        public event SiteScanCompleteEventHandle SiteScanComplete;

        public class SiteScanCompleteEventArgs : EventArgs
        {
            public Web web;
            public readonly List<Page> pages;
            public SiteScanCompleteEventArgs(List<Page> pages, Web web)
            {
                this.web = web;
                this.pages = pages;
            }
        }

        public virtual void OnSiteScanComplete(SiteScanCompleteEventArgs e)
        {
            if (SiteScanComplete != null)
            {
                Delegate[] delArray = SiteScanComplete.GetInvocationList();

                foreach (Delegate del in delArray)
                {
                    SiteScanCompleteEventHandle method = (SiteScanCompleteEventHandle)del;
                    method.BeginInvoke(this, e, null, null);
                }
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
                m_web.MaxCount = m_max_page;
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
        public BaseRule(Web web)
        {
            m_web = web;
            Pages = new List<Page>();
            int itemp = MaxPage;        // 计算一下页面总数
            this.ExistPages = new XPCollection<Page>(XpoDefault.Session,
                    CriteriaOperator.Parse("Parent_ID = ?", web.Oid));

            //this.PageScanComplete += SavePage;      // 开始下载时，自动保存
            this.SiteScanComplete += SavePage;
        }

        /// 取得单网站的扫描
        public bool SingleSiteScan(string url = "", bool search_all = false)
        {
            // 如果下载的地址为空的话就取WEB的地址（第一次调用不用加的）
            if (string.IsNullOrEmpty(url))
                url = m_web.URL;

            // 触发开始扫描事件
            SiteScanBeginEventArgs e1 = new SiteScanBeginEventArgs(m_web);
            OnSiteScanBegin(e1);

            return NextPageScan(url, search_all);
        }

        private bool NextPageScan(string url, bool search_all)
        {
            // 下载该列表下的page联接
            GetPagesOnList(url);

            // 触发事件
            PageScanCompleteEventArgs e2 = new PageScanCompleteEventArgs(Pages, m_web);
            OnPageScanComplete(e2);

            // 如果该栏目没有全部扫描过，就继续扫描
            if (!(search_all &&
                NewPageInExistPages()))     // 或者新的页面没有出现在页面列表里（表明全部都是新页面，那就要扫描下一页了）
            {

                string next_url = GetNextLink(url);
                if (!string.IsNullOrEmpty(next_url))
                {
                    // 如果取得下一页地址，就继续扫描
                    return NextPageScan(next_url, search_all);
                }
                else
                {
                    // 没有取得下一页地址
                    if (next_url == "")     // 空 表明全部搜索过了
                    {
                        //通知已经全部下载完成了。
                        SiteScanCompleteEventArgs e3 = new SiteScanCompleteEventArgs(Pages,m_web);
                        OnSiteScanComplete(e3);

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
        public static string GetFilePath(Page page)
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
        protected void SavePage(object sender, BaseRule.SiteScanCompleteEventArgs e)
        {
            using (UnitOfWork uow = new UnitOfWork())       // 开始事务
            {
                //  对于每个取得的新页面，判断是否已经存在，如果不存在就保存
                foreach (Page page in e.pages)      
                {
                    var fp = from p in ExistPages
                             where p.URL == page.URL
                             select p;
                    if (fp.Count() == 0)
                    {
                        Page newPage = new Page(uow) { Parent_ID = m_web.Oid, Title = page.Title, URL = page.URL };
                        newPage.Save();
                    }
                }
                uow.CommitChanges();
            }
        }
    }
}
