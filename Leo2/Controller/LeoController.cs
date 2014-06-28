using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Leo2.Model;
using Leo2.View;
using Leo2.Helper;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using Leo2.Rule;
using System.Threading.Tasks;
using System.Threading;

using DevExpress.Xpo.DB;
using System.Reflection;

namespace Leo2.Controller
{
    public class LeoController : IDisposable
    {
        private XPCollection<Web> m_webs;

        private frmMain m_view;
        public frmMain View
        {
            get { return m_view; }
            set { m_view = value; }
        }


        public void Dispose()
        {
            if (m_webs != null)
            {
                m_webs.Dispose();
                m_webs = null;
            }
            if (m_view != null)
            {
                m_view.Dispose();
                m_view = null;
            }
        }

        /// <summary>
        /// 取得所有的网站的数据
        /// </summary>
        /// <returns></returns>
        public XPCollection<Web> GetAllWebs()
        {
            return new XPCollection<Web>(XpoDefault.Session);
        }


        ///// <summary>
        ///// 下载指定的联接
        ///// </summary>
        ///// <param name="web_oid"></param>
        ///// <returns></returns>
        //public List<Page> DownloadPageFromURL(int web_oid, bool update_all = false)
        //{
        //    var webs = from w in Session.DefaultSession.Query<Web>()
        //               where w.Oid == web_oid
        //               select w;
        //    foreach (Web web in webs)
        //    {
        //        ListHelper.PageList = new List<Page>();
        //        ListHelper.GetAndSavePagesOnList(web, web.URL, update_all);
        //        SetUnreadCount(web, ListHelper.PageList.Count);
        //        //PageHelper.GetAllContentWithSave();
        //        return ListHelper.PageList;

        //    }

        //    return new List<Page>();
        //}

        ///// <summary>
        ///// 设置当前结点未读的数量
        ///// </summary>
        ///// <param name="web"></param>
        ///// <param name="unread"></param>
        //private void SetUnreadCount(Web web, int unread)
        //{
        //    web.Unread += unread;
        //    web.Save();
        //}


        /// <summary>
        /// 设置ID的页面为已读
        /// </summary>
        /// <param name="oid"></param>
        public void SetPageHasRead(int oid)
        {
            // 先检查参数是否正确
            Session session = XpoDefault.Session;
            Page current_page = session.GetObjectByKey<Page>(oid);
            if(current_page == null || current_page.ParentWeb == null)
                return;

            // 同一时间只能一个线程保存数据
            System.Threading.Mutex m = new System.Threading.Mutex(false, "SaveDB");
            m.WaitOne();


            // 开始事务
            using (UnitOfWork uow = new UnitOfWork(XpoDefault.DataLayer))
            {

                // 设置页面已读
                current_page.Is_Read = true;
                current_page.Save();

                // 再更新web的未读数量
                XPCollection<Page> list = new XPCollection<Page>(XpoDefault.Session,
                    CriteriaOperator.Parse("Parent_ID = ? and Is_Read = ?", current_page.Parent_ID, false));
                Web web = session.GetObjectByKey<Web>(current_page.Parent_ID);
                web.Unread = list.Count();
                web.Save();

                uow.CommitChanges();
            }

            m.ReleaseMutex();       // 释放锁
        }

        /// <summary>
        /// 取得总的子页面数
        /// </summary>
        /// <param name="oid"></param>
        /// <returns></returns>
        public XPCollection<Page> GetSubPages(int oid)
        {
            return new XPCollection<Page>(XpoDefault.Session, CriteriaOperator.Parse("Parent_ID = ?", oid));
        }


        /// <summary>
        /// 构造函数，判断一下，WEB表里是否有一条根记录，没有的话就要增加
        /// </summary>
        public LeoController()
        {
            Web.InitWebData();
            //Web.AddData();
        }

        #region  Page下载结束事件
        public delegate void PageDownloadCompleteHandler(object sender, PageDownloadCompleteEventArgs e);
        public event PageDownloadCompleteHandler PageDownloadComplete;
        public class PageDownloadCompleteEventArgs : EventArgs
        {
            public readonly Page page;
            public PageDownloadCompleteEventArgs(Page page)
            {
                this.page = page;
            }
        }
        public virtual void OnPageDownloadComplete(PageDownloadCompleteEventArgs e)
        {
            if (PageDownloadComplete != null)
            {
                Delegate[] delArray = PageDownloadComplete.GetInvocationList();

                foreach (Delegate del in delArray)
                {
                    PageDownloadCompleteHandler method = (PageDownloadCompleteHandler)del;
                    method.BeginInvoke(this, e, null, null);
                }
            }
        }
        #endregion

        #region  Page线程结束
        public delegate void AllPageDownloadCompleteHandler(object sender, EventArgs e);
        public event AllPageDownloadCompleteHandler AllPageDownloadComplete;

        public virtual void OnAllPageDownloadComplete(EventArgs e)
        {
            if (AllPageDownloadComplete != null)
            {
                Delegate[] delArray = AllPageDownloadComplete.GetInvocationList();

                foreach (Delegate del in delArray)
                {
                    AllPageDownloadCompleteHandler method = (AllPageDownloadCompleteHandler)del;
                    method.BeginInvoke(this, e, null, null);
                }
            }
        }
        #endregion

        // 批量下载所有的page的内容
        public bool DownloadPageContent(CancellationToken ct)
        {
            XPCollection<Page> pages = new XPCollection<Page>(XpoDefault.Session, CriteriaOperator.Parse("Is_Down = ?", false));
            foreach(Page p in pages)
            {
                if (ct.IsCancellationRequested)
                {
                    OnAllPageDownloadComplete(new EventArgs());     // 通知下载结束
                    return false;       // 被取消后的退出
                }
                else
                {
                    p.ParentWeb.Rule.GetPageContent(p);

                    // 发出下载完成事件
                    PageDownloadCompleteEventArgs e = new PageDownloadCompleteEventArgs(p);
                    OnPageDownloadComplete(e);
                }
            }
            OnAllPageDownloadComplete(new EventArgs());     // 通知下载结束
            return true;            // 下载完成后退出
        }


        public static ThreadSafeDataLayer GetThreadSafeDataLayer()
        {
            string ConnectionString = AccessConnectionProvider.GetConnectionString("Web.mdb"); //SQLiteConnectionProvider.GetConnectionString("Web.DB");

            // 加入线程安全
            List<Assembly> dataAssemblies = new List<Assembly>();
            dataAssemblies.Add(typeof(Leo2.Model.Page).Assembly);
            dataAssemblies.Add(typeof(Leo2.Model.Web).Assembly);

            //XpoDefault.ConnectionString = AccessConnectionProvider.GetConnectionString("data.mdb");
            //var dataStore = XpoDefault.GetConnectionProvider(AutoCreateOption.DatabaseAndSchema);

            //var dict = new ReflectionDictionary();
            //dict.CollectClassInfos(dataAssemblies.ToArray());
            //var dataLayer = new ThreadSafeDataLayer(dict, dataStore);

            DevExpress.Xpo.Metadata.XPDictionary dict = new DevExpress.Xpo.Metadata.ReflectionDictionary();
            IDataStore store = XpoDefault.GetConnectionProvider(ConnectionString, AutoCreateOption.SchemaAlreadyExists);
            dict.GetDataStoreSchema(dataAssemblies.ToArray());
            return new ThreadSafeDataLayer(dict, store);
        }

        public static void InitDatabase()
        {
            // Code that runs on the application startup
            // Specify the connection string, which is used to open a database.
            // It's supposed that you've already created the Comments database within the App_Data folder.
            string conn = DevExpress.Xpo.DB.AccessConnectionProvider.GetConnectionString("Web.mdb");
            DevExpress.Xpo.Metadata.XPDictionary dict = new DevExpress.Xpo.Metadata.ReflectionDictionary();
            // Initialize the XPO dictionary.
            dict.GetDataStoreSchema(typeof(Page).Assembly);
            dict.GetDataStoreSchema(typeof(Web).Assembly);
            DevExpress.Xpo.XpoDefault.Session = null;
            DevExpress.Xpo.DB.IDataStore store = DevExpress.Xpo.XpoDefault.GetConnectionProvider(conn, DevExpress.Xpo.DB.AutoCreateOption.DatabaseAndSchema);
            DevExpress.Xpo.XpoDefault.DataLayer = new DevExpress.Xpo.ThreadSafeDataLayer(dict, store);
            DevExpress.Xpo.XpoDefault.Session = new Session(XpoDefault.DataLayer);

            //// 设置当前的数据库联接
            //string ConnectionString = AccessConnectionProvider.GetConnectionString("Web.mdb"); //SQLiteConnectionProvider.GetConnectionString("Web.DB");
            ////XpoDefault.DataLayer = XpoDefault.GetDataLayer(ConnectionString, AutoCreateOption.DatabaseAndSchema);


            //// 加入线程安全
            //List<Assembly> dataAssemblies = new List<Assembly>();
            //dataAssemblies.Add(typeof(Leo2.Model.Page).Assembly);
            //dataAssemblies.Add(typeof(Leo2.Model.Web).Assembly);

            ////XpoDefault.ConnectionString = AccessConnectionProvider.GetConnectionString("data.mdb");
            ////var dataStore = XpoDefault.GetConnectionProvider(AutoCreateOption.DatabaseAndSchema);

            ////var dict = new ReflectionDictionary();
            ////dict.CollectClassInfos(dataAssemblies.ToArray());
            ////var dataLayer = new ThreadSafeDataLayer(dict, dataStore);

            //DevExpress.Xpo.Metadata.XPDictionary dict = new DevExpress.Xpo.Metadata.ReflectionDictionary();
            //IDataStore store = XpoDefault.GetConnectionProvider(ConnectionString, AutoCreateOption.SchemaAlreadyExists);
            //dict.GetDataStoreSchema(dataAssemblies.ToArray());
            //ThreadSafeDataLayer datalayer = new ThreadSafeDataLayer(dict, store);
            //XpoDefault.DataLayer = datalayer;
        }


        public void RunTest()
        {
            RunTestLink();
            RunTestPage();

            Console.WriteLine("测试完成,按任意键退出！");
            Console.ReadKey();
        }


        //测试联接
        public void RunTestLink()
        {
            return;
        }

        // 测试页面
        public void RunTestPage()
        {
            return;
        }
    }
}
