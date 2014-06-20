using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Leo2.Model;
using Leo2.View;
using Leo2.Helper;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;

namespace Leo2.Controller
{
    public class LeoController
    {
        private XPCollection<Web> m_webs;

        private frmMain m_view;
        public frmMain View
        {
            get { return m_view; }
            set { m_view = value; }
        }


        /// <summary>
        /// 取得所有的网站的数据
        /// </summary>
        /// <returns></returns>
        public XPCollection<Web> GetAllWebs()
        {
            return new XPCollection<Web>();
        }


        /// <summary>
        /// 下载指定的联接
        /// </summary>
        /// <param name="web_oid"></param>
        /// <returns></returns>
        public List<Page> DownloadPageFromURL(int web_oid, bool update_all = false)
        {
            var webs = from w in Session.DefaultSession.Query<Web>()
                       where w.Oid == web_oid
                       select w;
            foreach (Web web in webs)
            {
                ListHelper.PageList = new List<Page>();
                ListHelper.GetAndSavePagesOnList(web, web.URL, update_all);
                SetUnreadCount(web, ListHelper.PageList.Count);
                //PageHelper.GetAllContentWithSave();
                return ListHelper.PageList;

            }

            return new List<Page>();
        }

        /// <summary>
        /// 设置当前结点未读的数量
        /// </summary>
        /// <param name="web"></param>
        /// <param name="unread"></param>
        private void SetUnreadCount(Web web, int unread)
        {
            web.Unread += unread;
            web.Save();
        }


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
            return Page.GetSubPages(oid);
        }


        /// <summary>
        /// 构造函数，判断一下，WEB表里是否有一条根记录，没有的话就要增加
        /// </summary>
        public LeoController()
        {
            Web.InitWebData();
            //Web.AddData();
        }

        public BaseWeb GetRuleFromWeb(Web web)
        {
            return new www_sasac_gov_cn(web);
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
            XPQuery<Web> webQuery = new XPQuery<Web>(XpoDefault.Session);

            // 测试国资委
            var webs = from w in webQuery
                       where w.Name == "中国文化产业发展集团公司"
                       select w;

            ListHelper.PageList.Clear();
            foreach (Web web in webs)
            {
                ListHelper.GetAndSavePagesOnList(web, web.URL);
            }
        }

        // 测试页面
        public void RunTestPage()
        {
            PageHelper.GetSingleContentWithSave(29587);
        }
    }
}
