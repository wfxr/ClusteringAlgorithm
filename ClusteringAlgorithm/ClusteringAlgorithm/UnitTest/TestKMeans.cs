using System;
using System.Linq;
using Xunit;

namespace ClusteringAlgorithm.UnitTest {
    public class TestKMeans {
        [Fact]
        public void TestCategoriesCount() {
            var observationSet = new ObservationSet<double> {1, 2, 3, 4, 5, 6, 7, 8, 9};
            var km = new KMeans<double>(observationSet, Distance1D, Centroid1D);

            Assert.Equal(km.Classify(1).Count, 1);
            Assert.Equal(km.Classify(3).Count, 3);
            Assert.Equal(km.Classify(6).Count, 6);
        }

        [Fact]
        public void TestInvalidCategroiesCount() {
            var observationSet = new ObservationSet<double> {1, 2, 3, 7, 8, 9};
            var km = new KMeans<double>(observationSet, Distance1D, Centroid1D);

            Assert.Throws<ArgumentOutOfRangeException>(() => km.Classify(0));
            Assert.Throws<ArgumentOutOfRangeException>(() => km.Classify(7));
        }

        [Fact]
        public void TestCentroiesOfResults() {
            var observationSet = new ObservationSet<double> {1, 2, 3, 7, 8, 9};
            var km = new KMeans<double>(observationSet, Distance1D, Centroid1D);

            var categories = km.Classify(2).OrderByCentroids();

            Assert.Equal(categories[0].Centroid, 2.0);
            Assert.Equal(categories[1].Centroid, 8.0);
        }

        [Fact]
        public void TestSetsOfResults() {
            var observationSet = new ObservationSet<double> {1, 2, 3, 7, 8, 9};
            var km = new KMeans<double>(observationSet, Distance1D, Centroid1D);

            var categories = km.Classify(2).OrderByCentroids();

            Assert.Equal(categories[0].ObservationSet, new ObservationSet<double> {1, 2, 3});
            Assert.Equal(categories[1].ObservationSet, new ObservationSet<double> {7, 8, 9});
        }

        [Fact]
        public void Test2DTuple() {
            var observationSet = new ObservationSet<Tuple<double, double>> {
                Tuple.Create(1.0, 1.0),
                Tuple.Create(0.0, 1.0),
                Tuple.Create(1.0, 1.0),
                Tuple.Create(4.0, 5.0),
                Tuple.Create(5.0, 4.0),
                Tuple.Create(4.0, 5.0),
                Tuple.Create(4.0, 4.0)
            };
            var km = new KMeans<Tuple<double, double>>(observationSet, Distance2D,
                Centroid2D);

            var categories = km.Classify(2).OrderByCentroids();

            Assert.Equal(categories[0].Centroid, Tuple.Create(2.0/3, 3.0/3));
            Assert.Equal(categories[1].Centroid, Tuple.Create(17.0/4, 18.0/4));
        }

        [Fact]
        public void Test3DPoint() {
            var observationSet = new ObservationSet<Point> {
                new Point(1.0, 1.0, 1.0),
                new Point(0.0, 1.0, 1.0),
                new Point(1.0, 1.0, 0.0),
                new Point(4.0, 5.0, 5.0),
                new Point(5.0, 4.0, 4.0),
                new Point(4.0, 5.0, 4.0),
                new Point(4.0, 4.0, 4.0)
            };
            var km = new KMeans<Point>(observationSet, Point.Distance, Centroid3D);

            var categories = km.Classify(2).OrderByCentroids();
            Assert.Equal(categories[0].Centroid, new Point(2.0/3, 3.0/3, 2.0/3));
            Assert.Equal(categories[1].Centroid, new Point(17.0/4, 18.0/4, 17.0/4));
        }

        private static double Centroid1D(ObservationSet<double> observationSet)
            => observationSet.Average();

        private static Tuple<double, double> Centroid2D(
            ObservationSet<Tuple<double, double>> observationSet) {
            var sum1 = 0.0;
            var sum2 = 0.0;
            foreach (var obs in observationSet) {
                sum1 += obs.Item1;
                sum2 += obs.Item2;
            }
            var count = observationSet.Count;
            return Tuple.Create(sum1/count, sum2/count);
        }

        private static Point Centroid3D(ObservationSet<Point> observationSet) {
            var point = new Point(0, 0, 0);
            foreach (var obs in observationSet)
                point += obs;
            var count = observationSet.Count;
            return point/count;
        }

        private static double Distance1D(double obs1, double obs2) => Math.Abs(obs1 - obs2);

        private static double Distance2D(Tuple<double, double> obs1, Tuple<double, double> obs2)
            => Math.Sqrt(Math.Pow((obs1.Item1 - obs2.Item1), 2) + Math.Pow((obs1.Item2 - obs2.Item2), 2));
    }
}