using System;

namespace ClusteringAlgorithm.ObservationTypes {
    public struct Point : IComparable<Point>
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public Point(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public int CompareTo(Point other) {
            if (X > other.X)
                return 1;
            if (X < other.X)
                return -1;

            if (Y > other.Y)
                return 1;
            if (Y < other.Y)
                return -1;

            if (Z > other.Z)
                return 1;
            if (Z < other.Z)
                return -1;

            return 0;
        }

        public static double Distance(Point p1, Point p2)
            => Math.Sqrt((p1.X - p2.X)*(p1.X - p2.X) +
                         (p1.Y - p2.Y)*(p1.Y - p2.Y) +
                         (p1.Z - p2.Z)*(p1.Z - p2.Z));

        public static Point operator +(Point p1, Point p2)
            => new Point(p1.X + p2.X, p1.Y + p2.Y, p1.Z + p2.Z);

        public static Point operator -(Point p1, Point p2)
            => new Point(p1.X - p2.X, p1.Y - p2.Y, p1.Z - p2.Z);

        public static Point operator /(Point p1, double div)
            => new Point(p1.X/div, p1.Y/div, p1.Z/div);
    }
}