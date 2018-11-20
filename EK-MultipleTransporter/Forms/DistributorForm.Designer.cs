using System.Windows.Forms;

namespace EK_MultipleTransporter.Forms
{
    partial class DistributorForm
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
                components.Dispose();

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblTerm = new System.Windows.Forms.Label();
            this.lblCategoryYear = new System.Windows.Forms.Label();
            this.dtpDistributorYear = new System.Windows.Forms.DateTimePicker();
            this.lblDistriboturDocumentRoot = new System.Windows.Forms.Label();
            this.lblWorkPlaceType = new System.Windows.Forms.Label();
            this.lblCategoryDocumentType = new System.Windows.Forms.Label();
            this.txtDistDocumentRoot = new System.Windows.Forms.TextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblDocType = new System.Windows.Forms.Label();
            this.ofdDocument = new System.Windows.Forms.OpenFileDialog();
            this.cLstBxWorkSpaceType = new System.Windows.Forms.ListView();
            this.hdWorkSpaceType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.txtFilter = new System.Windows.Forms.TextBox();
            this.cScrollofLst = new System.Windows.Forms.VScrollBar();
            this.cbCheckAll = new System.Windows.Forms.CheckBox();
            this.lblCounter = new System.Windows.Forms.Label();
            this.lblProjectsOfDistricts = new System.Windows.Forms.Label();
            cmbDistriborTerm = new System.Windows.Forms.ComboBox();
            cmbDistWorkPlaceType = new System.Windows.Forms.ComboBox();
            cmbDistOTFolder = new System.Windows.Forms.ComboBox();
            cmbDocumentType = new System.Windows.Forms.ComboBox();
            cmbProjectsOfDistricts = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // lblTerm
            // 
            this.lblTerm.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTerm.AutoSize = true;
            this.lblTerm.Location = new System.Drawing.Point(48, 296);
            this.lblTerm.Name = "lblTerm";
            this.lblTerm.Size = new System.Drawing.Size(40, 13);
            this.lblTerm.TabIndex = 36;
            this.lblTerm.Text = "Çeyrek";
            // 
            // lblCategoryYear
            // 
            this.lblCategoryYear.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCategoryYear.AutoSize = true;
            this.lblCategoryYear.Location = new System.Drawing.Point(48, 248);
            this.lblCategoryYear.Name = "lblCategoryYear";
            this.lblCategoryYear.Size = new System.Drawing.Size(18, 13);
            this.lblCategoryYear.TabIndex = 29;
            this.lblCategoryYear.Text = "Yıl";
            // 
            // dtpDistributorYear
            // 
            this.dtpDistributorYear.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpDistributorYear.CalendarMonthBackground = System.Drawing.SystemColors.Menu;
            this.dtpDistributorYear.CustomFormat = "yyyy";
            this.dtpDistributorYear.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDistributorYear.Location = new System.Drawing.Point(162, 241);
            this.dtpDistributorYear.Name = "dtpDistributorYear";
            this.dtpDistributorYear.ShowUpDown = true;
            this.dtpDistributorYear.Size = new System.Drawing.Size(379, 20);
            this.dtpDistributorYear.TabIndex = 28;
            // 
            // lblDistriboturDocumentRoot
            // 
            this.lblDistriboturDocumentRoot.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDistriboturDocumentRoot.AutoSize = true;
            this.lblDistriboturDocumentRoot.Location = new System.Drawing.Point(48, 148);
            this.lblDistriboturDocumentRoot.Name = "lblDistriboturDocumentRoot";
            this.lblDistriboturDocumentRoot.Size = new System.Drawing.Size(87, 13);
            this.lblDistriboturDocumentRoot.TabIndex = 32;
            this.lblDistriboturDocumentRoot.Text = "Doküman Seçimi";
            // 
            // lblWorkPlaceType
            // 
            this.lblWorkPlaceType.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblWorkPlaceType.AutoSize = true;
            this.lblWorkPlaceType.Location = new System.Drawing.Point(48, 49);
            this.lblWorkPlaceType.Name = "lblWorkPlaceType";
            this.lblWorkPlaceType.Size = new System.Drawing.Size(61, 13);
            this.lblWorkPlaceType.TabIndex = 33;
            this.lblWorkPlaceType.Text = "İş alanı türü";
            // 
            // lblCategoryDocumentType
            // 
            this.lblCategoryDocumentType.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCategoryDocumentType.AutoSize = true;
            this.lblCategoryDocumentType.Location = new System.Drawing.Point(48, 103);
            this.lblCategoryDocumentType.Name = "lblCategoryDocumentType";
            this.lblCategoryDocumentType.Size = new System.Drawing.Size(70, 13);
            this.lblCategoryDocumentType.TabIndex = 34;
            this.lblCategoryDocumentType.Text = "Klasör Seçimi";
            // 
            // txtDistDocumentRoot
            // 
            this.txtDistDocumentRoot.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDistDocumentRoot.BackColor = System.Drawing.SystemColors.Menu;
            this.txtDistDocumentRoot.Location = new System.Drawing.Point(162, 148);
            this.txtDistDocumentRoot.Name = "txtDistDocumentRoot";
            this.txtDistDocumentRoot.Size = new System.Drawing.Size(379, 20);
            this.txtDistDocumentRoot.TabIndex = 25;
            this.txtDistDocumentRoot.Click += new System.EventHandler(this.txtDistDocumentRoot_Click);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(468, 714);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 30;
            this.btnOk.Text = "Yükle";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(310, 714);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 31;
            this.btnCancel.Text = "İptal";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblDocType
            // 
            this.lblDocType.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDocType.AutoSize = true;
            this.lblDocType.Location = new System.Drawing.Point(48, 204);
            this.lblDocType.Name = "lblDocType";
            this.lblDocType.Size = new System.Drawing.Size(78, 13);
            this.lblDocType.TabIndex = 40;
            this.lblDocType.Text = "Doküman Türü";
            // 
            // ofdDocument
            // 
            this.ofdDocument.FileName = "ofdDocument";
            // 
            // cLstBxWorkSpaceType
            // 
            this.cLstBxWorkSpaceType.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cLstBxWorkSpaceType.BackColor = System.Drawing.SystemColors.Menu;
            this.cLstBxWorkSpaceType.CheckBoxes = true;
            this.cLstBxWorkSpaceType.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.hdWorkSpaceType});
            this.cLstBxWorkSpaceType.Location = new System.Drawing.Point(51, 401);
            this.cLstBxWorkSpaceType.Name = "cLstBxWorkSpaceType";
            this.cLstBxWorkSpaceType.Size = new System.Drawing.Size(490, 277);
            this.cLstBxWorkSpaceType.TabIndex = 43;
            this.cLstBxWorkSpaceType.UseCompatibleStateImageBehavior = false;
            this.cLstBxWorkSpaceType.View = System.Windows.Forms.View.Details;
            this.cLstBxWorkSpaceType.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.cLstBxWorkSpaceType_ItemCheck);
            // 
            // hdWorkSpaceType
            // 
            this.hdWorkSpaceType.Name = "hdWorkSpaceType";
            this.hdWorkSpaceType.Text = "     İş Alanı Türü";
            this.hdWorkSpaceType.Width = 486;
            // 
            // txtFilter
            // 
            this.txtFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFilter.BackColor = System.Drawing.SystemColors.Menu;
            this.txtFilter.Location = new System.Drawing.Point(51, 375);
            this.txtFilter.Name = "txtFilter";
            this.txtFilter.Size = new System.Drawing.Size(490, 20);
            this.txtFilter.TabIndex = 44;
            this.txtFilter.Text = "İş alanlarını filtreleyin...";
            this.txtFilter.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtFilter_MouseClick);
            this.txtFilter.TextChanged += new System.EventHandler(this.txtFilter_TextChanged);
            // 
            // cScrollofLst
            // 
            this.cScrollofLst.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cScrollofLst.Location = new System.Drawing.Point(521, 405);
            this.cScrollofLst.Name = "cScrollofLst";
            this.cScrollofLst.Size = new System.Drawing.Size(17, 270);
            this.cScrollofLst.TabIndex = 45;
            this.cScrollofLst.Scroll += new System.Windows.Forms.ScrollEventHandler(this.cScrollofLst_Scroll);
            this.cScrollofLst.ValueChanged += new System.EventHandler(this.cScrollofLst_ValueChanged);
            // 
            // cbCheckAll
            // 
            this.cbCheckAll.AutoSize = true;
            this.cbCheckAll.Location = new System.Drawing.Point(57, 408);
            this.cbCheckAll.Name = "cbCheckAll";
            this.cbCheckAll.Size = new System.Drawing.Size(15, 14);
            this.cbCheckAll.TabIndex = 46;
            this.cbCheckAll.UseVisualStyleBackColor = true;
            this.cbCheckAll.Click += new System.EventHandler(this.cbCheckAll_Click);
            // 
            // lblCounter
            // 
            this.lblCounter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblCounter.AutoSize = true;
            this.lblCounter.Location = new System.Drawing.Point(48, 716);
            this.lblCounter.Name = "lblCounter";
            this.lblCounter.Size = new System.Drawing.Size(0, 13);
            this.lblCounter.TabIndex = 47;
            // 
            // lblProjectsOfDistricts
            // 
            this.lblProjectsOfDistricts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblProjectsOfDistricts.AutoSize = true;
            this.lblProjectsOfDistricts.Enabled = false;
            this.lblProjectsOfDistricts.Location = new System.Drawing.Point(48, 340);
            this.lblProjectsOfDistricts.Name = "lblProjectsOfDistricts";
            this.lblProjectsOfDistricts.Size = new System.Drawing.Size(38, 13);
            this.lblProjectsOfDistricts.TabIndex = 49;
            this.lblProjectsOfDistricts.Text = "Projesi";
            this.lblProjectsOfDistricts.Visible = false;
            // 
            // cmbDistriborTerm
            // 
            cmbDistriborTerm.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            cmbDistriborTerm.BackColor = System.Drawing.SystemColors.Menu;
            cmbDistriborTerm.FormattingEnabled = true;
            cmbDistriborTerm.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4"});
            cmbDistriborTerm.Location = new System.Drawing.Point(162, 288);
            cmbDistriborTerm.Name = "cmbDistriborTerm";
            cmbDistriborTerm.Size = new System.Drawing.Size(379, 21);
            cmbDistriborTerm.TabIndex = 35;
            // 
            // cmbDistWorkPlaceType
            // 
            cmbDistWorkPlaceType.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            cmbDistWorkPlaceType.BackColor = System.Drawing.SystemColors.Menu;
            cmbDistWorkPlaceType.FormattingEnabled = true;
            cmbDistWorkPlaceType.Location = new System.Drawing.Point(162, 41);
            cmbDistWorkPlaceType.Name = "cmbDistWorkPlaceType";
            cmbDistWorkPlaceType.Size = new System.Drawing.Size(379, 21);
            cmbDistWorkPlaceType.TabIndex = 26;
            cmbDistWorkPlaceType.SelectedIndexChanged += new System.EventHandler(this.cmbDistWorkPlaceType_SelectedIndexChanged);
            // 
            // cmbDistOTFolder
            // 
            cmbDistOTFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            cmbDistOTFolder.BackColor = System.Drawing.SystemColors.Menu;
            cmbDistOTFolder.Location = new System.Drawing.Point(162, 95);
            cmbDistOTFolder.Name = "cmbDistOTFolder";
            cmbDistOTFolder.Size = new System.Drawing.Size(379, 21);
            cmbDistOTFolder.TabIndex = 27;
            // 
            // cmbDocumentType
            // 
            cmbDocumentType.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            cmbDocumentType.BackColor = System.Drawing.SystemColors.Menu;
            cmbDocumentType.Location = new System.Drawing.Point(162, 196);
            cmbDocumentType.Name = "cmbDocumentType";
            cmbDocumentType.Size = new System.Drawing.Size(379, 21);
            cmbDocumentType.TabIndex = 39;
            // 
            // cmbProjectsOfDistricts
            // 
            cmbProjectsOfDistricts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            cmbProjectsOfDistricts.BackColor = System.Drawing.SystemColors.Menu;
            cmbProjectsOfDistricts.Enabled = false;
            cmbProjectsOfDistricts.FormattingEnabled = true;
            cmbProjectsOfDistricts.Location = new System.Drawing.Point(162, 332);
            cmbProjectsOfDistricts.Name = "cmbProjectsOfDistricts";
            cmbProjectsOfDistricts.Size = new System.Drawing.Size(379, 21);
            cmbProjectsOfDistricts.TabIndex = 48;
            cmbProjectsOfDistricts.Visible = false;
            cmbProjectsOfDistricts.SelectedIndexChanged += new System.EventHandler(this.cmbProjectsOfDistricts_SelectedIndexChanged);
            // 
            // DistributorForm
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(599, 749);
            this.Controls.Add(this.lblProjectsOfDistricts);
            this.Controls.Add(cmbProjectsOfDistricts);
            this.Controls.Add(this.lblCounter);
            this.Controls.Add(this.cbCheckAll);
            this.Controls.Add(this.cScrollofLst);
            this.Controls.Add(this.txtFilter);
            this.Controls.Add(this.cLstBxWorkSpaceType);
            this.Controls.Add(this.lblDocType);
            this.Controls.Add(cmbDocumentType);
            this.Controls.Add(this.lblTerm);
            this.Controls.Add(this.lblCategoryYear);
            this.Controls.Add(cmbDistriborTerm);
            this.Controls.Add(this.dtpDistributorYear);
            this.Controls.Add(this.lblDistriboturDocumentRoot);
            this.Controls.Add(this.lblWorkPlaceType);
            this.Controls.Add(this.lblCategoryDocumentType);
            this.Controls.Add(this.txtDistDocumentRoot);
            this.Controls.Add(cmbDistWorkPlaceType);
            this.Controls.Add(cmbDistOTFolder);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Name = "DistributorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Dökümanınızı Dağıtın";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DistributorForm_FormClosing);
            this.Load += new System.EventHandler(this.DistributorForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        System.Windows.Forms.ComboBox cmbDistriborTerm;
        System.Windows.Forms.ComboBox cmbDistWorkPlaceType;
        System.Windows.Forms.ComboBox cmbDistOTFolder;
        System.Windows.Forms.ComboBox cmbDocumentType;
        System.Windows.Forms.ComboBox cmbProjectsOfDistricts;
        private System.Windows.Forms.Label lblTerm;
        private System.Windows.Forms.Label lblCategoryYear;
        private System.Windows.Forms.DateTimePicker dtpDistributorYear;
        private System.Windows.Forms.Label lblDistriboturDocumentRoot;
        private System.Windows.Forms.Label lblWorkPlaceType;
        private System.Windows.Forms.Label lblCategoryDocumentType;
        private System.Windows.Forms.TextBox txtDistDocumentRoot;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblDocType;
        private System.Windows.Forms.OpenFileDialog ofdDocument;
        private System.Windows.Forms.ListView cLstBxWorkSpaceType;
        private System.Windows.Forms.ColumnHeader hdWorkSpaceType; 
        //private System.Windows.Forms.ColumnHeader hdCheckBoxField;
        private System.Windows.Forms.TextBox txtFilter;
        private System.Windows.Forms.VScrollBar cScrollofLst;
        private System.Windows.Forms.CheckBox cbCheckAll;
        private System.Windows.Forms.Label lblCounter;
        private System.Windows.Forms.Label lblProjectsOfDistricts;
    }
}