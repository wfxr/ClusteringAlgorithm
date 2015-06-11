using System;
using System.Collections.Generic;
using System.Windows;
using MathNet.Numerics.LinearAlgebra;

namespace ChartTest {
    public class PointCollection {
        public PointCollection(List<Point> itemsSource) { ItemsSource = itemsSource; }
        public PointCollection(double[,] array) { ItemsSource = PointsFromArray(array); }

        public PointCollection(Matrix<double> matrix) { ItemsSource = PointsFromMatrix(matrix); }

        public static List<Point> PointsFromArray(double[,] array) {
            var row = array.GetLength(0);
            var column = array.GetLength(1);
            if (column != 2)
                throw new ArgumentException($"column count should be 2 (which is {column})");
            var points = new List<Point>();
            for (var i = 0; i < row; ++i)
                points.Add(new Point(array[i, 0], array[i, 1]));

            return points;
        }

        public static List<Point> PointsFromMatrix(Matrix<double> matrix)
            => PointsFromArray(matrix.ToArray());

        public List<Point> ItemsSource { get; }
    }
}