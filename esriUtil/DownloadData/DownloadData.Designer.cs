namespace DownloadData
{
    partial class frmDownLoad
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDownLoad));
            this.txtExtent = new System.Windows.Forms.TextBox();
            this.btnPolyExtent = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnWorkspace = new System.Windows.Forms.Button();
            this.txtWorkspace = new System.Windows.Forms.TextBox();
            this.txtDownloadSite = new System.Windows.Forms.TextBox();
            this.btnDownload = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.btnIndex = new System.Windows.Forms.Button();
            this.txtIndex = new System.Windows.Forms.TextBox();
            this.cmbFld = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtFileExtension = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtExtent
            // 
            this.txtExtent.Location = new System.Drawing.Point(13, 136);
            this.txtExtent.Name = "txtExtent";
            this.txtExtent.Size = new System.Drawing.Size(237, 20);
            this.txtExtent.TabIndex = 1;
            // 
            // btnPolyExtent
            // 
            this.btnPolyExtent.Image = global::DownloadData.Properties.Resources.cmdOpenProject;
            this.btnPolyExtent.Location = new System.Drawing.Point(256, 133);
            this.btnPolyExtent.Name = "btnPolyExtent";
            this.btnPolyExtent.Size = new System.Drawing.Size(27, 24);
            this.btnPolyExtent.TabIndex = 2;
            this.btnPolyExtent.UseVisualStyleBackColor = true;
            this.btnPolyExtent.Click += new System.EventHandler(this.btnPolyExtent_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 116);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Extent (Feature Class)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Download Site";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 172);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Output Directory";
            // 
            // btnWorkspace
            // 
            this.btnWorkspace.Image = global::DownloadData.Properties.Resources.cmdOpenProject;
            this.btnWorkspace.Location = new System.Drawing.Point(349, 189);
            this.btnWorkspace.Name = "btnWorkspace";
            this.btnWorkspace.Size = new System.Drawing.Size(27, 24);
            this.btnWorkspace.TabIndex = 6;
            this.btnWorkspace.UseVisualStyleBackColor = true;
            this.btnWorkspace.Click += new System.EventHandler(this.btnWorkspace_Click);
            // 
            // txtWorkspace
            // 
            this.txtWorkspace.Location = new System.Drawing.Point(12, 192);
            this.txtWorkspace.Name = "txtWorkspace";
            this.txtWorkspace.Size = new System.Drawing.Size(331, 20);
            this.txtWorkspace.TabIndex = 5;
            // 
            // txtDownloadSite
            // 
            this.txtDownloadSite.Location = new System.Drawing.Point(13, 31);
            this.txtDownloadSite.Name = "txtDownloadSite";
            this.txtDownloadSite.Size = new System.Drawing.Size(445, 20);
            this.txtDownloadSite.TabIndex = 8;
            this.txtDownloadSite.Text = "ftp://rockyftp.cr.usgs.gov/vdelivery/Datasets/Staged/NAIP/fl_2013/";
            // 
            // btnDownload
            // 
            this.btnDownload.Location = new System.Drawing.Point(392, 190);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(66, 23);
            this.btnDownload.TabIndex = 9;
            this.btnDownload.Text = "Download";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 63);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(157, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Download Index (Feature Class)";
            // 
            // btnIndex
            // 
            this.btnIndex.Image = global::DownloadData.Properties.Resources.cmdOpenProject;
            this.btnIndex.Location = new System.Drawing.Point(256, 80);
            this.btnIndex.Name = "btnIndex";
            this.btnIndex.Size = new System.Drawing.Size(27, 24);
            this.btnIndex.TabIndex = 11;
            this.btnIndex.UseVisualStyleBackColor = true;
            this.btnIndex.Click += new System.EventHandler(this.btnIndex_Click);
            // 
            // txtIndex
            // 
            this.txtIndex.Location = new System.Drawing.Point(13, 83);
            this.txtIndex.Name = "txtIndex";
            this.txtIndex.Size = new System.Drawing.Size(237, 20);
            this.txtIndex.TabIndex = 10;
            this.txtIndex.TextChanged += new System.EventHandler(this.txtIndex_TextChanged);
            // 
            // cmbFld
            // 
            this.cmbFld.FormattingEnabled = true;
            this.cmbFld.Location = new System.Drawing.Point(306, 83);
            this.cmbFld.Name = "cmbFld";
            this.cmbFld.Size = new System.Drawing.Size(152, 21);
            this.cmbFld.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(305, 63);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(110, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "File Names From Field";
            // 
            // txtFileExtension
            // 
            this.txtFileExtension.Location = new System.Drawing.Point(306, 133);
            this.txtFileExtension.Name = "txtFileExtension";
            this.txtFileExtension.Size = new System.Drawing.Size(152, 20);
            this.txtFileExtension.TabIndex = 15;
            this.txtFileExtension.Text = ".jp2";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(315, 116);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(119, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "File extension (.jp2, .zip)";
            // 
            // frmDownLoad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(486, 224);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtFileExtension);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cmbFld);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnIndex);
            this.Controls.Add(this.txtIndex);
            this.Controls.Add(this.btnDownload);
            this.Controls.Add(this.txtDownloadSite);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnWorkspace);
            this.Controls.Add(this.txtWorkspace);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnPolyExtent);
            this.Controls.Add(this.txtExtent);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmDownLoad";
            this.Text = "Download Data";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtExtent;
        private System.Windows.Forms.Button btnPolyExtent;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnWorkspace;
        private System.Windows.Forms.TextBox txtWorkspace;
        private System.Windows.Forms.TextBox txtDownloadSite;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnIndex;
        private System.Windows.Forms.TextBox txtIndex;
        private System.Windows.Forms.ComboBox cmbFld;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtFileExtension;
        private System.Windows.Forms.Label label6;
    }
}

