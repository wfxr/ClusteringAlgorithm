using System;
using System.Collections.Generic;
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

        private int ClusterNumber => int.Parse(clusterNumberBox.SelectedItem.ToString());
        private double WeightedIndex => double.Parse(weightedIndexBox.SelectedItem.ToString());
        private int MaxIterations => int.Parse(maxIterationsBox.SelectedItem.ToString());
        private double MinImprovment => double.Parse(minImprovmentBox.SelectedItem.ToString());

        private string InputPath {
            get { return txtSourcePath.Text; }
            set { txtSourcePath.Text = value; }
        }

        private void btnSourcePath_Click(object sender, EventArgs e) {
            var dialog = new OpenFileDialog {
                Filter = @"径流数据文件(*.xml)|*.xml",
                Title = @"Source path"
            };
            if (dialog.ShowDialog() == DialogResult.OK)
                InputPath = dialog.FileName;
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

                new ResultDisplayForm(result).Show();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExit_Click(object sender, EventArgs e) { Environment.Exit(0); }
    }
}