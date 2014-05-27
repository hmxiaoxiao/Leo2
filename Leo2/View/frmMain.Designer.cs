namespace Leo2.View
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            DevExpress.XtraTreeList.StyleFormatConditions.StyleFormatCondition styleFormatCondition4 = new DevExpress.XtraTreeList.StyleFormatConditions.StyleFormatCondition();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            DevExpress.XtraGrid.StyleFormatCondition styleFormatCondition6 = new DevExpress.XtraGrid.StyleFormatCondition();
            this.treeUnread = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.gridIsRead = new DevExpress.XtraGrid.Columns.GridColumn();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeList1 = new DevExpress.XtraTreeList.TreeList();
            this.treeOid = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeName = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeURL = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeParentID = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeIsSearch = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeListURLXPath = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeNextURLXPath = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treePageXPath = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeTitle = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.imageCollection1 = new DevExpress.Utils.ImageCollection(this.components);
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridTitle = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridCDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridIsDown = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridOid = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridParentID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.btnUpdateAll = new DevExpress.XtraBars.BarButtonItem();
            this.bar3 = new DevExpress.XtraBars.Bar();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.repositoryItemButtonEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.popupMenu1 = new DevExpress.XtraBars.PopupMenu(this.components);
            this.btnUpdate = new DevExpress.XtraBars.BarButtonItem();
            this.btnForceUpdateAll = new DevExpress.XtraBars.BarButtonItem();
            this.btnForceUpdate = new DevExpress.XtraBars.BarButtonItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeList1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).BeginInit();
            this.SuspendLayout();
            // 
            // treeUnread
            // 
            this.treeUnread.Caption = "treeUnread";
            this.treeUnread.FieldName = "Unread";
            this.treeUnread.Name = "treeUnread";
            // 
            // gridIsRead
            // 
            this.gridIsRead.Caption = "是否已读";
            this.gridIsRead.FieldName = "Is_Read";
            this.gridIsRead.Name = "gridIsRead";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(2, 33);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeList1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(755, 500);
            this.splitContainer1.SplitterDistance = 199;
            this.splitContainer1.TabIndex = 1;
            // 
            // treeList1
            // 
            this.treeList1.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.treeOid,
            this.treeName,
            this.treeURL,
            this.treeParentID,
            this.treeIsSearch,
            this.treeListURLXPath,
            this.treeNextURLXPath,
            this.treePageXPath,
            this.treeUnread,
            this.treeTitle});
            this.treeList1.Dock = System.Windows.Forms.DockStyle.Fill;
            styleFormatCondition4.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            styleFormatCondition4.Appearance.Options.UseFont = true;
            styleFormatCondition4.ApplyToRow = true;
            styleFormatCondition4.Column = this.treeUnread;
            styleFormatCondition4.Condition = DevExpress.XtraGrid.FormatConditionEnum.Greater;
            styleFormatCondition4.Value1 = ((long)(0));
            this.treeList1.FormatConditions.AddRange(new DevExpress.XtraTreeList.StyleFormatConditions.StyleFormatCondition[] {
            styleFormatCondition4});
            this.treeList1.Location = new System.Drawing.Point(0, 0);
            this.treeList1.Name = "treeList1";
            this.treeList1.BeginUnboundLoad();
            this.treeList1.AppendNode(new object[] {
            null,
            "网站列表",
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            "Hello"}, -1);
            this.treeList1.AppendNode(new object[] {
            null,
            "国资委",
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            "World"}, 0, 0, 1, -1);
            this.treeList1.EndUnboundLoad();
            this.treeList1.OptionsBehavior.AutoChangeParent = false;
            this.treeList1.OptionsBehavior.Editable = false;
            this.treeList1.OptionsBehavior.PopulateServiceColumns = true;
            this.treeList1.OptionsLayout.AddNewColumns = false;
            this.treeList1.OptionsView.ShowColumns = false;
            this.treeList1.OptionsView.ShowHorzLines = false;
            this.treeList1.OptionsView.ShowIndicator = false;
            this.treeList1.OptionsView.ShowVertLines = false;
            this.treeList1.ParentFieldName = "Parent_ID";
            this.treeList1.SelectImageList = this.imageCollection1;
            this.treeList1.Size = new System.Drawing.Size(199, 500);
            this.treeList1.TabIndex = 0;
            this.treeList1.Click += new System.EventHandler(this.treeList1_Click);
            this.treeList1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.treeList1_MouseUp);
            // 
            // treeOid
            // 
            this.treeOid.Caption = "OID";
            this.treeOid.FieldName = "Oid";
            this.treeOid.Name = "treeOid";
            // 
            // treeName
            // 
            this.treeName.Caption = "Name";
            this.treeName.FieldName = "Name";
            this.treeName.MinWidth = 69;
            this.treeName.Name = "treeName";
            // 
            // treeURL
            // 
            this.treeURL.Caption = "URL";
            this.treeURL.FieldName = "URL";
            this.treeURL.Name = "treeURL";
            // 
            // treeParentID
            // 
            this.treeParentID.Caption = "ParentID";
            this.treeParentID.FieldName = "Parent_ID";
            this.treeParentID.Name = "treeParentID";
            // 
            // treeIsSearch
            // 
            this.treeIsSearch.Caption = "IsSearch";
            this.treeIsSearch.FieldName = "Is_Search";
            this.treeIsSearch.Name = "treeIsSearch";
            // 
            // treeListURLXPath
            // 
            this.treeListURLXPath.Caption = "ListURLXPath";
            this.treeListURLXPath.FieldName = "List_URL_XPath";
            this.treeListURLXPath.Name = "treeListURLXPath";
            // 
            // treeNextURLXPath
            // 
            this.treeNextURLXPath.Caption = "NextURLXPath";
            this.treeNextURLXPath.FieldName = "Next_URL_XPath";
            this.treeNextURLXPath.Name = "treeNextURLXPath";
            // 
            // treePageXPath
            // 
            this.treePageXPath.Caption = "PageXPath";
            this.treePageXPath.FieldName = "Page_XPath";
            this.treePageXPath.Name = "treePageXPath";
            // 
            // treeTitle
            // 
            this.treeTitle.Caption = "treeTitle";
            this.treeTitle.FieldName = "Title";
            this.treeTitle.MinWidth = 69;
            this.treeTitle.Name = "treeTitle";
            this.treeTitle.Visible = true;
            this.treeTitle.VisibleIndex = 0;
            this.treeTitle.Width = 128;
            // 
            // imageCollection1
            // 
            this.imageCollection1.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imageCollection1.ImageStream")));
            this.imageCollection1.Images.SetKeyName(0, "Folder.png");
            this.imageCollection1.Images.SetKeyName(1, "Folder-Search.png");
            this.imageCollection1.Images.SetKeyName(2, "Folder-Closed.png");
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.gridControl1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.webBrowser1);
            this.splitContainer2.Size = new System.Drawing.Size(552, 500);
            this.splitContainer2.SplitterDistance = 132;
            this.splitContainer2.TabIndex = 0;
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(0, 0);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.MenuManager = this.barManager1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(552, 132);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            this.gridControl1.Click += new System.EventHandler(this.gridControl1_Click);
            // 
            // gridView1
            // 
            this.gridView1.Appearance.FocusedCell.BackColor = System.Drawing.Color.Navy;
            this.gridView1.Appearance.FocusedCell.ForeColor = System.Drawing.Color.White;
            this.gridView1.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gridView1.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gridView1.Appearance.FocusedRow.BackColor = System.Drawing.Color.Navy;
            this.gridView1.Appearance.FocusedRow.ForeColor = System.Drawing.Color.White;
            this.gridView1.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gridView1.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridTitle,
            this.gridCDate,
            this.gridIsDown,
            this.gridIsRead,
            this.gridOid,
            this.gridParentID});
            styleFormatCondition6.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            styleFormatCondition6.Appearance.Options.UseFont = true;
            styleFormatCondition6.ApplyToRow = true;
            styleFormatCondition6.Column = this.gridIsRead;
            styleFormatCondition6.Condition = DevExpress.XtraGrid.FormatConditionEnum.Equal;
            styleFormatCondition6.Value1 = false;
            this.gridView1.FormatConditions.AddRange(new DevExpress.XtraGrid.StyleFormatCondition[] {
            styleFormatCondition6});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.Click;
            this.gridView1.OptionsSelection.MultiSelect = true;
            this.gridView1.OptionsView.EnableAppearanceEvenRow = true;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // gridTitle
            // 
            this.gridTitle.Caption = "标题";
            this.gridTitle.FieldName = "Title";
            this.gridTitle.Name = "gridTitle";
            this.gridTitle.Visible = true;
            this.gridTitle.VisibleIndex = 0;
            this.gridTitle.Width = 446;
            // 
            // gridCDate
            // 
            this.gridCDate.Caption = "日期";
            this.gridCDate.FieldName = "CDate";
            this.gridCDate.Name = "gridCDate";
            this.gridCDate.Visible = true;
            this.gridCDate.VisibleIndex = 1;
            // 
            // gridIsDown
            // 
            this.gridIsDown.Caption = "是否下载";
            this.gridIsDown.FieldName = "Is_Down";
            this.gridIsDown.Name = "gridIsDown";
            // 
            // gridOid
            // 
            this.gridOid.Caption = "Oid";
            this.gridOid.FieldName = "Oid";
            this.gridOid.Name = "gridOid";
            // 
            // gridParentID
            // 
            this.gridParentID.Caption = "parentid";
            this.gridParentID.FieldName = "Parent_ID";
            this.gridParentID.Name = "gridParentID";
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar1,
            this.bar3});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barButtonItem1,
            this.barButtonItem2,
            this.btnUpdateAll,
            this.btnUpdate,
            this.btnForceUpdateAll,
            this.btnForceUpdate});
            this.barManager1.MaxItemId = 8;
            this.barManager1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemButtonEdit1});
            this.barManager1.StatusBar = this.bar3;
            // 
            // bar1
            // 
            this.bar1.BarName = "Tools";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnUpdateAll, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnUpdate, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnForceUpdateAll, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnForceUpdate, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar1.OptionsBar.AllowQuickCustomization = false;
            this.bar1.OptionsBar.DisableClose = true;
            this.bar1.OptionsBar.DisableCustomization = true;
            this.bar1.Text = "Tools";
            // 
            // btnUpdateAll
            // 
            this.btnUpdateAll.Caption = "全部更新";
            this.btnUpdateAll.Glyph = ((System.Drawing.Image)(resources.GetObject("btnUpdateAll.Glyph")));
            this.btnUpdateAll.Id = 3;
            this.btnUpdateAll.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("btnUpdateAll.LargeGlyph")));
            this.btnUpdateAll.Name = "btnUpdateAll";
            this.btnUpdateAll.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnUpdateAll_ItemClick);
            // 
            // bar3
            // 
            this.bar3.BarName = "Status bar";
            this.bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.bar3.DockCol = 0;
            this.bar3.DockRow = 0;
            this.bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar3.OptionsBar.AllowQuickCustomization = false;
            this.bar3.OptionsBar.DrawDragBorder = false;
            this.bar3.OptionsBar.UseWholeRow = true;
            this.bar3.Text = "Status bar";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(2, 2);
            this.barDockControlTop.Size = new System.Drawing.Size(755, 31);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(2, 533);
            this.barDockControlBottom.Size = new System.Drawing.Size(755, 23);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(2, 33);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 500);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(757, 33);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 500);
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "增加网站";
            this.barButtonItem1.Glyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem1.Glyph")));
            this.barButtonItem1.Id = 0;
            this.barButtonItem1.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem1.LargeGlyph")));
            this.barButtonItem1.Name = "barButtonItem1";
            // 
            // barButtonItem2
            // 
            this.barButtonItem2.Caption = "删除网站";
            this.barButtonItem2.Glyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem2.Glyph")));
            this.barButtonItem2.Id = 1;
            this.barButtonItem2.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem2.LargeGlyph")));
            this.barButtonItem2.Name = "barButtonItem2";
            // 
            // repositoryItemButtonEdit1
            // 
            this.repositoryItemButtonEdit1.AutoHeight = false;
            this.repositoryItemButtonEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.repositoryItemButtonEdit1.Name = "repositoryItemButtonEdit1";
            // 
            // webBrowser1
            // 
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.Location = new System.Drawing.Point(0, 0);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(552, 364);
            this.webBrowser1.TabIndex = 0;
            // 
            // popupMenu1
            // 
            this.popupMenu1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.btnUpdateAll),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnUpdate),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnForceUpdateAll, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnForceUpdate)});
            this.popupMenu1.Manager = this.barManager1;
            this.popupMenu1.Name = "popupMenu1";
            // 
            // btnUpdate
            // 
            this.btnUpdate.Caption = "更新";
            this.btnUpdate.Glyph = ((System.Drawing.Image)(resources.GetObject("btnUpdate.Glyph")));
            this.btnUpdate.Id = 5;
            this.btnUpdate.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("btnUpdate.LargeGlyph")));
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnUpdate_ItemClick);
            // 
            // btnForceUpdateAll
            // 
            this.btnForceUpdateAll.Caption = "强制全部更新";
            this.btnForceUpdateAll.Glyph = ((System.Drawing.Image)(resources.GetObject("btnForceUpdateAll.Glyph")));
            this.btnForceUpdateAll.Id = 6;
            this.btnForceUpdateAll.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("btnForceUpdateAll.LargeGlyph")));
            this.btnForceUpdateAll.Name = "btnForceUpdateAll";
            this.btnForceUpdateAll.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnForceUpdateAll_ItemClick);
            // 
            // btnForceUpdate
            // 
            this.btnForceUpdate.Caption = "强制更新";
            this.btnForceUpdate.Glyph = ((System.Drawing.Image)(resources.GetObject("btnForceUpdate.Glyph")));
            this.btnForceUpdate.Id = 7;
            this.btnForceUpdate.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("btnForceUpdate.LargeGlyph")));
            this.btnForceUpdate.Name = "btnForceUpdate";
            this.btnForceUpdate.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnForceUpdate_ItemClick);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(759, 558);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMain";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.Text = "网站下载阅读器";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treeList1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).EndInit();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.Bar bar3;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraTreeList.TreeList treeList1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraBars.BarButtonItem btnUpdateAll;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repositoryItemButtonEdit1;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeOid;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeName;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeURL;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeParentID;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeIsSearch;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListURLXPath;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeNextURLXPath;
        private DevExpress.Utils.ImageCollection imageCollection1;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treePageXPath;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeUnread;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeTitle;
        private DevExpress.XtraGrid.Columns.GridColumn gridTitle;
        private DevExpress.XtraGrid.Columns.GridColumn gridCDate;
        private DevExpress.XtraGrid.Columns.GridColumn gridIsDown;
        private DevExpress.XtraGrid.Columns.GridColumn gridIsRead;
        private DevExpress.XtraGrid.Columns.GridColumn gridOid;
        private DevExpress.XtraGrid.Columns.GridColumn gridParentID;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private DevExpress.XtraBars.BarButtonItem btnUpdate;
        private DevExpress.XtraBars.BarButtonItem btnForceUpdateAll;
        private DevExpress.XtraBars.BarButtonItem btnForceUpdate;
        private DevExpress.XtraBars.PopupMenu popupMenu1;

    }
}