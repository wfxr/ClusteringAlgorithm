using System;
using System.Linq;
using Wfxr.Container;
using Wfxr.Data;
using Xunit;

namespace ClusteringAlgorithm {
    public class TestKMeans {
        [Fact]
        public void TestCategoriesCount() {
            var observationSet = new Set<double> {1, 2, 3, 4, 5, 6, 7, 8, 9};
            var km = new KMeans<double>(observationSet, Distance1D, Centroid1D);

            Assert.Equal(km.Classify(1).Count, 1);
            Assert.Equal(km.Classify(3).Count, 3);
            Assert.Equal(km.Classify(6).Count, 6);
        }

        [Fact]
        public void TestInvalidCategroiesCount() {
            var observationSet = new Set<double> {1, 2, 3, 7, 8, 9};
            var km = new KMeans<double>(observationSet, Distance1D, Centroid1D);

            Assert.Throws<ArgumentOutOfRangeException>(() => km.Classify(0));
            Assert.Throws<ArgumentOutOfRangeException>(() => km.Classify(7));
        }

        [Fact]
        public void TestCentroiesOfResults() {
            var observationSet = new Set<double> {1, 2, 3, 7, 8, 9};
            var km = new KMeans<double>(observationSet, Distance1D, Centroid1D);

            var categories = km.Classify(2).OrderByCentroids();

            Assert.Equal(categories[0].Centroid, 2.0);
            Assert.Equal(categories[1].Centroid, 8.0);
        }

        [Fact]
        public void TestSetsOfResults() {
            var observationSet = new Set<double> {1, 2, 3, 7, 8, 9};
            var km = new KMeans<double>(observationSet, Distance1D, Centroid1D);

            var categories = km.Classify(2).OrderByCentroids();

            Assert.Equal(categories[0], new Category<double> {1, 2, 3});
            Assert.Equal(categories[1], new Category<double> {7, 8, 9});
        }

        [Fact]
        public void Test2DTuple() {
            var observationSet = new Set<Tuple<double, double>> {
                Tuple.Create(1.0, 1.0),
                Tuple.Create(0.0, 1.0),
                Tuple.Create(1.0, 1.0),
                Tuple.Create(4.0, 5.0),
                Tuple.Create(5.0, 4.0),
                Tuple.Create(4.0, 5.0),
                Tuple.Create(4.0, 4.0)
            };
            var km = new KMeans<Tuple<double, double>>(observationSet,
                (x, y) => Math.Sqrt((x.Item1 - y.Item1)*(x.Item1 - y.Item1) +
                                    (x.Item2 - y.Item2)*(x.Item2 - y.Item2)),
                (x, y) => Tuple.Create(x.Item1 + y.Item1, x.Item2 + y.Item2),
                (x, d) => Tuple.Create(x.Item1/d, x.Item2/d));

            var categories = km.Classify(2).OrderByCentroids();

            Assert.Equal(categories[0].Centroid, Tuple.Create(2.0/3, 3.0/3));
            Assert.Equal(categories[1].Centroid, Tuple.Create(17.0/4, 18.0/4));
        }

        [Fact]
        public void Test3DPoint() {
            var observationSet = new Set<Point> {
                new Point(1.0, 1.0, 1.0),
                new Point(0.0, 1.0, 1.0),
                new Point(1.0, 1.0, 0.0),
                new Point(4.0, 5.0, 5.0),
                new Point(5.0, 4.0, 4.0),
                new Point(4.0, 5.0, 4.0),
                new Point(4.0, 4.0, 4.0)
            };
            var km = new KMeans<Point>(observationSet, Point.Distance, (x, y) => x + y,
                (x, d) => x/d);

            var categories = km.Classify(2).OrderByCentroids();
            Assert.Equal(categories[0].Centroid, new Point(2.0/3, 3.0/3, 2.0/3));
            Assert.Equal(categories[1].Centroid, new Point(17.0/4, 18.0/4, 17.0/4));
        }

        private static double Centroid1D(Set<double> set) => set.Average();
        private static double Distance1D(double x, double y) => Math.Abs(x - y);
    }
}