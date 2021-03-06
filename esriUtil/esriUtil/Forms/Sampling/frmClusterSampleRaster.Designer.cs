namespace esriUtil.Forms.Sampling
{
    partial class frmClusterSampleRaster
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmClusterSampleRaster));
            this.btnSample = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbRaster = new System.Windows.Forms.ComboBox();
            this.cmbSampleFeatureClass = new System.Windows.Forms.ComboBox();
            this.btnOpenFeatureClass = new System.Windows.Forms.Button();
            this.btnOpenRaster = new System.Windows.Forms.Button();
            this.cmbCluster = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dgvAzDs = new System.Windows.Forms.DataGridView();
            this.clmAzimuth = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmDistance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnPlus = new System.Windows.Forms.Button();
            this.btnMinus = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAzDs)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSample
            // 
            this.btnSample.Location = new System.Drawing.Point(189, 330);
            this.btnSample.Name = "btnSample";
            this.btnSample.Size = new System.Drawing.Size(63, 22);
            this.btnSample.TabIndex = 13;
            this.btnSample.Text = "Sample";
            this.btnSample.UseVisualStyleBackColor = true;
            this.btnSample.Click += new System.EventHandler(this.btnSample_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Raster to Sample";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Sample Loctions";
            // 
            // cmbRaster
            // 
            this.cmbRaster.FormattingEnabled = true;
            this.cmbRaster.Location = new System.Drawing.Point(12, 76);
            this.cmbRaster.Name = "cmbRaster";
            this.cmbRaster.Size = new System.Drawing.Size(211, 21);
            this.cmbRaster.TabIndex = 10;
            // 
            // cmbSampleFeatureClass
            // 
            this.cmbSampleFeatureClass.FormattingEnabled = true;
            this.cmbSampleFeatureClass.Location = new System.Drawing.Point(12, 27);
            this.cmbSampleFeatureClass.Name = "cmbSampleFeatureClass";
            this.cmbSampleFeatureClass.Size = new System.Drawing.Size(210, 21);
            this.cmbSampleFeatureClass.TabIndex = 9;
            // 
            // btnOpenFeatureClass
            // 
            this.btnOpenFeatureClass.Image = global::esriUtil.Properties.Resources.cmdOpenProject;
            this.btnOpenFeatureClass.Location = new System.Drawing.Point(225, 23);
            this.btnOpenFeatureClass.Name = "btnOpenFeatureClass";
            this.btnOpenFeatureClass.Size = new System.Drawing.Size(27, 28);
            this.btnOpenFeatureClass.TabIndex = 14;
            this.btnOpenFeatureClass.UseVisualStyleBackColor = true;
            this.btnOpenFeatureClass.Click += new System.EventHandler(this.btnOpenFeatureClass_Click);
            // 
            // btnOpenRaster
            // 
            this.btnOpenRaster.Image = global::esriUtil.Properties.Resources.cmdOpenProject;
            this.btnOpenRaster.Location = new System.Drawing.Point(226, 74);
            this.btnOpenRaster.Name = "btnOpenRaster";
            this.btnOpenRaster.Size = new System.Drawing.Size(27, 27);
            this.btnOpenRaster.TabIndex = 8;
            this.btnOpenRaster.UseVisualStyleBackColor = true;
            this.btnOpenRaster.Click += new System.EventHandler(this.btnOpenRaster_Click);
            // 
            // cmbCluster
            // 
            this.cmbCluster.FormattingEnabled = true;
            this.cmbCluster.Location = new System.Drawing.Point(12, 127);
            this.cmbCluster.Name = "cmbCluster";
            this.cmbCluster.Size = new System.Drawing.Size(210, 21);
            this.cmbCluster.TabIndex = 15;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 108);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(137, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "Cluster Summarization Type";
            // 
            // dgvAzDs
            // 
            this.dgvAzDs.AllowUserToAddRows = false;
            this.dgvAzDs.AllowUserToDeleteRows = false;
            this.dgvAzDs.AllowUserToResizeColumns = false;
            this.dgvAzDs.AllowUserToResizeRows = false;
            this.dgvAzDs.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvAzDs.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvAzDs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAzDs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmAzimuth,
            this.clmDistance});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvAzDs.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgvAzDs.Location = new System.Drawing.Point(15, 174);
            this.dgvAzDs.Name = "dgvAzDs";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvAzDs.RowHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvAzDs.RowHeadersVisible = false;
            this.dgvAzDs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvAzDs.Size = new System.Drawing.Size(207, 150);
            this.dgvAzDs.TabIndex = 17;
            // 
            // clmAzimuth
            // 
            dataGridViewCellStyle2.Format = "N0";
            dataGridViewCellStyle2.NullValue = null;
            this.clmAzimuth.DefaultCellStyle = dataGridViewCellStyle2;
            this.clmAzimuth.HeaderText = "Azimuth";
            this.clmAzimuth.Name = "clmAzimuth";
            // 
            // clmDistance
            // 
            dataGridViewCellStyle3.Format = "N0";
            dataGridViewCellStyle3.NullValue = null;
            this.clmDistance.DefaultCellStyle = dataGridViewCellStyle3;
            this.clmDistance.HeaderText = "Distance";
            this.clmDistance.Name = "clmDistance";
            // 
            // btnPlus
            // 
            this.btnPlus.Location = new System.Drawing.Point(226, 174);
            this.btnPlus.Name = "btnPlus";
            this.btnPlus.Size = new System.Drawing.Size(27, 23);
            this.btnPlus.TabIndex = 18;
            this.btnPlus.Text = "+";
            this.btnPlus.UseVisualStyleBackColor = true;
            this.btnPlus.Click += new System.EventHandler(this.btnPlus_Click);
            // 
            // btnMinus
            // 
            this.btnMinus.Location = new System.Drawing.Point(226, 203);
            this.btnMinus.Name = "btnMinus";
            this.btnMinus.Size = new System.Drawing.Size(27, 23);
            this.btnMinus.TabIndex = 19;
            this.btnMinus.Text = "-";
            this.btnMinus.UseVisualStyleBackColor = true;
            this.btnMinus.Click += new System.EventHandler(this.btnMinus_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 155);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 13);
            this.label4.TabIndex = 20;
            this.label4.Text = "Offset from center";
            // 
            // frmClusterSampleRaster
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(262, 359);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnMinus);
            this.Controls.Add(this.btnPlus);
            this.Controls.Add(this.dgvAzDs);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbCluster);
            this.Controls.Add(this.btnOpenFeatureClass);
            this.Controls.Add(this.btnSample);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbRaster);
            this.Controls.Add(this.cmbSampleFeatureClass);
            this.Controls.Add(this.btnOpenRaster);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmClusterSampleRaster";
            this.Text = "Cluster Sample Raster";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.dgvAzDs)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOpenFeatureClass;
        private System.Windows.Forms.Button btnSample;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbRaster;
        private System.Windows.Forms.ComboBox cmbSampleFeatureClass;
        private System.Windows.Forms.Button btnOpenRaster;
        private System.Windows.Forms.ComboBox cmbCluster;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView dgvAzDs;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmAzimuth;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmDistance;
        private System.Windows.Forms.Button btnPlus;
        private System.Windows.Forms.Button btnMinus;
        private System.Windows.Forms.Label label4;
    }
}