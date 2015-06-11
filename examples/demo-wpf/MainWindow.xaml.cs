using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using ClusteringAlgorithm;
using DevExpress.Xpf.Charts;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace ChartTest {
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private List<double[]> _pointData;
        public MainWindow() { InitializeComponent(); }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            _pointData = PointDataFromFile("PointData.txt");
            var points = CreatePointList(_pointData);
            AddSeriesOfPoints(points);
        }

        private List<Point> CreatePointList(Matrix<double> m) {
            var row = m.RowCount;
            var col = m.ColumnCount;
            if (col != 2) throw new FormatException("not a n*2 matrix");
            var points = new List<Point>();
            for (var i = 0; i < row; ++i)
                points.Add(new Point(m[i, 0], m[i, 1]));
            return points;
        }

        private Point CreatePointList(IList<double> v) {
            if (v.Count != 2) throw new FormatException("not a 2D vector");
            return new Point(v[1], v[2]);
        }

        private List<Point> CreatePointList(IEnumerable<double[]> list)
            => CreatePointList(DenseMatrix.OfRowArrays(list));

        private List<Point> CreatePointList(IEnumerable<Vector<double>> list)
            => CreatePointList(DenseMatrix.OfRowVectors(list));

        private static List<double[]> PointDataFromFile(string path) {
            var points = new List<double[]>();
            var lines = File.ReadAllLines(path);
            foreach (var line in lines) {
                var xy = line.Split(new[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries);
                if (xy.Length == 0) continue;
                if (xy.Length != 2)
                    throw new FormatException("there should be 2 number in one line");
                var x = double.Parse(xy[0]);
                var y = double.Parse(xy[1]);
                points.Add(new[] {x, y});
            }
            return points;
        }

        private void ButtonClustering_Click(object sender, RoutedEventArgs e) {
            var matrix = DenseMatrix.OfRowArrays(_pointData);
            var kmeans = new Kmeans(matrix);
            var resultKmeans = kmeans.Clustering(3);

            diagram.Series.Clear();
            foreach (var cluster in resultKmeans.Clusters) {
                var points = CreatePointList(cluster);
                AddSeriesOfPoints(points);
            }

            var centers = CreatePointList(resultKmeans.Center);
            AddSeriesOfPoints(centers, 10);
        }

        private void AddSeriesOfPoints(IReadOnlyCollection<Point> points, int pointSize = 4) {
            // Create a point series.
            var series = new PointSeries2D {
                DataSource = points,
                MarkerSize = pointSize,
                ArgumentDataMember = "X",
                ValueDataMember = "Y"
            };

            // Add it to series of the diagram
            diagram.Series.Add(series);
        }
    }
}