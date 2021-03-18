namespace esriUtil.Forms.Sampling
{
    partial class frmBootstrap
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBootstrap));
            this.btnOpenFeatureClass = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbSampleFeatureClass = new System.Windows.Forms.ComboBox();
            this.Field = new System.Windows.Forms.Label();
            this.cmbField = new System.Windows.Forms.ComboBox();
            this.nudIter = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.btnSimulate = new System.Windows.Forms.Button();
            this.cmbOut = new System.Windows.Forms.ComboBox();
            this.btnOutFile = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbStrata = new System.Windows.Forms.ComboBox();
            this.nudAlpha = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudIter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAlpha)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOpenFeatureClass
            // 
            this.btnOpenFeatureClass.Image = global::esriUtil.Properties.Resources.cmdOpenProject;
            this.btnOpenFeatureClass.Location = new System.Drawing.Point(233, 18);
            this.btnOpenFeatureClass.Name = "btnOpenFeatureClass";
            this.btnOpenFeatureClass.Size = new System.Drawing.Size(27, 28);
            this.btnOpenFeatureClass.TabIndex = 34;
            this.btnOpenFeatureClass.UseVisualStyleBackColor = true;
            this.btnOpenFeatureClass.Click += new System.EventHandler(this.btnOpenFeatureClass_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(148, 13);
            this.label1.TabIndex = 33;
            this.label1.Text = "Layer (Table or Feature Class)";
            // 
            // cmbSampleFeatureClass
            // 
            this.cmbSampleFeatureClass.FormattingEnabled = true;
            this.cmbSampleFeatureClass.Location = new System.Drawing.Point(12, 22);
            this.cmbSampleFeatureClass.Name = "cmbSampleFeatureClass";
            this.cmbSampleFeatureClass.Size = new System.Drawing.Size(210, 21);
            this.cmbSampleFeatureClass.TabIndex = 32;
            this.cmbSampleFeatureClass.SelectedIndexChanged += new System.EventHandler(this.cmbSampleFeatureClass_SelectedIndexChanged);
            // 
            // Field
            // 
            this.Field.AutoSize = true;
            this.Field.Location = new System.Drawing.Point(12, 48);
            this.Field.Name = "Field";
            this.Field.Size = new System.Drawing.Size(59, 13);
            this.Field.TabIndex = 41;
            this.Field.Text = "Value Field";
            // 
            // cmbField
            // 
            this.cmbField.FormattingEnabled = true;
            this.cmbField.Items.AddRange(new object[] {
            ""});
            this.cmbField.Location = new System.Drawing.Point(14, 66);
            this.cmbField.Name = "cmbField";
            this.cmbField.Size = new System.Drawing.Size(118, 21);
            this.cmbField.TabIndex = 40;
            this.cmbField.Text = " ";
            // 
            // nudIter
            // 
            this.nudIter.Location = new System.Drawing.Point(162, 67);
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
            this.nudIter.Size = new System.Drawing.Size(60, 20);
            this.nudIter.TabIndex = 39;
            this.nudIter.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(159, 49);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(50, 13);
            this.label5.TabIndex = 38;
            this.label5.Text = "Iterations";
            // 
            // btnSimulate
            // 
            this.btnSimulate.Location = new System.Drawing.Point(196, 201);
            this.btnSimulate.Name = "btnSimulate";
            this.btnSimulate.Size = new System.Drawing.Size(63, 21);
            this.btnSimulate.TabIndex = 37;
            this.btnSimulate.Text = "Run";
            this.btnSimulate.UseVisualStyleBackColor = true;
            this.btnSimulate.Click += new System.EventHandler(this.btnSample_Click);
            // 
            // cmbOut
            // 
            this.cmbOut.FormattingEnabled = true;
            this.cmbOut.Location = new System.Drawing.Point(12, 170);
            this.cmbOut.Name = "cmbOut";
            this.cmbOut.Size = new System.Drawing.Size(211, 21);
            this.cmbOut.TabIndex = 36;
            // 
            // btnOutFile
            // 
            this.btnOutFile.Image = global::esriUtil.Properties.Resources.cmdOpenProject;
            this.btnOutFile.Location = new System.Drawing.Point(233, 168);
            this.btnOutFile.Name = "btnOutFile";
            this.btnOutFile.Size = new System.Drawing.Size(27, 27);
            this.btnOutFile.TabIndex = 35;
            this.btnOutFile.UseVisualStyleBackColor = true;
            this.btnOutFile.Click += new System.EventHandler(this.btnOutFile_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 152);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 42;
            this.label2.Text = "Out Results";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 97);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(106, 13);
            this.label3.TabIndex = 44;
            this.label3.Text = "Strata Field (optional)";
            // 
            // cmbStrata
            // 
            this.cmbStrata.FormattingEnabled = true;
            this.cmbStrata.Items.AddRange(new object[] {
            ""});
            this.cmbStrata.Location = new System.Drawing.Point(12, 115);
            this.cmbStrata.Name = "cmbStrata";
            this.cmbStrata.Size = new System.Drawing.Size(118, 21);
            this.cmbStrata.TabIndex = 43;
            this.cmbStrata.Text = " ";
            // 
            // nudAlpha
            // 
            this.nudAlpha.DecimalPlaces = 3;
            this.nudAlpha.Location = new System.Drawing.Point(162, 115);
            this.nudAlpha.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            131072});
            this.nudAlpha.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nudAlpha.Name = "nudAlpha";
            this.nudAlpha.Size = new System.Drawing.Size(60, 20);
            this.nudAlpha.TabIndex = 45;
            this.nudAlpha.Value = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(162, 97);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 13);
            this.label4.TabIndex = 46;
            this.label4.Text = "Alpha";
            // 
            // frmBootstrap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(276, 232);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.nudAlpha);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbStrata);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Field);
            this.Controls.Add(this.cmbField);
            this.Controls.Add(this.nudIter);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnSimulate);
            this.Controls.Add(this.cmbOut);
            this.Controls.Add(this.btnOutFile);
            this.Controls.Add(this.btnOpenFeatureClass);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbSampleFeatureClass);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmBootstrap";
            this.Text = "Bootstrap Estimates";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.nudIter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAlpha)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOpenFeatureClass;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbSampleFeatureClass;
        private System.Windows.Forms.Label Field;
        private System.Windows.Forms.ComboBox cmbField;
        private System.Windows.Forms.NumericUpDown nudIter;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnSimulate;
        private System.Windows.Forms.ComboBox cmbOut;
        private System.Windows.Forms.Button btnOutFile;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbStrata;
        private System.Windows.Forms.NumericUpDown nudAlpha;
        private System.Windows.Forms.Label label4;
    }
}