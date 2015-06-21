using System;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using ClusteringAlgorithm;
using MathNet.Numerics.LinearAlgebra;

namespace RunoffsClustering {
    public partial class ResultDisplayForm : Form {
        private readonly ClusterResult _clusterResult;
        //private BindingSource _CenterSource = new BindingSource();

        public ResultDisplayForm(ClusterResult clusterResult) {
            InitializeComponent();
            _clusterResult = clusterResult;
        }

        private void ResultDisplayForm_Load(object sender, EventArgs e) {
            centerTable.Columns.Add("idx");
            // 聚类编号序列
            var idxSeq = _clusterResult.IDX;
            idxTable.Columns.Add("obs");
            idxTable.Columns.Add("idx");
            for (var i = 0; i < idxSeq.Count; ++i) {
                var item = new ListViewItem((i + 1).ToString());
                item.SubItems.Add(idxSeq[i].ToString());
                idxTable.Items.Add(item);
            }

            var columnCount = _clusterResult.Center.ColumnCount;

            // 中心列表
            var centers = _clusterResult.Center;
            for (var i = 0; i < columnCount; ++i)
                centerTable.Columns.Add($"d{i+1}");

            for (var i = 0; i < centers.RowCount; ++i) {
                var item = new ListViewItem(i.ToString());
                for (var j = 0; j < columnCount; ++j)
                    item.SubItems.Add($"{centers[i, j]:0}");
                centerTable.Items.Add(item);
            }

            // 聚类列表
            clusterTable.Columns.Add("idx");
            var clusters = _clusterResult.Clusters;
            for (var i = 0; i < columnCount; ++i)
                clusterTable.Columns.Add($"d{i + 1}");

            for (var idx = 0; idx < clusters.Count; ++idx) {
                for (var i = 0; i < clusters[idx].RowCount; ++i) {
                    var item = new ListViewItem(idx.ToString());
                    for (var j = 0; j < clusters[idx].ColumnCount; ++j)
                        item.SubItems.Add($"{clusters[idx][i, j]:0}");
                    clusterTable.Items.Add(item);
                }
            }
        }

        private static void WriteDataToXml(ClusterResult result, string path) {
            var root = new XElement("ClusterResult");
            var indexNode = new XElement("Index");
            var centersNode = new XElement("Centers");
            var clustersNode = new XElement("Clusters");

            // 聚类索引序列
            var indexString = result.IDX.Aggregate(string.Empty,
                (current, numCluster) => current + (numCluster + " "));
            indexNode.Value = indexString.Trim();

            // 聚类中心
            var centerRows = MatrixToRowStrings(result.Center);
            for (var i = 0; i < centerRows.Length; ++i) {
                var centerElement = new XElement("Center");
                centerElement.SetAttributeValue("Index", i);
                centersNode.Add(centerElement);
                centerElement.Value = centerRows[i];
            }

            // 聚类列表
            var clusters = result.Clusters;
            for (var i = 0; i < clusters.Count; ++i) {
                var clusterNode = new XElement("Cluster");
                clustersNode.Add(clusterNode);
                clusterNode.SetAttributeValue("Index", i);
                clusterNode.SetAttributeValue("Count", clusters[i].RowCount);

                var clusterRows = MatrixToRowStrings(clusters[i]);
                foreach (var t in clusterRows) {
                    var ele = new XElement("Item");
                    clusterNode.Add(ele);
                    ele.Value = t;
                }
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
            var dialog = new SaveFileDialog {Filter = @"xml file|*.xml"};
            if (dialog.ShowDialog() == DialogResult.OK)
                WriteDataToXml(_clusterResult, dialog.FileName);
        }
    }
}