namespace EK_MultipleTransporter.Forms
{
    partial class StaffForm
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
            this.dtpStaffYear = new System.Windows.Forms.DateTimePicker();
            this.lblFolderRoot = new System.Windows.Forms.Label();
            this.lblChild = new System.Windows.Forms.Label();
            this.lblCategoryDocumentType = new System.Windows.Forms.Label();
            this.txtStaffFolderRoot = new System.Windows.Forms.TextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.fbdStaffsFolder = new System.Windows.Forms.FolderBrowserDialog();
            cmbStaffTerm = new System.Windows.Forms.ComboBox();
            cmbStaffChildRoot = new System.Windows.Forms.ComboBox();
            cmbStaffDocumentType = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // lblTerm
            // 
            this.lblTerm.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTerm.AutoSize = true;
            this.lblTerm.Location = new System.Drawing.Point(60, 259);
            this.lblTerm.Name = "lblTerm";
            this.lblTerm.Size = new System.Drawing.Size(40, 13);
            this.lblTerm.TabIndex = 24;
            this.lblTerm.Text = "Çeyrek";
            // 
            // lblCategoryYear
            // 
            this.lblCategoryYear.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCategoryYear.AutoSize = true;
            this.lblCategoryYear.Location = new System.Drawing.Point(60, 208);
            this.lblCategoryYear.Name = "lblCategoryYear";
            this.lblCategoryYear.Size = new System.Drawing.Size(18, 13);
            this.lblCategoryYear.TabIndex = 17;
            this.lblCategoryYear.Text = "Yıl";
            // 
            // dtpStaffYear
            // 
            this.dtpStaffYear.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpStaffYear.CalendarMonthBackground = System.Drawing.SystemColors.Menu;
            this.dtpStaffYear.CustomFormat = "yyyy";
            this.dtpStaffYear.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpStaffYear.Location = new System.Drawing.Point(174, 201);
            this.dtpStaffYear.Name = "dtpStaffYear";
            this.dtpStaffYear.ShowUpDown = true;
            this.dtpStaffYear.Size = new System.Drawing.Size(379, 20);
            this.dtpStaffYear.TabIndex = 16;
            // 
            // lblFolderRoot
            // 
            this.lblFolderRoot.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFolderRoot.AutoSize = true;
            this.lblFolderRoot.Location = new System.Drawing.Point(60, 46);
            this.lblFolderRoot.Name = "lblFolderRoot";
            this.lblFolderRoot.Size = new System.Drawing.Size(70, 13);
            this.lblFolderRoot.TabIndex = 20;
            this.lblFolderRoot.Text = "Klasör Seçimi";
            // 
            // lblChild
            // 
            this.lblChild.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblChild.AutoSize = true;
            this.lblChild.Location = new System.Drawing.Point(60, 105);
            this.lblChild.Name = "lblChild";
            this.lblChild.Size = new System.Drawing.Size(78, 13);
            this.lblChild.TabIndex = 21;
            this.lblChild.Text = "Alt Birim Seçimi";
            // 
            // lblCategoryDocumentType
            // 
            this.lblCategoryDocumentType.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCategoryDocumentType.AutoSize = true;
            this.lblCategoryDocumentType.Location = new System.Drawing.Point(60, 159);
            this.lblCategoryDocumentType.Name = "lblCategoryDocumentType";
            this.lblCategoryDocumentType.Size = new System.Drawing.Size(78, 13);
            this.lblCategoryDocumentType.TabIndex = 22;
            this.lblCategoryDocumentType.Text = "Doküman Türü";
            // 
            // txtStaffFolderRoot
            // 
            this.txtStaffFolderRoot.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtStaffFolderRoot.BackColor = System.Drawing.SystemColors.Menu;
            this.txtStaffFolderRoot.Location = new System.Drawing.Point(174, 46);
            this.txtStaffFolderRoot.Name = "txtStaffFolderRoot";
            this.txtStaffFolderRoot.Size = new System.Drawing.Size(379, 20);
            this.txtStaffFolderRoot.TabIndex = 13;
            this.txtStaffFolderRoot.Click += new System.EventHandler(this.txtStaffFolderRoot_Click);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(478, 311);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 18;
            this.btnOk.Text = "Yükle";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.Location = new System.Drawing.Point(320, 311);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 19;
            this.btnCancel.Text = "İptal";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // cmbStaffTerm
            // 
            cmbStaffTerm.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            cmbStaffTerm.BackColor = System.Drawing.SystemColors.Menu;
            cmbStaffTerm.FormattingEnabled = true;
            cmbStaffTerm.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4"});
            cmbStaffTerm.Location = new System.Drawing.Point(174, 251);
            cmbStaffTerm.Name = "cmbStaffTerm";
            cmbStaffTerm.Size = new System.Drawing.Size(379, 21);
            cmbStaffTerm.TabIndex = 23;
            // 
            // cmbStaffChildRoot
            // 
            cmbStaffChildRoot.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            cmbStaffChildRoot.BackColor = System.Drawing.SystemColors.Menu;
            cmbStaffChildRoot.FormattingEnabled = true;
            cmbStaffChildRoot.Location = new System.Drawing.Point(174, 97);
            cmbStaffChildRoot.Name = "cmbStaffChildRoot";
            cmbStaffChildRoot.Size = new System.Drawing.Size(379, 21);
            cmbStaffChildRoot.TabIndex = 14;
            // 
            // cmbStaffDocumentType
            // 
            cmbStaffDocumentType.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            cmbStaffDocumentType.BackColor = System.Drawing.SystemColors.Menu;
            cmbStaffDocumentType.Location = new System.Drawing.Point(174, 151);
            cmbStaffDocumentType.Name = "cmbStaffDocumentType";
            cmbStaffDocumentType.Size = new System.Drawing.Size(379, 21);
            cmbStaffDocumentType.TabIndex = 15;
            // 
            // StaffForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.SeaGreen;
            this.ClientSize = new System.Drawing.Size(596, 377);
            this.Controls.Add(this.lblTerm);
            this.Controls.Add(this.lblCategoryYear);
            this.Controls.Add(cmbStaffTerm);
            this.Controls.Add(this.dtpStaffYear);
            this.Controls.Add(this.lblFolderRoot);
            this.Controls.Add(this.lblChild);
            this.Controls.Add(this.lblCategoryDocumentType);
            this.Controls.Add(this.txtStaffFolderRoot);
            this.Controls.Add(cmbStaffChildRoot);
            this.Controls.Add(cmbStaffDocumentType);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Name = "StaffForm";
            this.Text = "Personel dökümanlarını Yükleyin.";
            this.Load += new System.EventHandler(this.StaffForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox cmbStaffTerm;
        private System.Windows.Forms.ComboBox cmbStaffChildRoot;
        private System.Windows.Forms.ComboBox cmbStaffDocumentType;
        private System.Windows.Forms.Label lblTerm;
        private System.Windows.Forms.Label lblCategoryYear;
        private System.Windows.Forms.DateTimePicker dtpStaffYear;
        private System.Windows.Forms.Label lblFolderRoot;
        private System.Windows.Forms.Label lblChild;
        private System.Windows.Forms.Label lblCategoryDocumentType;
        private System.Windows.Forms.TextBox txtStaffFolderRoot;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.FolderBrowserDialog fbdStaffsFolder;
    }
}