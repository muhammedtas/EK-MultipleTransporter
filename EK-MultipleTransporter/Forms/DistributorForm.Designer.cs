﻿using System.Windows.Forms;

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
            cmbDistriborTerm = new System.Windows.Forms.ComboBox();
            cmbDistWorkPlaceType = new System.Windows.Forms.ComboBox();
            cmbDistOTFolder = new System.Windows.Forms.ComboBox();
            cmbDocumentType = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // lblTerm
            // 
            this.lblTerm.AutoSize = true;
            this.lblTerm.Location = new System.Drawing.Point(48, 296);
            this.lblTerm.Name = "lblTerm";
            this.lblTerm.Size = new System.Drawing.Size(40, 13);
            this.lblTerm.TabIndex = 36;
            this.lblTerm.Text = "Çeyrek";
            // 
            // lblCategoryYear
            // 
            this.lblCategoryYear.AutoSize = true;
            this.lblCategoryYear.Location = new System.Drawing.Point(48, 245);
            this.lblCategoryYear.Name = "lblCategoryYear";
            this.lblCategoryYear.Size = new System.Drawing.Size(18, 13);
            this.lblCategoryYear.TabIndex = 29;
            this.lblCategoryYear.Text = "Yıl";
            // 
            // dtpDistributorYear
            // 
            this.dtpDistributorYear.CalendarMonthBackground = System.Drawing.SystemColors.Menu;
            this.dtpDistributorYear.CustomFormat = "yyyy";
            this.dtpDistributorYear.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDistributorYear.Location = new System.Drawing.Point(162, 238);
            this.dtpDistributorYear.Name = "dtpDistributorYear";
            this.dtpDistributorYear.ShowUpDown = true;
            this.dtpDistributorYear.Size = new System.Drawing.Size(379, 20);
            this.dtpDistributorYear.TabIndex = 28;
            // 
            // lblDistriboturDocumentRoot
            // 
            this.lblDistriboturDocumentRoot.AutoSize = true;
            this.lblDistriboturDocumentRoot.Location = new System.Drawing.Point(48, 148);
            this.lblDistriboturDocumentRoot.Name = "lblDistriboturDocumentRoot";
            this.lblDistriboturDocumentRoot.Size = new System.Drawing.Size(87, 13);
            this.lblDistriboturDocumentRoot.TabIndex = 32;
            this.lblDistriboturDocumentRoot.Text = "Doküman Seçimi";
            // 
            // lblWorkPlaceType
            // 
            this.lblWorkPlaceType.AutoSize = true;
            this.lblWorkPlaceType.Location = new System.Drawing.Point(48, 49);
            this.lblWorkPlaceType.Name = "lblWorkPlaceType";
            this.lblWorkPlaceType.Size = new System.Drawing.Size(61, 13);
            this.lblWorkPlaceType.TabIndex = 33;
            this.lblWorkPlaceType.Text = "İş alanı türü";
            // 
            // lblCategoryDocumentType
            // 
            this.lblCategoryDocumentType.AutoSize = true;
            this.lblCategoryDocumentType.Location = new System.Drawing.Point(48, 103);
            this.lblCategoryDocumentType.Name = "lblCategoryDocumentType";
            this.lblCategoryDocumentType.Size = new System.Drawing.Size(70, 13);
            this.lblCategoryDocumentType.TabIndex = 34;
            this.lblCategoryDocumentType.Text = "Klasör Seçimi";
            // 
            // txtDistDocumentRoot
            // 
            this.txtDistDocumentRoot.BackColor = System.Drawing.SystemColors.Menu;
            this.txtDistDocumentRoot.Location = new System.Drawing.Point(162, 148);
            this.txtDistDocumentRoot.Name = "txtDistDocumentRoot";
            this.txtDistDocumentRoot.Size = new System.Drawing.Size(379, 20);
            this.txtDistDocumentRoot.TabIndex = 25;
            this.txtDistDocumentRoot.Click += new System.EventHandler(this.txtDistDocumentRoot_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(466, 670);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 30;
            this.btnOk.Text = "Yükle";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(308, 670);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 31;
            this.btnCancel.Text = "İptal";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblDocType
            // 
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
            this.cLstBxWorkSpaceType.BackColor = System.Drawing.SystemColors.Menu;
            this.cLstBxWorkSpaceType.CheckBoxes = true;
            this.cLstBxWorkSpaceType.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.hdWorkSpaceType});
            this.cLstBxWorkSpaceType.Location = new System.Drawing.Point(51, 348);
            this.cLstBxWorkSpaceType.Name = "cLstBxWorkSpaceType";
            this.cLstBxWorkSpaceType.Size = new System.Drawing.Size(490, 305);
            this.cLstBxWorkSpaceType.TabIndex = 43;
            this.cLstBxWorkSpaceType.UseCompatibleStateImageBehavior = false;
            this.cLstBxWorkSpaceType.View = System.Windows.Forms.View.Details;
            this.cLstBxWorkSpaceType.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            this.cLstBxWorkSpaceType.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            this.cLstBxWorkSpaceType.MultiSelect = true;
            // 
            // hdWorkSpaceType
            // 
            this.hdWorkSpaceType.Name = "hdWorkSpaceType";
            this.hdWorkSpaceType.Text = "İş Alanı Türü";
            this.hdWorkSpaceType.Width = 375;
            // 
            // cmbDistriborTerm
            // 
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
            cmbDistOTFolder.BackColor = System.Drawing.SystemColors.Menu;
            cmbDistOTFolder.Location = new System.Drawing.Point(162, 95);
            cmbDistOTFolder.Name = "cmbDistOTFolder";
            cmbDistOTFolder.Size = new System.Drawing.Size(379, 21);
            cmbDistOTFolder.TabIndex = 27;
            cmbDistOTFolder.SelectedIndexChanged += new System.EventHandler(this.cmbDistOTFolder_SelectedIndexChanged);
            // 
            // cmbDocumentType
            // 
            cmbDocumentType.BackColor = System.Drawing.SystemColors.Menu;
            cmbDocumentType.Location = new System.Drawing.Point(162, 196);
            cmbDocumentType.Name = "cmbDocumentType";
            cmbDocumentType.Size = new System.Drawing.Size(379, 21);
            cmbDocumentType.TabIndex = 39;
            // 
            // DistributorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(599, 725);
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
            this.Text = "DistributorForm";
            this.Load += new System.EventHandler(this.DistributorForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        System.Windows.Forms.ComboBox cmbDistriborTerm;
        System.Windows.Forms.ComboBox cmbDistWorkPlaceType;
        System.Windows.Forms.ComboBox cmbDistOTFolder;
        System.Windows.Forms.ComboBox cmbDocumentType;
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
        private OpenFileDialog ofdDocument;
        public ListView cLstBxWorkSpaceType;
        private ColumnHeader hdWorkSpaceType;
    }
}