﻿using System;
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
            return new XPCollection<Web>();
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
            XpoDefault.Session.BeginTransaction();      // 开始一个事务

            // 先更新网页的已读标志
            XPQuery<Page> all_pages = Session.DefaultSession.Query<Page>();
            XPQuery<Web> all_webs = Session.DefaultSession.Query<Web>();

            var pages = from p in all_pages
                               where p.Oid == oid
                               select p;
            if(pages.Count() == 1)       // 先更新page已读
            {
                Page current_page = pages.First<Page>();
                current_page.Is_Read = true;
                current_page.Save();

                var webs = from w in all_webs
                           where w.Oid == current_page.Parent_ID
                           select w;
                if (webs.Count() == 1)  // 再更新web的已读数量
                {
                    var unread_pages = from p in all_pages
                                        where p.Parent_ID == current_page.Parent_ID && p.Is_Read == false
                                        select p;
                    Web current_web = webs.First();
                    current_web.Unread = unread_pages.Count();
                    current_web.Save();

                }
            }

            XpoDefault.Session.CommitTransaction();     // 提交事务
        }

        /// <summary>
        /// 取得总的子页面数
        /// </summary>
        /// <param name="oid"></param>
        /// <returns></returns>
        public XPCollection<Page> GetSubPages(int oid)
        {
            return new XPCollection<Page>(CriteriaOperator.Parse("Parent_ID = ?", oid));
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
            XPCollection<Page> pages = new XPCollection<Page>(CriteriaOperator.Parse("Is_Down = ?", false));
            foreach(Page p in pages)
            {
                if (ct.IsCancellationRequested)
                {
                    OnAllPageDownloadComplete(new EventArgs());     // 通知下载结束
                    return false;       // 被取消后的退出
                }
                else
                {
                    p.ParentWeb.Rule.GetSingleContentWithSave(p);

                    // 发出下载完成事件
                    PageDownloadCompleteEventArgs e = new PageDownloadCompleteEventArgs(p);
                    OnPageDownloadComplete(e);
                }
            }
            OnAllPageDownloadComplete(new EventArgs());     // 通知下载结束
            return true;            // 下载完成后退出
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
