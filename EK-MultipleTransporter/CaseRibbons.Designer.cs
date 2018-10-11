﻿namespace EK_MultipleTransporter
{
    partial class CaseRibbons : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public CaseRibbons()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CaseRibbons));
            this.tabMain = this.Factory.CreateRibbonTab();
            this.grMain = this.Factory.CreateRibbonGroup();
            this.rbtnProjectList = this.Factory.CreateRibbonButton();
            this.rbtnPlotList = this.Factory.CreateRibbonButton();
            this.rbtnDistrictLst = this.Factory.CreateRibbonButton();
            this.rbtnLitigationList = this.Factory.CreateRibbonButton();
            this.rbtnPersonelList = this.Factory.CreateRibbonButton();
            this.tabMain.SuspendLayout();
            this.grMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabMain
            // 
            this.tabMain.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.tabMain.Groups.Add(this.grMain);
            this.tabMain.Label = "Emlak Konut";
            this.tabMain.Name = "tabMain";
            // 
            // grMain
            // 
            this.grMain.Items.Add(this.rbtnProjectList);
            this.grMain.Items.Add(this.rbtnPlotList);
            this.grMain.Items.Add(this.rbtnDistrictLst);
            this.grMain.Items.Add(this.rbtnLitigationList);
            this.grMain.Items.Add(this.rbtnPersonelList);
            this.grMain.Label = "OpenText Yükleyicileri";
            this.grMain.Name = "grMain";
            // 
            // rbtnProjectList
            // 
            this.rbtnProjectList.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.rbtnProjectList.Image = ((System.Drawing.Image)(resources.GetObject("rbtnProjectList.Image")));
            this.rbtnProjectList.Label = "Projeleri Aktar";
            this.rbtnProjectList.Name = "rbtnProjectList";
            this.rbtnProjectList.ShowImage = true;
            this.rbtnProjectList.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.rbtnProjectList_Click);
            // 
            // rbtnPlotList
            // 
            this.rbtnPlotList.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.rbtnPlotList.Image = ((System.Drawing.Image)(resources.GetObject("rbtnPlotList.Image")));
            this.rbtnPlotList.Label = "Arsaları Aktar";
            this.rbtnPlotList.Name = "rbtnPlotList";
            this.rbtnPlotList.ShowImage = true;
            this.rbtnPlotList.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.rbtnPlotList_Click);
            // 
            // rbtnDistrictLst
            // 
            this.rbtnDistrictLst.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.rbtnDistrictLst.Image = ((System.Drawing.Image)(resources.GetObject("rbtnDistrictLst.Image")));
            this.rbtnDistrictLst.Label = "Bağımsız Bölümleri Aktar";
            this.rbtnDistrictLst.Name = "rbtnDistrictLst";
            this.rbtnDistrictLst.ShowImage = true;
            this.rbtnDistrictLst.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.rbtnDistrictLst_Click);
            // 
            // rbtnLitigationList
            // 
            this.rbtnLitigationList.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.rbtnLitigationList.Image = ((System.Drawing.Image)(resources.GetObject("rbtnLitigationList.Image")));
            this.rbtnLitigationList.Label = "Davaları Aktar";
            this.rbtnLitigationList.Name = "rbtnLitigationList";
            this.rbtnLitigationList.ShowImage = true;
            this.rbtnLitigationList.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.rbtnLitigationList_Click);
            // 
            // rbtnPersonelList
            // 
            this.rbtnPersonelList.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.rbtnPersonelList.Image = ((System.Drawing.Image)(resources.GetObject("rbtnPersonelList.Image")));
            this.rbtnPersonelList.Label = "Personelleri Aktar";
            this.rbtnPersonelList.Name = "rbtnPersonelList";
            this.rbtnPersonelList.ShowImage = true;
            this.rbtnPersonelList.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.rbtnPersonelList_Click);
            // 
            // CaseRibbons
            // 
            this.Name = "CaseRibbons";
            this.RibbonType = "Microsoft.Excel.Workbook";
            this.Tabs.Add(this.tabMain);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.CaseRibbons_Load);
            this.tabMain.ResumeLayout(false);
            this.tabMain.PerformLayout();
            this.grMain.ResumeLayout(false);
            this.grMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public Microsoft.Office.Tools.Ribbon.RibbonTab tabMain;
        public Microsoft.Office.Tools.Ribbon.RibbonGroup grMain;
        public Microsoft.Office.Tools.Ribbon.RibbonButton rbtnPersonelList;
        public Microsoft.Office.Tools.Ribbon.RibbonButton rbtnLitigationList;
        public Microsoft.Office.Tools.Ribbon.RibbonButton rbtnDistrictLst;
        public Microsoft.Office.Tools.Ribbon.RibbonButton rbtnPlotList;
        public Microsoft.Office.Tools.Ribbon.RibbonButton rbtnProjectList;
        //internal Microsoft.Office.Tools.Ribbon.RibbonButton rbtnProjectsList;
        //internal Microsoft.Office.Tools.Ribbon.RibbonButton rbtnPlotList;
        //internal Microsoft.Office.Tools.Ribbon.RibbonButton rbtnDistrictList;
        //internal Microsoft.Office.Tools.Ribbon.RibbonButton rbtnLitigationList;
        //internal Microsoft.Office.Tools.Ribbon.RibbonButton rbtnPersonList;
    }

    partial class ThisRibbonCollection
    {
        internal CaseRibbons CaseRibbons
        {
            get { return this.GetRibbon<CaseRibbons>(); }
        }
    }
}
