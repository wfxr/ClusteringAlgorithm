﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using ClusteringAlgorithm;
using DevExpress.Xpf.Charts;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using Wfxr.Utility.Container;

namespace ChartTest {
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private List<double[]> _pointData;
        public MainWindow() { InitializeComponent(); }
        private int ClusterNumber => int.Parse(ClusterNumberBox.SelectedItem.ToString());
        private double WeightedIndex => double.Parse(WeightedIndexBox.SelectedItem.ToString());
        private int MaxIterations => int.Parse(MaxIterationsBox.SelectedItem.ToString());
        private double MinImprovment => double.Parse(MinImprovmentBox.SelectedItem.ToString());

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            ClusterNumberBox.ItemsSource = Enumerable.Range(2, 8);
            ClusterNumberBox.SelectedIndex = 1;
            WeightedIndexBox.ItemsSource = new List<double> {1.5, 2.0, 2.5, 3.0};
            WeightedIndexBox.SelectedIndex = 1;
            MaxIterationsBox.ItemsSource = new List<int> {100, 200, 500, 1000};
            MaxIterationsBox.SelectedIndex = 1;
            MinImprovmentBox.ItemsSource = new List<double> {1e-4, 1e-5, 1e-6, 1e-7};
            MinImprovmentBox.SelectedIndex = 1;

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
        private List<Point> CreatePointList(IEnumerable<double[]> list)
            => CreatePointList(DenseMatrix.OfRowArrays(list));

        private List<List<Point>> GroupPoints(Matrix<double> m, int[] idx) {
            var row = m.RowCount;
            var col = m.ColumnCount;
            var groupCount = idx.Max() + 1;
            if (col != 2) throw new FormatException("not a n*2 matrix");

            var group = new List<List<Point>>(groupCount).PopulateDefault();
            for (var i = 0; i < row; ++i)
                group[idx[i]].Add(new Point(m[i, 0], m[i, 1]));

            return group;
        }

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

        private void RunButton_Click(object sender, RoutedEventArgs e) {
            var matrix = DenseMatrix.OfRowArrays(_pointData);
            ClusterReport report = null;
            if (KmeansRadio.IsChecked == true) {
                var kmeans = new Kmeans(matrix);
                report = kmeans.Run(ClusterNumber, MaxIterations, MinImprovment);
            }
            else if (CmeansRadio.IsChecked == true) {
                var cmeans = new Fcm(matrix);
                report = cmeans.Run(ClusterNumber, WeightedIndex, MaxIterations,
                    MinImprovment);
            }

            if (report == null) return;

            diagram.Series.Clear();
            var groups = GroupPoints(report.Obs, report.Idx);
            foreach (var group in groups) 
                AddSeriesOfPoints(group);

            var centers = CreatePointList(report.Center);
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

    [ValueConversion(typeof (bool), typeof (Visibility))]
    public class BoolToVisibilityConverter : IValueConverter {
        public BoolToVisibilityConverter() : this(true) { }

        public BoolToVisibilityConverter(bool collapsewhenInvisible) {
            CollapseWhenInvisible = collapsewhenInvisible;
        }

        public bool CollapseWhenInvisible { get; set; }

        public Visibility FalseVisibility
            => CollapseWhenInvisible ? Visibility.Collapsed : Visibility.Hidden;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value == null)
                return Visibility.Visible;
            return (bool) value ? Visibility.Visible : FalseVisibility;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            CultureInfo culture) {
            if (value == null)
                return true;
            return ((Visibility) value == Visibility.Visible);
        }
    }
}