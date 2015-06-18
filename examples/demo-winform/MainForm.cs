using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using ClusteringAlgorithm;
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
            var runoffData =
                from element in xe.Elements("Data").Elements("Item")
                select element;


            foreach (var runoff in runoffData) {
                var values = runoff.Value.Split(new[] {' ', ',', '\t'},
                    StringSplitOptions.RemoveEmptyEntries);
                var row = new List<double>();
                foreach (var value in values)
                    row.Add(double.Parse(value));
                rows.Add(row);
            }
            return rows;
        }

        private static void WriteDataToXml(ClusterResult result, string path) {
            var root = new XElement("Result");
            var membershipVectorNode = new XElement("MembershipVector");
            root.Add(membershipVectorNode);

            var vectorString = result.UV.Aggregate(string.Empty,
                (current, numCluster) => current + (numCluster + " "));
            membershipVectorNode.Value = vectorString;
            root.Save(path);
            MessageBox.Show(@"Complete!");
        }

        private int ClusterNumber => int.Parse(clusterNumberBox.SelectedItem.ToString());
        private double WeightedIndex => double.Parse(weightedIndexBox.SelectedItem.ToString());
        private int MaxIterations => int.Parse(maxIterationsBox.SelectedItem.ToString());
        private double MinImprovment => double.Parse(minImprovmentBox.SelectedItem.ToString());

        private string SourceFilePath {
            get { return txtSourcePath.Text; }
            set { txtSourcePath.Text = value; }
        }

        private string ResultFilePath {
            get { return txtResultPath.Text; }
            set { txtResultPath.Text = value; }
        }

        private void btnSourcePath_Click(object sender, EventArgs e) {
            var dialog = new OpenFileDialog {
                Filter = @"径流数据文件(*.xml)|*.xml",
                Title = @"Source path"
            };
            if (dialog.ShowDialog() == DialogResult.OK)
                SourceFilePath = dialog.FileName;
        }

        private void btnResultPath_Click(object sender, EventArgs e) {
            var dialog = new SaveFileDialog {
                Filter = @"径流数据文件(*.xml)|*.xml",
                Title = @"Result path"
            };
            if (dialog.ShowDialog() == DialogResult.OK)
                ResultFilePath = dialog.FileName;
        }

        private void btnRun_Click(object sender, EventArgs e) {
            var rows = ReadDataFromXml(SourceFilePath);
            var matrix = DenseMatrix.OfRows(rows);
            ClusterResult result = null;
            if (btnKmeans.Checked) {
                var kmeans = new Kmeans(matrix);
                result = kmeans.Clustering(ClusterNumber, MaxIterations, MinImprovment);
            }
            else if (btnFCM.Checked) {
                var cmeans = new Fcm(matrix);
                result = cmeans.Cluster(ClusterNumber, WeightedIndex, MaxIterations, MinImprovment);
            }

            WriteDataToXml(result, ResultFilePath);
        }
    }
}