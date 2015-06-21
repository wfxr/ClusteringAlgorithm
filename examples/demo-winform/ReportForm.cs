using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using ClusteringAlgorithm;
using MathNet.Numerics.LinearAlgebra;

namespace RunoffsClustering {
    public partial class ReportForm : Form {
        private ClusterReport ClusterReport { get; }
        private DataTable CenterTable { get; } = new DataTable("CenterTable");
        private DataTable ClusterTable { get; } = new DataTable("ClusterTable");
        private int ObsDimension => ClusterReport.ObsDimension;

        public ReportForm(ClusterReport clusterReport) {
            InitializeComponent();
            ClusterReport = clusterReport;
        }

        private void ResultDisplayForm_Load(object sender, EventArgs e) {
            SetCenterTable();
            SetClusterTable();

            clusterView.DefaultCellStyle.Format = "0";
            centerView.DataSource = CenterTable;
            centerView.DefaultCellStyle.Format = "0";
            clusterView.DataSource = ClusterTable;
        }

        private void SetClusterTable() {
            ClusterTable.Columns.Add("ObsIdx", typeof (int)).Unique = true;
            ClusterTable.Columns.Add("ClusterIdx", typeof(int));

            var obs = ClusterReport.Obs;
            var idx = ClusterReport.Idx;
            for (var i = 0; i < ObsDimension; ++i)
                ClusterTable.Columns.Add($"d{i + 1}");


            for (var i = 0; i < obs.RowCount; i++) {
                var tableRow = ClusterTable.NewRow();
                tableRow[0] = i + 1;
                tableRow[1] = idx[i] + 1;        // 矩阵首列为所属聚类的编号(从0开始)
                for (var j = 0; j < obs.ColumnCount; ++j)
                    tableRow[j + 2] = obs[i, j];
                ClusterTable.Rows.Add(tableRow);
            }
        }

        private void SetCenterTable() {
            CenterTable.Columns.Add("ClusterIdx", typeof(int)).Unique = true;
            CenterTable.Columns.Add("ObsCount", typeof (int));

            var centers = ClusterReport.Center;
            var idx = ClusterReport.Idx;
            for (var i = 0; i < ObsDimension; ++i)
                CenterTable.Columns.Add($"d{i + 1}", typeof(double));

            for (var i = 0; i < centers.RowCount; ++i) {
                var row = CenterTable.NewRow();
                row[0] = i + 1;
                row[1] = idx.Count(item => item == i);

                for (var j = 0; j < ObsDimension; ++j)
                    row[j + 2] = centers[i, j];
                CenterTable.Rows.Add(row);
            }
        }
        
        // TODO:修改为从DataTable而非report写入
        private static void WriteDataToXml(ClusterReport report, string path) {
            var root = new XElement("ClusterReport");
            var indexNode = new XElement("Index");
            var centersNode = new XElement("Centers");
            var clustersNode = new XElement("Result");

            // 聚类索引序列
            var indexString = report.Idx.Aggregate(string.Empty,
                (current, numCluster) => current + (numCluster + " "));
            indexNode.Value = indexString.Trim();

            // 聚类中心
            var centerRows = MatrixToRowStrings(report.Center);
            for (var i = 0; i < centerRows.Length; ++i) {
                var centerElement = new XElement("Center");
                centerElement.SetAttributeValue("Index", i);
                centersNode.Add(centerElement);
                centerElement.Value = centerRows[i];
            }

            // 聚类列表
            var obs = MatrixToRowStrings(report.Obs);
            var idx = report.Idx;
            for (var i = 0; i < obs.Length; ++i) {
                var ele = new XElement("Item");
                ele.SetAttributeValue("Index", idx[i]);
                ele.Value = obs[i];
                clustersNode.Add(ele);
            }

            root.Add(indexNode);
            root.Add(centersNode);
            root.Add(clustersNode);
            root.Save(path);
        }

        private static string[] MatrixToRowStrings(Matrix<double> matrix) {
            return matrix.ToMatrixString(matrix.RowCount, matrix.ColumnCount)
                .Split(new[] {'\n', '\r'}, StringSplitOptions.RemoveEmptyEntries);
        }

        private void btnSaveToXml_Click(object sender, EventArgs e) {
            var dialog = new SaveFileDialog {
                FileName = "result",
                Filter = @"xml file|*.xml",
            };
            if (dialog.ShowDialog() == DialogResult.OK) {
                WriteDataToXml(ClusterReport, dialog.FileName);

                // 询问是否打开文件
                if (MessageBox.Show(@"Write succeed. Open it now?", @"Report",
                    MessageBoxButtons.YesNo) == DialogResult.Yes)
                    Process.Start(dialog.FileName);
            }
        }

        private void btnExit_Click(object sender, EventArgs e) => Close();
    }
}