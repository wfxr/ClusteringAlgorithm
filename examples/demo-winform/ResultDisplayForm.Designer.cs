namespace RunoffsClustering
{
    partial class ResultDisplayForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSaveToXml = new System.Windows.Forms.Button();
            this.centerTable = new System.Windows.Forms.ListView();
            this.clusterTable = new System.Windows.Forms.ListView();
            this.idxTable = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "聚类编号：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(185, 9);
            this.label2.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 21);
            this.label2.TabIndex = 0;
            this.label2.Text = "聚类中心：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(185, 181);
            this.label3.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 21);
            this.label3.TabIndex = 0;
            this.label3.Text = "聚类列表：";
            // 
            // btnSaveToXml
            // 
            this.btnSaveToXml.Location = new System.Drawing.Point(360, 628);
            this.btnSaveToXml.Margin = new System.Windows.Forms.Padding(5);
            this.btnSaveToXml.Name = "btnSaveToXml";
            this.btnSaveToXml.Size = new System.Drawing.Size(125, 41);
            this.btnSaveToXml.TabIndex = 3;
            this.btnSaveToXml.Text = "Save to xml";
            this.btnSaveToXml.UseVisualStyleBackColor = true;
            this.btnSaveToXml.Click += new System.EventHandler(this.btnSaveToXml_Click);
            // 
            // centerTable
            // 
            this.centerTable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.centerTable.Location = new System.Drawing.Point(189, 33);
            this.centerTable.Name = "centerTable";
            this.centerTable.Size = new System.Drawing.Size(644, 145);
            this.centerTable.TabIndex = 4;
            this.centerTable.UseCompatibleStateImageBehavior = false;
            this.centerTable.View = System.Windows.Forms.View.Details;
            // 
            // clusterTable
            // 
            this.clusterTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.clusterTable.Location = new System.Drawing.Point(189, 205);
            this.clusterTable.Name = "clusterTable";
            this.clusterTable.Size = new System.Drawing.Size(644, 415);
            this.clusterTable.TabIndex = 5;
            this.clusterTable.UseCompatibleStateImageBehavior = false;
            this.clusterTable.View = System.Windows.Forms.View.Details;
            // 
            // idxTable
            // 
            this.idxTable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.idxTable.Location = new System.Drawing.Point(12, 33);
            this.idxTable.Name = "idxTable";
            this.idxTable.Size = new System.Drawing.Size(165, 587);
            this.idxTable.TabIndex = 6;
            this.idxTable.UseCompatibleStateImageBehavior = false;
            this.idxTable.View = System.Windows.Forms.View.Details;
            // 
            // ResultDisplayForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(845, 683);
            this.Controls.Add(this.idxTable);
            this.Controls.Add(this.clusterTable);
            this.Controls.Add(this.centerTable);
            this.Controls.Add(this.btnSaveToXml);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "ResultDisplayForm";
            this.Text = "ResultDisplayForm";
            this.Load += new System.EventHandler(this.ResultDisplayForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSaveToXml;
        private System.Windows.Forms.ListView centerTable;
        private System.Windows.Forms.ListView clusterTable;
        private System.Windows.Forms.ListView idxTable;
    }
}