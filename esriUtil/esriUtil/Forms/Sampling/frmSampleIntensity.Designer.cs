namespace esriUtil.Forms.Sampling
{
    partial class frmSampleIntensity
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSampleIntensity));
            this.label3 = new System.Windows.Forms.Label();
            this.btnSimulate = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbOut = new System.Windows.Forms.ComboBox();
            this.cmbSampleFeatureClass = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbDesign = new System.Windows.Forms.ComboBox();
            this.btnOpenFeatureClass = new System.Windows.Forms.Button();
            this.btnOutFile = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.chbContinuous = new System.Windows.Forms.CheckBox();
            this.nudPlots = new System.Windows.Forms.NumericUpDown();
            this.nudArea = new System.Windows.Forms.NumericUpDown();
            this.nudIter = new System.Windows.Forms.NumericUpDown();
            this.chbPoints = new System.Windows.Forms.CheckBox();
            this.Field = new System.Windows.Forms.Label();
            this.cmbField = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.nudPlots)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudArea)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudIter)).BeginInit();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 145);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 18;
            this.label3.Text = "Plots / Acre";
            // 
            // btnSimulate
            // 
            this.btnSimulate.Location = new System.Drawing.Point(195, 242);
            this.btnSimulate.Name = "btnSimulate";
            this.btnSimulate.Size = new System.Drawing.Size(63, 21);
            this.btnSimulate.TabIndex = 15;
            this.btnSimulate.Text = "Run";
            this.btnSimulate.UseVisualStyleBackColor = true;
            this.btnSimulate.Click += new System.EventHandler(this.btnSample_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 193);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Out Results";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Layer (Raster or Polygon)";
            // 
            // cmbOut
            // 
            this.cmbOut.FormattingEnabled = true;
            this.cmbOut.Location = new System.Drawing.Point(11, 211);
            this.cmbOut.Name = "cmbOut";
            this.cmbOut.Size = new System.Drawing.Size(211, 21);
            this.cmbOut.TabIndex = 12;
            // 
            // cmbSampleFeatureClass
            // 
            this.cmbSampleFeatureClass.FormattingEnabled = true;
            this.cmbSampleFeatureClass.Location = new System.Drawing.Point(10, 25);
            this.cmbSampleFeatureClass.Name = "cmbSampleFeatureClass";
            this.cmbSampleFeatureClass.Size = new System.Drawing.Size(210, 21);
            this.cmbSampleFeatureClass.TabIndex = 11;
            this.cmbSampleFeatureClass.SelectedIndexChanged += new System.EventHandler(this.cmbSampleFeatureClass_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(137, 52);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 13);
            this.label4.TabIndex = 20;
            this.label4.Text = "Design";
            // 
            // cmbDesign
            // 
            this.cmbDesign.FormattingEnabled = true;
            this.cmbDesign.Items.AddRange(new object[] {
            ""});
            this.cmbDesign.Location = new System.Drawing.Point(139, 70);
            this.cmbDesign.Name = "cmbDesign";
            this.cmbDesign.Size = new System.Drawing.Size(81, 21);
            this.cmbDesign.TabIndex = 19;
            this.cmbDesign.Text = " ";
            // 
            // btnOpenFeatureClass
            // 
            this.btnOpenFeatureClass.Image = global::esriUtil.Properties.Resources.cmdOpenProject;
            this.btnOpenFeatureClass.Location = new System.Drawing.Point(231, 21);
            this.btnOpenFeatureClass.Name = "btnOpenFeatureClass";
            this.btnOpenFeatureClass.Size = new System.Drawing.Size(27, 28);
            this.btnOpenFeatureClass.TabIndex = 16;
            this.btnOpenFeatureClass.UseVisualStyleBackColor = true;
            this.btnOpenFeatureClass.Click += new System.EventHandler(this.btnOpenFeatureClass_Click);
            // 
            // btnOutFile
            // 
            this.btnOutFile.Image = global::esriUtil.Properties.Resources.cmdOpenProject;
            this.btnOutFile.Location = new System.Drawing.Point(232, 209);
            this.btnOutFile.Name = "btnOutFile";
            this.btnOutFile.Size = new System.Drawing.Size(27, 27);
            this.btnOutFile.TabIndex = 10;
            this.btnOutFile.UseVisualStyleBackColor = true;
            this.btnOutFile.Click += new System.EventHandler(this.btnOutFile_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 94);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(50, 13);
            this.label5.TabIndex = 22;
            this.label5.Text = "Iterations";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(125, 144);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(86, 13);
            this.label6.TabIndex = 24;
            this.label6.Text = "Plot Area (Acres)";
            // 
            // chbContinuous
            // 
            this.chbContinuous.AutoSize = true;
            this.chbContinuous.Location = new System.Drawing.Point(12, 245);
            this.chbContinuous.Name = "chbContinuous";
            this.chbContinuous.Size = new System.Drawing.Size(79, 17);
            this.chbContinuous.TabIndex = 25;
            this.chbContinuous.Text = "Continuous";
            this.chbContinuous.UseVisualStyleBackColor = true;
            // 
            // nudPlots
            // 
            this.nudPlots.DecimalPlaces = 5;
            this.nudPlots.Location = new System.Drawing.Point(10, 163);
            this.nudPlots.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            458752});
            this.nudPlots.Name = "nudPlots";
            this.nudPlots.Size = new System.Drawing.Size(93, 20);
            this.nudPlots.TabIndex = 26;
            this.nudPlots.Value = new decimal(new int[] {
            166667,
            0,
            0,
            589824});
            // 
            // nudArea
            // 
            this.nudArea.DecimalPlaces = 5;
            this.nudArea.Location = new System.Drawing.Point(127, 163);
            this.nudArea.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            327680});
            this.nudArea.Name = "nudArea";
            this.nudArea.Size = new System.Drawing.Size(93, 20);
            this.nudArea.TabIndex = 27;
            this.nudArea.Value = new decimal(new int[] {
            166667,
            0,
            0,
            393216});
            // 
            // nudIter
            // 
            this.nudIter.Location = new System.Drawing.Point(10, 112);
            this.nudIter.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nudIter.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudIter.Name = "nudIter";
            this.nudIter.Size = new System.Drawing.Size(120, 20);
            this.nudIter.TabIndex = 28;
            this.nudIter.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // chbPoints
            // 
            this.chbPoints.AutoSize = true;
            this.chbPoints.Location = new System.Drawing.Point(97, 245);
            this.chbPoints.Name = "chbPoints";
            this.chbPoints.Size = new System.Drawing.Size(83, 17);
            this.chbPoints.TabIndex = 29;
            this.chbPoints.Text = "Save Points";
            this.chbPoints.UseVisualStyleBackColor = true;
            // 
            // Field
            // 
            this.Field.AutoSize = true;
            this.Field.Location = new System.Drawing.Point(10, 52);
            this.Field.Name = "Field";
            this.Field.Size = new System.Drawing.Size(29, 13);
            this.Field.TabIndex = 31;
            this.Field.Text = "Field";
            // 
            // cmbField
            // 
            this.cmbField.FormattingEnabled = true;
            this.cmbField.Items.AddRange(new object[] {
            ""});
            this.cmbField.Location = new System.Drawing.Point(12, 70);
            this.cmbField.Name = "cmbField";
            this.cmbField.Size = new System.Drawing.Size(118, 21);
            this.cmbField.TabIndex = 30;
            this.cmbField.Text = " ";
            // 
            // frmSampleIntensity
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(273, 278);
            this.Controls.Add(this.Field);
            this.Controls.Add(this.cmbField);
            this.Controls.Add(this.chbPoints);
            this.Controls.Add(this.nudIter);
            this.Controls.Add(this.nudArea);
            this.Controls.Add(this.nudPlots);
            this.Controls.Add(this.chbContinuous);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmbDesign);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnOpenFeatureClass);
            this.Controls.Add(this.btnSimulate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbOut);
            this.Controls.Add(this.cmbSampleFeatureClass);
            this.Controls.Add(this.btnOutFile);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmSampleIntensity";
            this.Text = "Sample Simulation";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.nudPlots)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudArea)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudIter)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnOpenFeatureClass;
        private System.Windows.Forms.Button btnSimulate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbOut;
        private System.Windows.Forms.ComboBox cmbSampleFeatureClass;
        private System.Windows.Forms.Button btnOutFile;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbDesign;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox chbContinuous;
        private System.Windows.Forms.NumericUpDown nudPlots;
        private System.Windows.Forms.NumericUpDown nudArea;
        private System.Windows.Forms.NumericUpDown nudIter;
        private System.Windows.Forms.CheckBox chbPoints;
        private System.Windows.Forms.Label Field;
        private System.Windows.Forms.ComboBox cmbField;
    }
}