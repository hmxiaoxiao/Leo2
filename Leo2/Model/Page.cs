using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using Leo2.Helper;
using System.IO;

namespace Leo2.Model
{
    /// <summary>
    /// 网页的实体类
    /// </summary>
    public class Page : XPObject
    {

        private string m_css = @"
body{font-size: 12pt; 
    line-height: 17pt;
}
.body{
    background:#E7F4FE;
}
td{font-FAMILY:宋体;font-size: 9pt; line-height: 160%}
.article_title{font-FAMILY:宋体;font-size: 19pt;  letter-spacing: 2px; line-height: 130%; margin: 6; font-weight: bold;}
.newfont2{font-FAMILY:宋体;font-size: 9pt; line-height: 12pt}
.newfont3{font-FAMILY:宋体;font-size: 9pt; line-height: 12pt}
.thetd{font-FAMILY:宋体;font-size: 12pt; color:#000000;line-height: 160%;font-weight:None;font-style: None}
INPUT
{
    BORDER-BOTTOM-COLOR: #cccccc;
    BORDER-BOTTOM-WIDTH: 1px;
    BORDER-LEFT-COLOR: #cccccc;
    BORDER-LEFT-WIDTH: 1px;
    BORDER-RIGHT-COLOR: #cccccc;
    BORDER-RIGHT-WIDTH: 1px;
    BORDER-TOP-COLOR: #cccccc;
    BORDER-TOP-WIDTH: 1px;
    FONT-FAMILY: 宋体;
    FONT-SIZE: 9pt;
    HEIGHT: 18px;
    PADDING-BOTTOM: 1px;
    PADDING-LEFT: 1px;
    PADDING-RIGHT: 1px;
    PADDING-TOP: 1px
}
a:hover {color: #FF0000}
a:link {text-decoration: none}
a:visited {text-decoration: none}
";
        private string m_template = @"
<HTML>
<HEAD>
<META http-equiv=Content-Type content='text/html; charset=utf8'>
<style type='text/css'> 
{2}
</style> 
</HEAD>
<BODY text=#000000 vLink=#990000 aLink=#990000 link=#990000 leftMargin=0 topMargin=0 marginheight=0 marginwidth=0 Bgcolor=#E7F4FE class=Body style='word-break:break-all'>

<table border='0' height='98' cellpadding='0' cellspacing='0' width=100% id=main> 
<tr> <td width='100%' align='left' valign='top'>
<br><br>
</TD></TR> <TR vAlign=center align=left> 

<TD height='295'  valign='top' align='left'> 
<TABLE cellSpacing=0 borderColorDark=#999999 cellPadding=0 align=center borderColorLight=#ffffff border='0' style='line-height:150%;' WIDTH='90%'> 
<TBODY> <TR vAlign=top align=left> 
<TD id=thetd class=thetd>
<div align='left' style='width: 100%; height: 132'> 
<p align='left'><center><font class='article_title'>{0}</font></center><br>
<hr size='1' noshade color='#FF9900'><span id='content'><!--BookContent Start-->{1}<!--BookContent End--><br><br></div>
</TD></TR> </TBODY> </TABLE></TD></TR> </TABLE></td>
    <td width='100%' valign='top'> 
      <div align='center'></div>
    </td>
  </tr> 
</table>
<br> 
</BODY></HTML>
";


        private string m_cdate;
        private int m_parent_id;
        private string m_title;
        private bool m_is_down;
        private bool m_is_read;
        private string m_url;
        private string m_content;


        //public Page() : base(XpoDefault.DataLayer) { }
        public Page(Session session) : base(session) { }
        //public Page(Session session, int parent_id, string url)
        //    : base(session)
        //{
        //    m_parent_id = parent_id;
        //    m_url = url;
        //}

        /// <summary>
        /// 网页的标题
        /// </summary>
        [Size(255)]
        public string Title
        {
            get { return m_title; }
            set { SetPropertyValue<string>("Title", ref m_title, value); }
        }

        /// <summary>
        /// 网页的发布日期
        /// </summary>
        [Size(255)]
        public string CDate
        {
            get { return m_cdate; }
            set { SetPropertyValue<string>("CDate", ref m_cdate, value); }
        }

        /// <summary>
        /// 属属网站结点
        /// </summary>
        public int Parent_ID
        {
            get { return m_parent_id; }
            set { SetPropertyValue<int>("Parent_ID", ref m_parent_id, value); }
        }

        /// <summary>
        /// 该页面是否已读
        /// </summary>
        public bool Is_Read
        {
            get { return m_is_read; }
            set { SetPropertyValue<bool>("Is_Read", ref m_is_read, value); }
        }

        /// <summary>
        /// 该页面是否已经下载
        /// </summary>
        public bool Is_Down
        {
            get { return m_is_down; }
            set { SetPropertyValue<bool>("Is_Down", ref m_is_down, value); }
        }


        /// <summary>
        /// 网页所在的URL
        /// </summary>
        [Size(255)]
        [Indexed(Unique=false)]
        public string URL
        {
            get { return m_url; }
            set { SetPropertyValue<string>("URL", ref m_url, value); }
        }

        /// <summary>
        /// 网页内容（已经截取的）
        /// </summary>
        [Size(SizeAttribute.Unlimited)]
        public string Content
        {
            get { return m_content; }
            set { SetPropertyValue<string>("Content", ref m_content, value); }
        }


        /// <summary>
        /// 设置当前网页已读
        /// </summary>
        public void ReadyRead()
        {
            this.Is_Read = true;
            this.Save();
        }

        private Web m_parentweb = null;
        // 取得当前网页的WEB
        public Web ParentWeb
        {
            get
            {
                if(m_parentweb == null)
                {
                    XPCollection<Web> webs = new XPCollection<Web>(new Session(XpoDefault.DataLayer),
                        CriteriaOperator.Parse("Oid = ?", this.m_parent_id));
                    if (webs.Count > 0)
                        m_parentweb = webs[0];
                }
                return m_parentweb;
            }
        }

        /// <summary>
        /// 加上模板的显示状态
        /// </summary>
        /// <returns></returns>
        public string DisplayContent()
        {
            m_content = GetContentFromFile(this);
            return string.Format(m_template, m_title, m_content, m_css);
        }


        /// <summary>
        /// 获得文件内容
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public static string GetContentFromFile(Page page)
        {
            string result = "";
            string filename = GetFilePath(page);
            //如果没有找到文件，就先下载
            if (!File.Exists(filename))
            {
                Leo2.Rule.BaseRule br = page.ParentWeb.Rule;
                if (br == null)
                    return "";

                return br.GetSingleContentWithSave(page);
            }
            else
            {
                StreamReader srt = new StreamReader(filename, Convert.ToBoolean(FileMode.Open));
                //存在
                result = srt.ReadToEnd();
                srt.Close();
                return result;
            }
        }


        /// <summary>
        /// 返回页面所对应的文件
        /// 文件存放规则为content为主目录
        /// page的父结点的ID为子目录
        /// page的ID为文件名
        /// </summary>
        /// <param name="page"></param>
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
    }
}
