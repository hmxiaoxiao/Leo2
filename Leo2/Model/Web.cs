using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;


namespace Leo2.Model
{
    /// <summary>
    /// 网站的实体类
    /// </summary>
    public class Web : XPObject
    {
        //public Web(Session session)
        //    : base(session)
        //{
        //}


        private string m_name;
        private string m_url;
        private int m_parent_id;
        private bool m_is_search;
        private string m_list_url_xpath;
        private string m_next_url_xpath;
        private string m_page_xpath;
        private int m_unread;


        public string Title
        {
            get { return m_unread > 0 ? m_name + "(" + m_unread.ToString() + ")" : m_name ; }
        }

        /// <summary>
        /// 网站结点名称
        /// </summary>
        [Size(255)]
        public string Name 
        {
            get { return m_name; }
            set { SetPropertyValue<string>("Name", ref m_name, value); }
        }

        /// <summary>
        /// 该结点对应的URL
        /// </summary>
        [Size(1000)]
        [Indexed(Unique=true)]
        public string URL
        {
            get { return m_url; }
            set { SetPropertyValue<string>("URL", ref m_url, value); }
        }


        /// <summary>
        /// 自己的父结点ID
        /// </summary>
        public int Parent_ID
        {
            get { return m_parent_id; }
            set { SetPropertyValue<int>("Parent_ID", ref m_parent_id, value); } 
        }

        /// <summary>
        /// 这个结点是否需要搜索，通常有子结点的，是不需要搜索的
        /// </summary>
        public bool Is_Search
        {
            get { return m_is_search; }
            set { SetPropertyValue<bool>("Is_Search", ref m_is_search, value); }
        }

        /// <summary>
        /// 列表页中，取得每个页面联接的XPath
        /// </summary>
        [Size(1000)]
        public string List_URL_XPath
        {
            get { return m_list_url_xpath; }
            set { SetPropertyValue<string>("List_URL_XPath", ref m_list_url_xpath, value); }
        }

        /// <summary>
        /// 列表页中，用于取得下一页的XPath路径
        /// </summary>
        [Size(1000)]
        public string Next_URL_XPath
        {
            get { return m_next_url_xpath; }
            set { SetPropertyValue<string>("Next_URL_XPath", ref m_next_url_xpath, value); }
        }

        /// <summary>
        /// page页中，用于提取网页的内容有XPath
        /// </summary>
        [Size(1000)]
        public string Page_XPath
        {
            get { return m_page_xpath; }
            set { SetPropertyValue<string>("Page_XPath", ref m_page_xpath, value); }
        }

        /// <summary>
        /// 未读数量
        /// </summary>
        public int Unread
        {
            get { return m_unread; }
            set { SetPropertyValue<int>("Unread", ref m_unread, value); }
        }

        /// <summary>
        /// 取得
        /// </summary>
        /// <param name="web_oid"></param>
        /// <returns></returns>
        public static Web CreateWebWithOID(int web_oid)
        {
            XPCollection<Web> webs = new XPCollection<Web>(XpoDefault.Session,
                Web.Fields.Oid == web_oid);
            if (webs.Count > 0)
                return webs[0];
            return null;
        }

        /// <summary>
        /// 初始化网站的内容
        /// </summary>
        public static void InitWebData()
        {
            XPCollection<Web> webs = new XPCollection<Web>(Web.Fields.Name == "网站列表");
            if (webs.Count == 0)
            {
                Web root = new Web();
                root.Parent_ID = 0;
                root.Name = "网站列表";
                root.Is_Search = false;
                root.Save();

                Web web0 = new Web();
                web0.Parent_ID = root.Oid;
                web0.Name = "国资委";
                web0.Is_Search = false;
                web0.Save();

                Web web01 = new Web();
                web01.Parent_ID = web0.Oid;
                web01.Name = "国资要闻";
                web01.URL = "http://www.sasac.gov.cn/n1180/n20240/n20259/index.html";
                web01.Is_Search = true;
                web01.List_URL_XPath = "//td[@class='black14']/a";
                web01.Next_URL_XPath = "";
                web01.m_page_xpath = "/html[1]/body[1]/table[2]/tr[1]/td[2]/table[11]/tr[1]/td[1]";
                web01.Save();

            }
        }


        /// <summary>
        /// 这个内部类是用来做查询的时候用的。
        /// </summary>
        public new class Fields
        {
            private Fields() { }
            public static OperandProperty Name
            {
                get { return new OperandProperty("Name"); }
            }

            public static OperandParameter Oid
            {
                get { return new OperandParameter("Oid"); }
            }
        }

    }
}
