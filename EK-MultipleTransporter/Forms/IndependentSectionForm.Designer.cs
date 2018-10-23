namespace EK_MultipleTransporter.Forms
{
    partial class IndependentSectionForm
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
            this.cmbDistrictsList = new System.Windows.Forms.ComboBox();
            this.lblDistricts = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cmbDistrictsList
            // 
            this.cmbDistrictsList.BackColor = System.Drawing.SystemColors.Menu;
            this.cmbDistrictsList.FormattingEnabled = true;
            this.cmbDistrictsList.Location = new System.Drawing.Point(221, 47);
            this.cmbDistrictsList.Name = "cmbDistrictsList";
            this.cmbDistrictsList.Size = new System.Drawing.Size(273, 21);
            this.cmbDistrictsList.TabIndex = 0;
            // 
            // lblDistricts
            // 
            this.lblDistricts.AutoSize = true;
            this.lblDistricts.Location = new System.Drawing.Point(54, 54);
            this.lblDistricts.Name = "lblDistricts";
            this.lblDistricts.Size = new System.Drawing.Size(117, 13);
            this.lblDistricts.TabIndex = 1;
            this.lblDistricts.Text = "Bağımsız Bölüm Seçiniz";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(221, 98);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "İptal";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(419, 98);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "Yükle";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // IndependentSectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(555, 159);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblDistricts);
            this.Controls.Add(this.cmbDistrictsList);
            this.Name = "Bağımsız Bölüm Dökümanlarınızı Yükleyin";
            this.Text = "DistrictsForm";
            this.Load += new System.EventHandler(this.DistrictsForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbDistrictsList;
        private System.Windows.Forms.Label lblDistricts;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
    }
}