namespace RunoffsClustering
{
    partial class MainForm
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
            this.btnSourcePath = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSourcePath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnResultPath = new System.Windows.Forms.Button();
            this.txtResultPath = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnFCM = new System.Windows.Forms.RadioButton();
            this.btnKmeans = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.minImprovmentBox = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.maxIterationsBox = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.weightedIndexBox = new System.Windows.Forms.ComboBox();
            this.clusterNumberBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnRun = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSourcePath
            // 
            this.btnSourcePath.Location = new System.Drawing.Point(484, 57);
            this.btnSourcePath.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnSourcePath.Name = "btnSourcePath";
            this.btnSourcePath.Size = new System.Drawing.Size(41, 26);
            this.btnSourcePath.TabIndex = 2;
            this.btnSourcePath.Text = "...";
            this.btnSourcePath.UseVisualStyleBackColor = true;
            this.btnSourcePath.Click += new System.EventHandler(this.btnSourcePath_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(54, 32);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "Source file path:";
            // 
            // txtSourcePath
            // 
            this.txtSourcePath.Location = new System.Drawing.Point(58, 57);
            this.txtSourcePath.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtSourcePath.Name = "txtSourcePath";
            this.txtSourcePath.Size = new System.Drawing.Size(467, 26);
            this.txtSourcePath.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(54, 98);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 10, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 20);
            this.label2.TabIndex = 6;
            this.label2.Text = "Result file path:";
            // 
            // btnResultPath
            // 
            this.btnResultPath.Location = new System.Drawing.Point(484, 123);
            this.btnResultPath.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnResultPath.Name = "btnResultPath";
            this.btnResultPath.Size = new System.Drawing.Size(41, 26);
            this.btnResultPath.TabIndex = 5;
            this.btnResultPath.Text = "...";
            this.btnResultPath.UseVisualStyleBackColor = true;
            this.btnResultPath.Click += new System.EventHandler(this.btnResultPath_Click);
            // 
            // txtResultPath
            // 
            this.txtResultPath.Location = new System.Drawing.Point(58, 123);
            this.txtResultPath.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtResultPath.Name = "txtResultPath";
            this.txtResultPath.Size = new System.Drawing.Size(467, 26);
            this.txtResultPath.TabIndex = 7;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnFCM);
            this.groupBox1.Controls.Add(this.btnKmeans);
            this.groupBox1.Location = new System.Drawing.Point(58, 164);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(10);
            this.groupBox1.Size = new System.Drawing.Size(467, 77);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Clustering algorithm";
            // 
            // btnFCM
            // 
            this.btnFCM.AutoSize = true;
            this.btnFCM.Checked = true;
            this.btnFCM.Location = new System.Drawing.Point(255, 32);
            this.btnFCM.Name = "btnFCM";
            this.btnFCM.Size = new System.Drawing.Size(134, 24);
            this.btnFCM.TabIndex = 0;
            this.btnFCM.TabStop = true;
            this.btnFCM.Text = "Fuzzy c-means";
            this.btnFCM.UseVisualStyleBackColor = true;
            // 
            // btnKmeans
            // 
            this.btnKmeans.AutoSize = true;
            this.btnKmeans.Location = new System.Drawing.Point(25, 32);
            this.btnKmeans.Name = "btnKmeans";
            this.btnKmeans.Size = new System.Drawing.Size(90, 24);
            this.btnKmeans.TabIndex = 0;
            this.btnKmeans.Text = "K-means";
            this.btnKmeans.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.minImprovmentBox);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.maxIterationsBox);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.weightedIndexBox);
            this.groupBox2.Controls.Add(this.clusterNumberBox);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(58, 254);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(10);
            this.groupBox2.Size = new System.Drawing.Size(467, 167);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Parameters";
            // 
            // minImprovmentBox
            // 
            this.minImprovmentBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.minImprovmentBox.FormattingEnabled = true;
            this.minImprovmentBox.Items.AddRange(new object[] {
            "1E-4",
            "1E-5",
            "1E-6",
            "1E-7"});
            this.minImprovmentBox.Location = new System.Drawing.Point(255, 117);
            this.minImprovmentBox.Name = "minImprovmentBox";
            this.minImprovmentBox.Size = new System.Drawing.Size(194, 28);
            this.minImprovmentBox.TabIndex = 0;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(251, 94);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(162, 20);
            this.label6.TabIndex = 6;
            this.label6.Text = "Minimum improvment:";
            // 
            // maxIterationsBox
            // 
            this.maxIterationsBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.maxIterationsBox.FormattingEnabled = true;
            this.maxIterationsBox.Items.AddRange(new object[] {
            "100",
            "200",
            "500",
            "1000"});
            this.maxIterationsBox.Location = new System.Drawing.Point(25, 117);
            this.maxIterationsBox.Name = "maxIterationsBox";
            this.maxIterationsBox.Size = new System.Drawing.Size(194, 28);
            this.maxIterationsBox.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(21, 94);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(145, 20);
            this.label5.TabIndex = 6;
            this.label5.Text = "Maxiumu iterations:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(251, 29);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(122, 20);
            this.label4.TabIndex = 6;
            this.label4.Text = "Weighted index:";
            // 
            // weightedIndexBox
            // 
            this.weightedIndexBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.weightedIndexBox.FormattingEnabled = true;
            this.weightedIndexBox.Items.AddRange(new object[] {
            "1.5",
            "2.0",
            "2.5",
            "3.0"});
            this.weightedIndexBox.Location = new System.Drawing.Point(255, 52);
            this.weightedIndexBox.Name = "weightedIndexBox";
            this.weightedIndexBox.Size = new System.Drawing.Size(194, 28);
            this.weightedIndexBox.TabIndex = 0;
            // 
            // clusterNumberBox
            // 
            this.clusterNumberBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.clusterNumberBox.FormattingEnabled = true;
            this.clusterNumberBox.Items.AddRange(new object[] {
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10"});
            this.clusterNumberBox.Location = new System.Drawing.Point(25, 52);
            this.clusterNumberBox.Name = "clusterNumberBox";
            this.clusterNumberBox.Size = new System.Drawing.Size(194, 28);
            this.clusterNumberBox.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 29);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(129, 20);
            this.label3.TabIndex = 6;
            this.label3.Text = "Clusters number:";
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(196, 441);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(184, 30);
            this.btnRun.TabIndex = 10;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(578, 490);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnResultPath);
            this.Controls.Add(this.txtResultPath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSourcePath);
            this.Controls.Add(this.txtSourcePath);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSourcePath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSourcePath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnResultPath;
        private System.Windows.Forms.TextBox txtResultPath;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton btnFCM;
        private System.Windows.Forms.RadioButton btnKmeans;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox clusterNumberBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox minImprovmentBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox maxIterationsBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox weightedIndexBox;
        private System.Windows.Forms.Button btnRun;
    }
}

