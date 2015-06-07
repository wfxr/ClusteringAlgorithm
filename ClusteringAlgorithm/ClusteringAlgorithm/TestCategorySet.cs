using System;
using System.Collections.Generic;
using System.Linq;
using Wfxr.Utility.Container;
using Wfxr.Utility.Data;
using Xunit;

namespace ClusteringAlgorithm {
    public class TestCategorySet {
        [Fact]
        public void TestClearAllObservations() {
            var categorySet = new CategorySet<double>(Distance, Average) {
                new Category<double> {1, 3, 2, 4},
                new Category<double> {2, 5, 3, 1}
            };
            Assert.NotEmpty(categorySet);
            Assert.NotEmpty(categorySet[0]);
            Assert.NotEmpty(categorySet[1]);

            categorySet.ClearAllObservations();
            Assert.NotEmpty(categorySet);
            Assert.Empty(categorySet[0]);
            Assert.Empty(categorySet[1]);
        }

        [Fact]
        public void TestClassifyObservation() {
            var categorySet = new CategorySet<double>(Distance, Average) {
                new Category<double>(2.0),
                new Category<double>(8.0)
            };

            categorySet.Classify(3);
            Assert.Contains(3, categorySet[0]);
            Assert.DoesNotContain(3, categorySet[1]);

            categorySet.Classify(20);
            Assert.Contains(20, categorySet[1]);
            Assert.DoesNotContain(20, categorySet[0]);
        }

        [Fact]
        public void TestClassifyObservationSet() {
            var categorySet = new CategorySet<double>(Distance, Average) {
                new Category<double>(2.0),
                new Category<double>(8.0)
            };
            var observationSet = new List<double> {-1, 2, 3, 6, 8, 9};

            categorySet.Classify(observationSet);
            foreach (var num in new[] {-1, 2, 3}) {
                Assert.Contains(num, categorySet[0]);
                Assert.DoesNotContain(num, categorySet[1]);
            }
            foreach (var num in new[] {6, 8, 9}) {
                Assert.Contains(num, categorySet[1]);
                Assert.DoesNotContain(num, categorySet[0]);
            }
        }

        [Fact]
        public void TestFindNearestCategory() {
            var categorySet = new CategorySet<double>(Distance, Average) {
                new Category<double>(2.0),
                new Category<double>(8.0)
            };
            var nearest = categorySet.FindNearestCategory(3);
            Assert.Equal(nearest, categorySet[0]);

            nearest = categorySet.FindNearestCategory(19);
            Assert.Equal(nearest, categorySet[1]);
        }

        [Fact]
        public void UpdateAllCentroids() {
            var categorySet = new CategorySet<double>(Distance, Average) {
                new Category<double>(2.0) {1, 3, 4, -2},
                new Category<double>(8.0) {4, 6, 2}
            };
            categorySet.UpdateAllCentroids();

            Assert.Equal(categorySet[0].Centroid, 1.5);
            Assert.Equal(categorySet[1].Centroid, 4);
        }

        [Fact]
        public void UpdateAllCentroidsWithOffsetReturn() {
            var categorySet = new CategorySet<double>(Distance, Average) {
                new Category<double>(2.0) {1, 3, 4, -2},
                new Category<double>(8.0) {4, 6, 2}
            };

            List<double> distanceOffsets;
            categorySet.UpdateAllCentroids(out distanceOffsets);

            Assert.Equal(categorySet[0].Centroid, 1.5);
            Assert.Equal(categorySet[1].Centroid, 4);
            Assert.Equal(distanceOffsets[0], 0.5);
            Assert.Equal(distanceOffsets[1], 4.0);
        }

        [Fact]
        public void TestDistance() {
            var categorySet = new CategorySet<Point>(Point.Distance,
                set => set.Average((x, y) => x + y, (x, d) => x/d));
            var point1 = new Point(1, 2, 3);
            var point2 = new Point(-2, 3, 5);
            var distance = categorySet.Distance(point1, point2);

            Assert.Equal(distance, point1.DistanceTo(point2));
        }

        [Fact]
        public void TestOrderByCentroids() {
            var categorySet = new CategorySet<double>(Distance, Average) {
                new Category<double>(2.0),
                new Category<double>(8.0),
                new Category<double>(6.0),
                new Category<double>(-1.0)
            };
            var result = categorySet.OrderByCentroids();
            var actual = new CategorySet<double>(Distance, Average) {
                new Category<double>(-1.0),
                new Category<double>(2.0),
                new Category<double>(6.0),
                new Category<double>(8.0)
            };

            for (var i = 0; i < actual.Count; ++i)
                Assert.Equal(result[i], actual[i]);
        }

        private static double Distance(double x, double y) => Math.Abs(x - y);
        private static double Average(IEnumerable<double> enumerable) => enumerable.Average();
    }
}