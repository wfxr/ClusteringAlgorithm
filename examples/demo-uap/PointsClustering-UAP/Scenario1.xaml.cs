using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ClusteringAlgorithm;
using DevExpress.UI.Xaml.Charts;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace PointsClustering_UAP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Scenario1 : Page
    {
        private List<double[]> _pointData;
        public Scenario1()
        {
            this.InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _pointData = PointDataFromFile("PointData.txt").Result;
            var points = CreatePointList(_pointData);
            AddSeriesOfPoints(points);
        }

        private List<Point> CreatePointList(Matrix<double> m)
        {
            var row = m.RowCount;
            var col = m.ColumnCount;
            if (col != 2) throw new FormatException("not a n*2 matrix");
            var points = new List<Point>();
            for (var i = 0; i < row; ++i)
                points.Add(new Point(m[i, 0], m[i, 1]));
            return points;
        }

        private Point CreatePointList(IList<double> v)
        {
            if (v.Count != 2) throw new FormatException("not a 2D vector");
            return new Point(v[1], v[2]);
        }

        private List<Point> CreatePointList(IEnumerable<double[]> list)
            => CreatePointList(DenseMatrix.OfRowArrays(list));

        private List<Point> CreatePointList(IEnumerable<Vector<double>> list)
            => CreatePointList(DenseMatrix.OfRowVectors(list));

        private static async Task<List<double[]>> PointDataFromFile(string path)
        {
            var points = new List<double[]>();
            //var lines = File.ReadAllLines(path);
            var file = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync(path);
            var lines = await Windows.Storage.FileIO.ReadLinesAsync(file);

            foreach (var line in lines)
            {
                var xy = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if (xy.Length == 0) continue;
                if (xy.Length != 2)
                    throw new FormatException("there should be 2 number in one line");
                var x = double.Parse(xy[0]);
                var y = double.Parse(xy[1]);
                points.Add(new[] { x, y });
            }
            return points;
        }

        private void ButtonClustering_Click(object sender, RoutedEventArgs e)
        {
            //var matrix = DenseMatrix.OfRowArrays(_pointData);
            //var kmeans = new Kmeans(matrix);
            //var resultKmeans = kmeans.Clustering(3);

            //diagram.Series.Clear();
            //foreach (var cluster in resultKmeans.Clusters)
            //{
            //    var points = CreatePointList(cluster);
            //    AddSeriesOfPoints(points);
            //}

            //var centers = CreatePointList(resultKmeans.Center);
            //AddSeriesOfPoints(centers, 10);
        }

        private void AddSeriesOfPoints(IReadOnlyCollection<Point> points, int pointSize = 4)
        {
            //// Create a point series.
            //var series = new Series
            //{
            //    DataContext = points,
            //};
            //series.Data = new DataSourceAdapter(points);

            //// Add it to series of the diagram
            //diagram.Series.Add(series);
        }
    }
}
