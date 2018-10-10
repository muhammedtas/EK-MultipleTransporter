namespace EK_MultipleTransporter.Forms
{
    partial class ProjectsForm
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.txtFolderRoot = new System.Windows.Forms.TextBox();
            this.lblCategoryDocumentType = new System.Windows.Forms.Label();
            this.lblChild = new System.Windows.Forms.Label();
            this.lblFolderRoot = new System.Windows.Forms.Label();
            this.ofdRootFolder = new System.Windows.Forms.OpenFileDialog();
            this.lblCategoryYear = new System.Windows.Forms.Label();
            this.lblTerm = new System.Windows.Forms.Label();
            this.fbdRootFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.dtpYear = new System.Windows.Forms.DateTimePicker();
            this.cmbChildRoot = new System.Windows.Forms.ComboBox();
            this.cmbDocumentType = new System.Windows.Forms.ComboBox();
            this.cmbTerm = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(322, 290);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "İptal";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(480, 290);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "Tamam";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // txtFolderRoot
            // 
            this.txtFolderRoot.Location = new System.Drawing.Point(176, 46);
            this.txtFolderRoot.Name = "txtFolderRoot";
            this.txtFolderRoot.Size = new System.Drawing.Size(379, 20);
            this.txtFolderRoot.TabIndex = 6;
            this.txtFolderRoot.Click += new System.EventHandler(this.txtFolderRoot_Click);
            // 
            // lblCategoryDocumentType
            // 
            this.lblCategoryDocumentType.AutoSize = true;
            this.lblCategoryDocumentType.Location = new System.Drawing.Point(63, 159);
            this.lblCategoryDocumentType.Name = "lblCategoryDocumentType";
            this.lblCategoryDocumentType.Size = new System.Drawing.Size(78, 13);
            this.lblCategoryDocumentType.TabIndex = 8;
            this.lblCategoryDocumentType.Text = "Doküman Türü";
            // 
            // lblChild
            // 
            this.lblChild.AutoSize = true;
            this.lblChild.Location = new System.Drawing.Point(63, 105);
            this.lblChild.Name = "lblChild";
            this.lblChild.Size = new System.Drawing.Size(78, 13);
            this.lblChild.TabIndex = 9;
            this.lblChild.Text = "Alt Birim Seçimi";
            // 
            // lblFolderRoot
            // 
            this.lblFolderRoot.AutoSize = true;
            this.lblFolderRoot.Location = new System.Drawing.Point(63, 46);
            this.lblFolderRoot.Name = "lblFolderRoot";
            this.lblFolderRoot.Size = new System.Drawing.Size(70, 13);
            this.lblFolderRoot.TabIndex = 10;
            this.lblFolderRoot.Text = "Klasör Seçimi";
            // 
            // ofdRootFolder
            // 
            this.ofdRootFolder.FileName = "openFileDialog1";
            // 
            // lblCategoryYear
            // 
            this.lblCategoryYear.AutoSize = true;
            this.lblCategoryYear.Location = new System.Drawing.Point(63, 197);
            this.lblCategoryYear.Name = "lblCategoryYear";
            this.lblCategoryYear.Size = new System.Drawing.Size(18, 13);
            this.lblCategoryYear.TabIndex = 13;
            this.lblCategoryYear.Text = "Yıl";
            // 
            // lblTerm
            // 
            this.lblTerm.AutoSize = true;
            this.lblTerm.Location = new System.Drawing.Point(63, 236);
            this.lblTerm.Name = "lblTerm";
            this.lblTerm.Size = new System.Drawing.Size(40, 13);
            this.lblTerm.TabIndex = 14;
            this.lblTerm.Text = "Çeyrek";
            // 
            // cmbChildRoot
            // 
            this.cmbChildRoot.FormattingEnabled = true;
            this.cmbChildRoot.Location = new System.Drawing.Point(176, 97);
            this.cmbChildRoot.Name = "cmbChildRoot";
            this.cmbChildRoot.Size = new System.Drawing.Size(379, 21);
            this.cmbChildRoot.TabIndex = 5;
            // 
            // cmbDocumentType
            // 
            this.cmbDocumentType.Items.AddRange(new object[] {
            "Yapı Ruhsatı",
            "Tapu",
            "Katalog",
            "Sözleşme"});
            this.cmbDocumentType.Location = new System.Drawing.Point(176, 151);
            this.cmbDocumentType.Name = "cmbDocumentType";
            this.cmbDocumentType.Size = new System.Drawing.Size(379, 21);
            this.cmbDocumentType.TabIndex = 15;
            // 
            // cmbTerm
            // 
            this.cmbTerm.FormattingEnabled = true;
            this.cmbTerm.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4"});
            this.cmbTerm.Location = new System.Drawing.Point(176, 228);
            this.cmbTerm.Name = "cmbTerm";
            this.cmbTerm.Size = new System.Drawing.Size(379, 21);
            this.cmbTerm.TabIndex = 11;
            // 
            // dtpYear
            // 
            this.dtpYear.Location = new System.Drawing.Point(176, 190);
            this.dtpYear.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpYear.CustomFormat = "yyyy";
            this.dtpYear.Name = "dtpYear";
            this.dtpYear.Size = new System.Drawing.Size(379, 20);
            this.dtpYear.TabIndex = 0;
            // 
            // ProjectsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(596, 371);
            this.Controls.Add(this.lblTerm);
            this.Controls.Add(this.lblCategoryYear);
            this.Controls.Add(this.cmbTerm);
            this.Controls.Add(this.dtpYear);
            this.Controls.Add(this.lblFolderRoot);
            this.Controls.Add(this.lblChild);
            this.Controls.Add(this.lblCategoryDocumentType);
            this.Controls.Add(this.txtFolderRoot);
            this.Controls.Add(this.cmbChildRoot);
            this.Controls.Add(this.cmbDocumentType);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Name = "ProjectsForm";
            this.Text = "ProjectsForm";
            this.Load += new System.EventHandler(this.ProjectsForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox cmbChildRoot;
        private System.Windows.Forms.ComboBox cmbDocumentType;
        private System.Windows.Forms.ComboBox cmbTerm;
        private System.Windows.Forms.DateTimePicker dtpYear;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.TextBox txtFolderRoot;
        private System.Windows.Forms.Label lblCategoryDocumentType;
        private System.Windows.Forms.Label lblChild;
        private System.Windows.Forms.Label lblFolderRoot;
        private System.Windows.Forms.OpenFileDialog ofdRootFolder;
        private System.Windows.Forms.Label lblCategoryYear;
        private System.Windows.Forms.Label lblTerm;
        private System.Windows.Forms.FolderBrowserDialog fbdRootFolder;
    }
}