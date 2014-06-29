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
        public delegate void PageScanCompleteEventHandler(object sender, ScanCompleteEventArgs e);
        public event PageScanCompleteEventHandler PageScanComplete;

        //public class PageScanCompleteEventArgs : EventArgs
        //{
        //    public readonly List<Page> pages;
        //    public readonly Web web;
        //    public PageScanCompleteEventArgs(List<Page> pages, Web web)
        //    {
        //        this.pages = pages;
        //        this.web = web;
        //    }
        //}


        // 定义事件，触发时，调用所有关注的事件。
        // 这里采用异步的联接方式，注意
        public virtual void OnPageScanComplete(ScanCompleteEventArgs e)
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
        public delegate void SiteScanCompleteEventHandle(object sender, ScanCompleteEventArgs e);
        public event SiteScanCompleteEventHandle SiteScanComplete;

        public class ScanCompleteEventArgs : EventArgs
        {
            public Web web;
            public readonly List<Page> pages;
            public ScanCompleteEventArgs(List<Page> pages, Web web)
            {
                this.web = web;
                this.pages = pages;
            }
        }

        public virtual void OnSiteScanComplete(ScanCompleteEventArgs e)
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

        protected string List_XPath { get; set; }       // 列表取数规则
        protected string Page_XPath { get; set; }       // 页面取数规则
        public List<Page> Pages { get; set; }           // 当前的页面数
        protected XPCollection<Page> ExistPages { get; set; }  // 已经搜索到的页面

        
        protected Web CurrentWeb { get; set; }          // 当前规则对应的Web

        private int m_max_page = -1;        // 记录最大的页数
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
        /// 具体实现由子类来完成
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
            CurrentWeb = web;
            Pages = new List<Page>();
            this.ExistPages = new XPCollection<Page>(XpoDefault.Session,
                    CriteriaOperator.Parse("Parent_ID = ?", web.Oid));

            // 开始下载时，自动保存  
            this.PageScanComplete += SaveScanResult;        // 每扫描好一页就进行保存
            //this.SiteScanComplete += SavePage;      
        }

        /// 准备开始扫描
        public bool PrepareScan(bool search_all = false)
        {
            // 触发开始扫描事件
            SiteScanBeginEventArgs e1 = new SiteScanBeginEventArgs(CurrentWeb);
            OnSiteScanBegin(e1);

            // 正式扫描
            return ScaningListPage(CurrentWeb.URL, search_all);
        }

        private bool ScaningListPage(string url, bool search_all)
        {          
            // 取得列表上的所有的Page
            List<Page> list = GetPagesOnList(url);

            // 触发单列表扫描完毕事件
            ScanCompleteEventArgs e2 = new ScanCompleteEventArgs(list, CurrentWeb);
            OnPageScanComplete(e2);

            // 如果该栏目没有全部扫描过，就继续扫描
            if (!(search_all &&
                NewPageInExistPages(list)))     // 或者新的页面没有出现在页面列表里（表明全部都是新页面，那就要扫描下一页了）
            {
                // 取得下一页 
                string next_url = GetNextLink(url);

                if (!string.IsNullOrEmpty(next_url))
                {
                    // 如果取得下一页地址，就继续扫描
                    return ScaningListPage(next_url, search_all);
                }
                else
                {
                    // 没有取得下一页地址
                    if (next_url == "")     // 空 表明全部搜索过了
                    {
                        //通知已经全部下载完成了。
                        ScanCompleteEventArgs e3 = new ScanCompleteEventArgs(new List<Page>(), CurrentWeb);
                        OnSiteScanComplete(e3);

                        return true;
                    }
                    else
                    {
                        //也要通知已经下载结束了了。
                        ScanCompleteEventArgs e4 = new ScanCompleteEventArgs(null, CurrentWeb);
                        OnSiteScanComplete(e4);

                        return false;
                    }
                }
            }

            //也要通知已经下载结束了了。
            ScanCompleteEventArgs e5 = new ScanCompleteEventArgs(null, CurrentWeb);
            OnSiteScanComplete(e5);

            return false;
        }



        /// 判断新取得的页面是否在新页面列表里存在
        /// <summary>
        /// 判断新取得的页面是否在新页面列表里存在
        /// </summary>
        /// <returns>存在，返回真，不存在，返回假</returns>
        private bool NewPageInExistPages(List<Page> list)
        {
            if (list.Count() <= 0)
                return false;

            List<string> filter = new List<string>();

            // 生成过滤条件
            foreach (Page page in list)
            {
                filter.Add(page.URL);
            }

            XPCollection<Page> pages = new XPCollection<Page>(XpoDefault.Session,
                new InOperator("URL", filter.ToArray()));
            if(pages.Count() > 0)
                return true;
            else
                return false;
        }


        /// 从列表中取得页面
        /// <summary>
        /// 保存指定列表中的所有的页面数据
        /// </summary>
        /// <param name="web"></param>
        /// <param name="list_url"></param>
        /// <returns>正常返回为真，读取网页内容出错，则为假</returns>
        public List<Page> GetPagesOnList(string list_url)
        {
            // 定义一个Session,仅为初始化Page实例而用
            Session session = XpoDefault.Session;
            List<Page> list = new List<Page>();

            // 取得列表面的内容
            HtmlDocument doc = WebHelper.GetHtmlDocument(list_url);

            // 如果没有取到内容，返回空列表
            if (doc == null) 
                return list;

            // 分析里面的结点
            HtmlNodeCollection lists = doc.DocumentNode.SelectNodes(this.List_XPath);

            // 取所有符合要求的结点
            foreach (HtmlNode node in lists)
            {
                // 生成一个新的page实例 
                Page page = new Page(session);

                // 取得联接
                page.URL = GeneRightURL(node.Attributes["href"].Value);

                // 网页的标题 
                if (node.Attributes["title"] != null)
                    page.Title = node.Attributes["title"].Value;
                else
                    page.Title = node.InnerText;

                list.Add(page);      // 加入查到的页面结点
            }
            return list;
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
            return "";      // 没有找到下一页，说明结束了
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
            Uri u = new Uri(this.CurrentWeb.URL);
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

        // 保存所有扫描出来的页面到数据库里面
        protected void SaveScanResult(object sender, BaseRule.ScanCompleteEventArgs e)
        {
            // 同一时间只能一个线程保存数据
            System.Threading.Mutex m = new System.Threading.Mutex(false, "SaveDB");
            m.WaitOne();
            using (UnitOfWork uow = new UnitOfWork(XpoDefault.DataLayer))       // 开始事务
            {
                uow.BeginTransaction();
                //  对于每个取得的新页面，判断是否已经存在，如果不存在就保存
                foreach (Page page in e.pages)      
                {
                    XPCollection<Page> pages = new XPCollection<Page>(uow,
                        CriteriaOperator.Parse("URL = ?", page.URL));
                    if (pages.Count() == 0)
                    {
                        Page newPage = new Page(uow) { Parent_ID = CurrentWeb.Oid, Title = page.Title, URL = page.URL };
                        newPage.Save();
                    }
                }
                uow.CommitChanges();
            }

            m.ReleaseMutex();
        }

        #region 页面下载的处理函数


        public string GetPageDate(Page p)
        {
            // 如果没有取得Page就退出
            if (p.ParentWeb == null)
                return "";

            HtmlDocument doc = WebHelper.GetHtmlDocument(p.URL, p.ParentWeb.Encoding);

            return GetDataFromContent(doc.DocumentNode.InnerHtml);
            //HtmlNodeCollection firstpage = doc.DocumentNode.SelectNodes(Page_XPath);

            //if (firstpage != null && firstpage.Count >= 1)
            //{
            //    return firstpage[0].InnerHtml;
            //}
            //return "";
        }


        /// <summary>
        /// 下载指定网页的内容
        /// </summary>
        /// <param name="p"></param>
        public string GetPageContent(Page p)
        {
            // 如果没有取得Page就退出
            if (p.ParentWeb == null)
                return "";

            HtmlDocument doc = WebHelper.GetHtmlDocument(p.URL, p.ParentWeb.Encoding);
            HtmlNodeCollection firstpage = doc.DocumentNode.SelectNodes(Page_XPath);

            if (firstpage != null && firstpage.Count >= 1)
            {
                return FilterImage(firstpage[0]);
            }
            return "";
        }


        // 将网页中的图片地址过滤一下。
        private string FilterImage(HtmlNode node)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(node.InnerHtml);
            HtmlNodeCollection nc = doc.DocumentNode.SelectNodes("//img");
            if (nc != null)
            {
                foreach (HtmlNode n in nc)
                {
                    //string src = GetRightImageURL(n.Attributes["src"].ToString());
                    //HtmlAttribute attr = new HtmlAttribute();
                    n.Attributes["src"].Value = GetRightImageURL(n.Attributes["src"].Value);
                }

                return doc.DocumentNode.InnerHtml;
            }
            else
            {
                return node.InnerHtml;
            }
        }


        protected string GetRightImageURL(string url)
        {
            Uri u = new Uri(this.CurrentWeb.URL);
            string web_root = "http://" + u.Authority;

            if (url.Substring(0, 1) == "/")        // 如果地址是以"/"开头，说明用的是相对地址，要加上网址
                return web_root + url;
            else
                return url;
        }


        /// 根据网页的内容取得该网页的发布日期
        /// <summary>
        /// 根据网页的内容取得该网页的发布日期
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private string GetDataFromContent(string content)
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

        #endregion
    }
}
