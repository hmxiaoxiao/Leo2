using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Leo2.Model;
using Leo2.View;
using Leo2.Helper;
using DevExpress.Xpo;

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
        /// 构造函数，判断一下，WEB表里是否有一条根记录，没有的话就要增加
        /// </summary>
        public LeoController()
        {
            Web.InitWebData();
            Web.AddData();
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
            m_webs = new XPCollection<Web>();
            foreach (Web web in m_webs)
            {
                if (web.Oid == web_oid)
                {
                    ListHelper.GetAndSavePagesOnList(web, web.URL, update_all);
                    SetUnreadCount(web, ListHelper.PageList.Count);
                    PageHelper.GetAllContentWithSave();
                    return ListHelper.PageList;
                }
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
            XpoDefault.Session.BeginTransaction();

            // 先更新网页的已读标志
            string sql = "Update Page set is_read = 1 where oid = " + oid.ToString();
            XpoDefault.Session.ExecuteNonQuery(sql);

            // 再更新网站的未读数量
            sql = @"
Update Web set unread = (select count(*) 
                         from page where parent_id = (select parent_id from page where oid = {0})
                                         and is_read = 0 )
           where oid = (select parent_id from page where oid = {0} )";
            sql = string.Format(sql, oid);
            XpoDefault.Session.ExecuteNonQuery(sql);
            XpoDefault.Session.CommitTransaction();
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
            //XPQuery<Web> webQuery = new XPQuery<Web>(XpoDefault.Session);

            //// 测试国资委
            //var webs = from w in webQuery
            //           where w.Name == "集团新闻"
            //           select w;

            //ListHelper.PageList.Clear();
            //foreach (Web web in webs)
            //{
            //    ListHelper.GetAndSavePagesOnList(web, web.URL);
            //}
        }

        // 测试页面
        public void RunTestPage()
        {
            PageHelper.GetSingleContentWithSave(24483);
        }
    }
}
