﻿namespace esriUtil.Forms.Texture
{
    partial class frmLandscapeMetrics
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLandscapeMetrics));
            this.btnExecute = new System.Windows.Forms.Button();
            this.cmbFocalStat = new System.Windows.Forms.ComboBox();
            this.txtOutName = new System.Windows.Forms.TextBox();
            this.lblRows = new System.Windows.Forms.Label();
            this.cmbWindowType = new System.Windows.Forms.ComboBox();
            this.nudColumns = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.nudRows = new System.Windows.Forms.NumericUpDown();
            this.lblColumns = new System.Windows.Forms.Label();
            this.btnOpenRaster = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbRaster = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbMetric = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudColumns)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRows)).BeginInit();
            this.SuspendLayout();
            // 
            // btnExecute
            // 
            this.btnExecute.Location = new System.Drawing.Point(188, 172);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(75, 23);
            this.btnExecute.TabIndex = 50;
            this.btnExecute.Text = "Execute";
            this.btnExecute.UseVisualStyleBackColor = true;
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // cmbFocalStat
            // 
            this.cmbFocalStat.FormattingEnabled = true;
            this.cmbFocalStat.Location = new System.Drawing.Point(141, 79);
            this.cmbFocalStat.Name = "cmbFocalStat";
            this.cmbFocalStat.Size = new System.Drawing.Size(121, 21);
            this.cmbFocalStat.TabIndex = 43;
            // 
            // txtOutName
            // 
            this.txtOutName.Location = new System.Drawing.Point(12, 174);
            this.txtOutName.Name = "txtOutName";
            this.txtOutName.Size = new System.Drawing.Size(148, 20);
            this.txtOutName.TabIndex = 49;
            // 
            // lblRows
            // 
            this.lblRows.AutoSize = true;
            this.lblRows.Location = new System.Drawing.Point(198, 106);
            this.lblRows.Name = "lblRows";
            this.lblRows.Size = new System.Drawing.Size(34, 13);
            this.lblRows.TabIndex = 42;
            this.lblRows.Text = "Rows";
            // 
            // cmbWindowType
            // 
            this.cmbWindowType.FormattingEnabled = true;
            this.cmbWindowType.Location = new System.Drawing.Point(12, 123);
            this.cmbWindowType.Name = "cmbWindowType";
            this.cmbWindowType.Size = new System.Drawing.Size(121, 21);
            this.cmbWindowType.TabIndex = 37;
            this.cmbWindowType.SelectedIndexChanged += new System.EventHandler(this.cmbWindowType_SelectedIndexChanged);
            // 
            // nudColumns
            // 
            this.nudColumns.Location = new System.Drawing.Point(141, 124);
            this.nudColumns.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nudColumns.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudColumns.Name = "nudColumns";
            this.nudColumns.Size = new System.Drawing.Size(46, 20);
            this.nudColumns.TabIndex = 39;
            this.nudColumns.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 156);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 48;
            this.label2.Text = "Output Name";
            // 
            // nudRows
            // 
            this.nudRows.Location = new System.Drawing.Point(201, 124);
            this.nudRows.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nudRows.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudRows.Name = "nudRows";
            this.nudRows.Size = new System.Drawing.Size(46, 20);
            this.nudRows.TabIndex = 40;
            this.nudRows.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // lblColumns
            // 
            this.lblColumns.AutoSize = true;
            this.lblColumns.Location = new System.Drawing.Point(138, 106);
            this.lblColumns.Name = "lblColumns";
            this.lblColumns.Size = new System.Drawing.Size(47, 13);
            this.lblColumns.TabIndex = 41;
            this.lblColumns.Text = "Columns";
            // 
            // btnOpenRaster
            // 
            this.btnOpenRaster.Image = global::esriUtil.Properties.Resources.cmdOpenProject;
            this.btnOpenRaster.Location = new System.Drawing.Point(238, 26);
            this.btnOpenRaster.Name = "btnOpenRaster";
            this.btnOpenRaster.Size = new System.Drawing.Size(25, 27);
            this.btnOpenRaster.TabIndex = 47;
            this.btnOpenRaster.UseVisualStyleBackColor = true;
            this.btnOpenRaster.Click += new System.EventHandler(this.btnOpenRaster_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(138, 60);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 13);
            this.label4.TabIndex = 44;
            this.label4.Text = "Patch Statistic";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 46;
            this.label1.Text = "Input Raster";
            // 
            // cmbRaster
            // 
            this.cmbRaster.FormattingEnabled = true;
            this.cmbRaster.Location = new System.Drawing.Point(12, 29);
            this.cmbRaster.Name = "cmbRaster";
            this.cmbRaster.Size = new System.Drawing.Size(220, 21);
            this.cmbRaster.TabIndex = 45;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 105);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 13);
            this.label3.TabIndex = 38;
            this.label3.Text = "Window Type";
            // 
            // cmbMetric
            // 
            this.cmbMetric.FormattingEnabled = true;
            this.cmbMetric.Location = new System.Drawing.Point(12, 79);
            this.cmbMetric.Name = "cmbMetric";
            this.cmbMetric.Size = new System.Drawing.Size(121, 21);
            this.cmbMetric.TabIndex = 51;
            this.cmbMetric.SelectedIndexChanged += new System.EventHandler(this.cmbMetric_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 60);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(92, 13);
            this.label5.TabIndex = 52;
            this.label5.Text = "Landscape Metric";
            // 
            // frmLandscapeMetrics
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(275, 212);
            this.Controls.Add(this.cmbMetric);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnExecute);
            this.Controls.Add(this.cmbFocalStat);
            this.Controls.Add(this.txtOutName);
            this.Controls.Add(this.lblRows);
            this.Controls.Add(this.cmbWindowType);
            this.Controls.Add(this.nudColumns);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.nudRows);
            this.Controls.Add(this.lblColumns);
            this.Controls.Add(this.btnOpenRaster);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbRaster);
            this.Controls.Add(this.label3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmLandscapeMetrics";
            this.Text = "Landscape Metrics";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.nudColumns)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRows)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.ComboBox cmbFocalStat;
        private System.Windows.Forms.TextBox txtOutName;
        private System.Windows.Forms.Label lblRows;
        private System.Windows.Forms.ComboBox cmbWindowType;
        private System.Windows.Forms.NumericUpDown nudColumns;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudRows;
        private System.Windows.Forms.Label lblColumns;
        private System.Windows.Forms.Button btnOpenRaster;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbRaster;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbMetric;
        private System.Windows.Forms.Label label5;
    }
}