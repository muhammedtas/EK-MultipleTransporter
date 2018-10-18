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
            this.lblWorkPlace = new System.Windows.Forms.Label();
            this.lblDocType = new System.Windows.Forms.Label();
            this.cmbDistriborTerm = new System.Windows.Forms.ComboBox();
            this.cmbDistWorkPlaceType = new System.Windows.Forms.ComboBox();
            this.cmbDistOTFolder = new System.Windows.Forms.ComboBox();
            this.cmbWorkSpaceType = new System.Windows.Forms.ComboBox();
            this.cmbDocumentType = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
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
            // 
            // cmbDistOTFolder
            // 
            cmbDistOTFolder.BackColor = System.Drawing.SystemColors.Menu;
            cmbDistOTFolder.Location = new System.Drawing.Point(162, 95);
            cmbDistOTFolder.Name = "cmbDistOTFolder";
            cmbDistOTFolder.Size = new System.Drawing.Size(379, 21);
            cmbDistOTFolder.TabIndex = 27;
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
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(466, 387);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 30;
            this.btnOk.Text = "Yükle";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(308, 387);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 31;
            this.btnCancel.Text = "İptal";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // lblWorkPlace
            // 
            this.lblWorkPlace.AutoSize = true;
            this.lblWorkPlace.Location = new System.Drawing.Point(48, 348);
            this.lblWorkPlace.Name = "lblWorkPlace";
            this.lblWorkPlace.Size = new System.Drawing.Size(61, 13);
            this.lblWorkPlace.TabIndex = 38;
            this.lblWorkPlace.Text = "İş alanı türü";
            // 
            // cmbWorkSpaceType
            // 
            cmbWorkSpaceType.BackColor = System.Drawing.SystemColors.Menu;
            cmbWorkSpaceType.FormattingEnabled = true;
            cmbWorkSpaceType.Location = new System.Drawing.Point(162, 340);
            cmbWorkSpaceType.Name = "cmbWorkSpaceType";
            cmbWorkSpaceType.Size = new System.Drawing.Size(379, 21);
            cmbWorkSpaceType.TabIndex = 37;
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
            this.ClientSize = new System.Drawing.Size(614, 462);
            this.Controls.Add(this.lblDocType);
            this.Controls.Add(cmbDocumentType);
            this.Controls.Add(this.lblWorkPlace);
            this.Controls.Add(cmbWorkSpaceType);
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

        private System.Windows.Forms.ComboBox cmbDistriborTerm;
        private System.Windows.Forms.ComboBox cmbDistWorkPlaceType;
        private System.Windows.Forms.ComboBox cmbDistOTFolder;
        private System.Windows.Forms.ComboBox cmbWorkSpaceType;
        private System.Windows.Forms.ComboBox cmbDocumentType;
        private System.Windows.Forms.Label lblTerm;
        private System.Windows.Forms.Label lblCategoryYear;
        private System.Windows.Forms.DateTimePicker dtpDistributorYear;
        private System.Windows.Forms.Label lblDistriboturDocumentRoot;
        private System.Windows.Forms.Label lblWorkPlaceType;
        private System.Windows.Forms.Label lblCategoryDocumentType;
        private System.Windows.Forms.TextBox txtDistDocumentRoot;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblWorkPlace;
        private System.Windows.Forms.Label lblDocType;
    }
}