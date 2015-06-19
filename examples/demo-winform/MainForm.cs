using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using ClusteringAlgorithm;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace RunoffsClustering {
    public partial class MainForm : Form {
        public MainForm() {
            InitializeComponent();
            clusterNumberBox.SelectedIndex = 1;
            weightedIndexBox.SelectedIndex = 1;
            maxIterationsBox.SelectedIndex = 1;
            minImprovmentBox.SelectedIndex = 1;

            btnFCM.CheckedChanged += (sender, args) => weightedIndexBox.Enabled = btnFCM.Checked;
        }

        private static List<List<double>> ReadDataFromXml(string path) {
            var rows = new List<List<double>>();
            var xe = XElement.Load(path);
            var items =
                from element in xe.Elements("Data").Elements("Item")
                select element;

            foreach (var item in items) {
                var values = item.Value.Split(new[] {' ', ',', '\t'},
                    StringSplitOptions.RemoveEmptyEntries);
                var row = values.Select(double.Parse).ToList();
                rows.Add(row);
            }
            return rows;
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

        private int ClusterNumber => int.Parse(clusterNumberBox.SelectedItem.ToString());
        private double WeightedIndex => double.Parse(weightedIndexBox.SelectedItem.ToString());
        private int MaxIterations => int.Parse(maxIterationsBox.SelectedItem.ToString());
        private double MinImprovment => double.Parse(minImprovmentBox.SelectedItem.ToString());

        private string InputPath {
            get { return txtSourcePath.Text; }
            set { txtSourcePath.Text = value; }
        }

        private string OutputPath {
            get { return txtResultPath.Text; }
            set { txtResultPath.Text = value; }
        }

        private void btnSourcePath_Click(object sender, EventArgs e) {
            var dialog = new OpenFileDialog {
                Filter = @"径流数据文件(*.xml)|*.xml",
                Title = @"Source path"
            };
            if (dialog.ShowDialog() == DialogResult.OK)
                InputPath = dialog.FileName;
        }

        private void btnResultPath_Click(object sender, EventArgs e) {
            var dialog = new SaveFileDialog {
                Filter = @"径流数据文件(*.xml)|*.xml",
                Title = @"Result path"
            };
            if (dialog.ShowDialog() == DialogResult.OK)
                OutputPath = dialog.FileName;
        }

        private void btnRun_Click(object sender, EventArgs e) {
            try {
                var rows = ReadDataFromXml(InputPath);
                var matrix = DenseMatrix.OfRows(rows);

                ClusterResult result = null;
                if (btnKmeans.Checked) {
                    var kmeans = new Kmeans(matrix);
                    result = kmeans.Clustering(ClusterNumber, MaxIterations, MinImprovment);
                }
                else if (btnFCM.Checked) {
                    var cmeans = new Fcm(matrix);
                    result = cmeans.Cluster(ClusterNumber, WeightedIndex, MaxIterations,
                        MinImprovment);
                }

                WriteDataToXml(result, OutputPath);
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show(@"Clustering complete!");
        }

        private void btnExit_Click(object sender, EventArgs e) { Environment.Exit(0); }
    }
}