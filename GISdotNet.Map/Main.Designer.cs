namespace GISdotNet.Map
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnFileOpen = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.StatusLine = new System.Windows.Forms.StatusStrip();
            this.slMap1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.slMap2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.slMap3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.OpenMapDialog = new System.Windows.Forms.OpenFileDialog();
            this.pntConvert = new AxaxGisToolKit.AxaxMapPoint();
            this.MapPoint = new AxaxGisToolKit.AxaxMapPoint();
            this.MobilObj = new AxaxGisToolKit.AxaxMapObj();
            this.mvRsc = new AxaxGisToolKit.AxaxMapRsc();
            this.axMapScreen = new AxaxGisToolKit.AxaxcMapScreen();
            this.VectorObj = new AxaxGisToolKit.AxaxMapObj();
            this.RlsObj = new AxaxGisToolKit.AxaxMapObj();
            this.RlsZoneObj = new AxaxGisToolKit.AxaxMapObj();
            this.MapFind = new AxaxGisToolKit.AxaxMapFind();
            this.SearchableObj = new AxaxGisToolKit.AxaxMapObj();
            this.TextObj = new AxaxGisToolKit.AxaxMapObj();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.TargetNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HcNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Range = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Azimuth = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Height = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Velocity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Source = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Corr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.serviceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1.SuspendLayout();
            this.StatusLine.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pntConvert)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MapPoint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MobilObj)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mvRsc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axMapScreen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VectorObj)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RlsObj)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RlsZoneObj)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MapFind)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SearchableObj)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TextObj)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnFileOpen,
            this.toolStripSeparator1,
            this.toolStripSeparator2});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(757, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnFileOpen
            // 
            this.btnFileOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnFileOpen.Image = ((System.Drawing.Image)(resources.GetObject("btnFileOpen.Image")));
            this.btnFileOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnFileOpen.Name = "btnFileOpen";
            this.btnFileOpen.Size = new System.Drawing.Size(23, 22);
            this.btnFileOpen.Text = "Открыть файл ЭК";
            this.btnFileOpen.Click += new System.EventHandler(this.btnFileOpen_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // StatusLine
            // 
            this.StatusLine.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.slMap1,
            this.slMap2,
            this.slMap3});
            this.StatusLine.Location = new System.Drawing.Point(0, 496);
            this.StatusLine.Name = "StatusLine";
            this.StatusLine.Size = new System.Drawing.Size(757, 22);
            this.StatusLine.TabIndex = 1;
            this.StatusLine.Text = "statusStrip1";
            // 
            // slMap1
            // 
            this.slMap1.AutoSize = false;
            this.slMap1.Name = "slMap1";
            this.slMap1.Size = new System.Drawing.Size(109, 17);
            this.slMap1.Text = "    ";
            // 
            // slMap2
            // 
            this.slMap2.AutoSize = false;
            this.slMap2.Name = "slMap2";
            this.slMap2.Size = new System.Drawing.Size(109, 17);
            this.slMap2.Text = "    ";
            // 
            // slMap3
            // 
            this.slMap3.AutoSize = false;
            this.slMap3.Name = "slMap3";
            this.slMap3.Size = new System.Drawing.Size(109, 17);
            this.slMap3.Text = "    ";
            // 
            // OpenMapDialog
            // 
            this.OpenMapDialog.Filter = "Карты |*.sitx;*.sitz;*.map";
            this.OpenMapDialog.Title = "Открыть карту";
            // 
            // pntConvert
            // 
            this.pntConvert.Location = new System.Drawing.Point(75, 168);
            this.pntConvert.Name = "pntConvert";
            this.pntConvert.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("pntConvert.OcxState")));
            this.pntConvert.Size = new System.Drawing.Size(32, 32);
            this.pntConvert.TabIndex = 13;
            // 
            // MapPoint
            // 
            this.MapPoint.Location = new System.Drawing.Point(75, 115);
            this.MapPoint.Name = "MapPoint";
            this.MapPoint.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("MapPoint.OcxState")));
            this.MapPoint.Size = new System.Drawing.Size(32, 32);
            this.MapPoint.TabIndex = 12;
            // 
            // MobilObj
            // 
            this.MobilObj.Location = new System.Drawing.Point(122, 115);
            this.MobilObj.Name = "MobilObj";
            this.MobilObj.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("MobilObj.OcxState")));
            this.MobilObj.Size = new System.Drawing.Size(32, 32);
            this.MobilObj.TabIndex = 11;
            // 
            // mvRsc
            // 
            this.mvRsc.Location = new System.Drawing.Point(27, 115);
            this.mvRsc.Name = "mvRsc";
            this.mvRsc.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("mvRsc.OcxState")));
            this.mvRsc.Size = new System.Drawing.Size(32, 32);
            this.mvRsc.TabIndex = 10;
            // 
            // axMapScreen
            // 
            this.axMapScreen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axMapScreen.Location = new System.Drawing.Point(0, 0);
            this.axMapScreen.Name = "axMapScreen";
            this.axMapScreen.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMapScreen.OcxState")));
            this.axMapScreen.Size = new System.Drawing.Size(530, 447);
            this.axMapScreen.TabIndex = 9;
            this.axMapScreen.OnMapMouseMove += new AxaxGisToolKit.IaxMapScreenEvents_OnMapMouseMoveEventHandler(this.axMapScreen_OnMapMouseMove);
            this.axMapScreen.OnMapScreenUpdate += new AxaxGisToolKit.IaxMapScreenEvents_OnMapScreenUpdateEventHandler(this.axMapScreen_OnMapScreenUpdate);
            // 
            // VectorObj
            // 
            this.VectorObj.Location = new System.Drawing.Point(122, 168);
            this.VectorObj.Name = "VectorObj";
            this.VectorObj.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("VectorObj.OcxState")));
            this.VectorObj.Size = new System.Drawing.Size(32, 32);
            this.VectorObj.TabIndex = 14;
            // 
            // RlsObj
            // 
            this.RlsObj.Location = new System.Drawing.Point(174, 115);
            this.RlsObj.Name = "RlsObj";
            this.RlsObj.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("RlsObj.OcxState")));
            this.RlsObj.Size = new System.Drawing.Size(32, 32);
            this.RlsObj.TabIndex = 15;
            // 
            // RlsZoneObj
            // 
            this.RlsZoneObj.Location = new System.Drawing.Point(174, 168);
            this.RlsZoneObj.Name = "RlsZoneObj";
            this.RlsZoneObj.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("RlsZoneObj.OcxState")));
            this.RlsZoneObj.Size = new System.Drawing.Size(32, 32);
            this.RlsZoneObj.TabIndex = 16;
            // 
            // MapFind
            // 
            this.MapFind.Location = new System.Drawing.Point(235, 115);
            this.MapFind.Name = "MapFind";
            this.MapFind.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("MapFind.OcxState")));
            this.MapFind.Size = new System.Drawing.Size(32, 32);
            this.MapFind.TabIndex = 17;
            // 
            // SearchableObj
            // 
            this.SearchableObj.Location = new System.Drawing.Point(122, 221);
            this.SearchableObj.Name = "SearchableObj";
            this.SearchableObj.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("SearchableObj.OcxState")));
            this.SearchableObj.Size = new System.Drawing.Size(32, 32);
            this.SearchableObj.TabIndex = 18;
            // 
            // TextObj
            // 
            this.TextObj.Location = new System.Drawing.Point(174, 221);
            this.TextObj.Name = "TextObj";
            this.TextObj.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("TextObj.OcxState")));
            this.TextObj.Size = new System.Drawing.Size(32, 32);
            this.TextObj.TabIndex = 19;
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 49);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.axMapScreen);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.dataGridView);
            this.splitContainer.Size = new System.Drawing.Size(757, 447);
            this.splitContainer.SplitterDistance = 530;
            this.splitContainer.TabIndex = 20;
            // 
            // dataGridView
            // 
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TargetNumber,
            this.HcNumber,
            this.Range,
            this.Azimuth,
            this.Height,
            this.Velocity,
            this.Source,
            this.Corr});
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.Location = new System.Drawing.Point(0, 0);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.RowHeadersVisible = false;
            this.dataGridView.Size = new System.Drawing.Size(223, 447);
            this.dataGridView.TabIndex = 0;
            this.dataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_CellContentClick);
            this.dataGridView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView_CellFormatting);
            this.dataGridView.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dataGridView_RowsAdded);
            // 
            // TargetNumber
            // 
            this.TargetNumber.HeaderText = "№  target";
            this.TargetNumber.Name = "TargetNumber";
            // 
            // HcNumber
            // 
            this.HcNumber.HeaderText = "№ HC";
            this.HcNumber.Name = "HcNumber";
            // 
            // Range
            // 
            this.Range.HeaderText = "R";
            this.Range.Name = "Range";
            // 
            // Azimuth
            // 
            this.Azimuth.HeaderText = "A";
            this.Azimuth.Name = "Azimuth";
            // 
            // Height
            // 
            this.Height.HeaderText = "H";
            this.Height.Name = "Height";
            // 
            // Velocity
            // 
            this.Velocity.HeaderText = "V";
            this.Velocity.Name = "Velocity";
            // 
            // Source
            // 
            this.Source.HeaderText = "Source";
            this.Source.Name = "Source";
            // 
            // Corr
            // 
            this.Corr.HeaderText = "Corr";
            this.Corr.Name = "Corr";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.mapToolStripMenuItem,
            this.serviceToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(757, 24);
            this.menuStrip1.TabIndex = 21;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // mapToolStripMenuItem
            // 
            this.mapToolStripMenuItem.Name = "mapToolStripMenuItem";
            this.mapToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.mapToolStripMenuItem.Text = "Map";
            // 
            // serviceToolStripMenuItem
            // 
            this.serviceToolStripMenuItem.Name = "serviceToolStripMenuItem";
            this.serviceToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
            this.serviceToolStripMenuItem.Text = "Service";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(757, 518);
            this.Controls.Add(this.TextObj);
            this.Controls.Add(this.SearchableObj);
            this.Controls.Add(this.MapFind);
            this.Controls.Add(this.RlsZoneObj);
            this.Controls.Add(this.RlsObj);
            this.Controls.Add(this.VectorObj);
            this.Controls.Add(this.pntConvert);
            this.Controls.Add(this.MapPoint);
            this.Controls.Add(this.MobilObj);
            this.Controls.Add(this.mvRsc);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.StatusLine);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmMain";
            this.Text = "Демо для тренажера";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMain_FormClosed);
            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.onMouseWheel);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.StatusLine.ResumeLayout(false);
            this.StatusLine.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pntConvert)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MapPoint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MobilObj)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mvRsc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axMapScreen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VectorObj)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RlsObj)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RlsZoneObj)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MapFind)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SearchableObj)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TextObj)).EndInit();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.StatusStrip StatusLine;
        private System.Windows.Forms.ToolStripButton btnFileOpen;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripStatusLabel slMap1;
        private System.Windows.Forms.ToolStripStatusLabel slMap2;
        private System.Windows.Forms.ToolStripStatusLabel slMap3;
        private System.Windows.Forms.OpenFileDialog OpenMapDialog;
        private AxaxGisToolKit.AxaxMapPoint pntConvert;
        private AxaxGisToolKit.AxaxMapPoint MapPoint;
        private AxaxGisToolKit.AxaxMapObj MobilObj;
        private AxaxGisToolKit.AxaxMapRsc mvRsc;
        private AxaxGisToolKit.AxaxcMapScreen axMapScreen;
        private AxaxGisToolKit.AxaxMapObj VectorObj;
        private AxaxGisToolKit.AxaxMapObj RlsObj;
        private AxaxGisToolKit.AxaxMapObj RlsZoneObj;
        private AxaxGisToolKit.AxaxMapFind MapFind;
        private AxaxGisToolKit.AxaxMapObj SearchableObj;
        private AxaxGisToolKit.AxaxMapObj TextObj;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mapToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem serviceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn TargetNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn HcNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn Range;
        private System.Windows.Forms.DataGridViewTextBoxColumn Azimuth;
        private System.Windows.Forms.DataGridViewTextBoxColumn Height;
        private System.Windows.Forms.DataGridViewTextBoxColumn Velocity;
        private System.Windows.Forms.DataGridViewTextBoxColumn Source;
        private System.Windows.Forms.DataGridViewTextBoxColumn Corr;
    }
}

