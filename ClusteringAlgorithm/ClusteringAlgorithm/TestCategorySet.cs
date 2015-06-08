using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Wfxr.Utility.Container;
using Wfxr.Utility.Data;
using Xunit;

namespace ClusteringAlgorithm {
    public class TestCategorySet {
        [Fact]
        public void TestClearAllObservations() {
            var categorySet = new CategorySet<double>(Distance, Average) {
                new Category<double>(),
                new Category<double> {2},
                new Category<double> {1, 3, 2, 4}
            };
            categorySet.ClearAllObservations();

            Assert.NotEmpty(categorySet);
            foreach (var category in categorySet)
                Assert.Empty(category);
        }

        [Fact]
        public void Test() {
            var categorySet = new CategorySet<double>(Distance, Average) {
                new Category<double> {1},
                new Category<double> {2},
                new Category<double> {2},
            };
            Assert.Equal(categorySet.Count, 2);
            Assert.False(categorySet.Add(new Category<double> {1}));
        }

        public class ListEquility : IEqualityComparer<List<double>> {
            public bool Equals(List<double> x, List<double> y) {
                return x == y;
            }
            public int GetHashCode(List<double> obj) {
                var hash = 0;
                foreach (var element in obj)
                {
                    hash ^= element.GetHashCode();
                }
                return hash;
            }
        }

        [Theory]
        [InlineData(2, 8)]
        [InlineData(-1, 7)]
        [InlineData(3, 9)]
        public void TestClassifyObservation(double value1, double value2) {
            var categoryA = new Category<double> {Centroid = 2.0};
            var categoryB = new Category<double> {Centroid = 8.0};
            var categorySet = new CategorySet<double>(Distance, Average) {categoryA, categoryB};

            categorySet.Classify(value1);
            categorySet.Classify(value2);

            // value1 should belong to categoryA, value2 should belong to categoryB
            Assert.Contains(value1, categoryA);
            Assert.Contains(value2, categoryB);
            Assert.DoesNotContain(value1, categoryB);
            Assert.DoesNotContain(value2, categoryA);
        }

        [Fact]
        public void TestClassifyObservationSet() {
            var categoryA = new Category<double> {Centroid = 2.0};
            var categoryB = new Category<double> {Centroid = 8.0};
            var categorySet = new CategorySet<double>(Distance, Average) {categoryA, categoryB};
            var observationSet = new List<double> {-1, 2, 3, 6, 8, 9};

            categorySet.Classify(observationSet);
            foreach (var value in new[] {-1, 2, 3}) {
                Assert.Contains(value, categoryA);
                Assert.DoesNotContain(value, categoryB);
            }
            foreach (var value in new[] {6, 8, 9}) {
                Assert.Contains(value, categoryB);
                Assert.DoesNotContain(value, categoryA);
            }
        }

        [Theory]
        [InlineData(2, 8)]
        [InlineData(-1, 7)]
        [InlineData(3, 9)]
        public void TestFindNearestCategory(double value1, double value2) {
            var categoryA = new Category<double> {Centroid = 2.0};
            var categoryB = new Category<double> {Centroid = 8.0};
            var categorySet = new CategorySet<double>(Distance, Average) {categoryA, categoryB};

            Assert.Equal(categorySet.FindNearestCategory(value1), categoryA);
            Assert.Equal(categorySet.FindNearestCategory(value2), categoryB);
        }

        [Fact]
        public void UpdateAllCentroids() {
            var categoryA = new Category<double>(2.0, new double[] {1, 3, 4, -2});
            var categoryB = new Category<double>(8.0, new double[] {4, 6, 2});
            var categorySet = new CategorySet<double>(Distance, Average) {categoryA, categoryB};

            categorySet.UpdateAllCentroids();

            Assert.Equal(categoryA.Centroid, 1.5);
            Assert.Equal(categoryB.Centroid, 4);
        }

        [Fact]
        public void UpdateAllCentroidsWithOffsetReturn() {
            var categoryA = new Category<double>(2.0, new double[] {1, 3, 4, -2});
            var categoryB = new Category<double>(8.0, new double[] {4, 6, 2});
            var categorySet = new CategorySet<double>(Distance, Average) {categoryA, categoryB};

            List<double> distanceOffsets;
            categorySet.UpdateAllCentroids(out distanceOffsets);

            Assert.Equal(categoryA.Centroid, 1.5);
            Assert.Equal(categoryB.Centroid, 4);
            Assert.Equal(distanceOffsets[0], 0.5);
            Assert.Equal(distanceOffsets[1], 4.0);
        }

        [Theory]
        [InlineData(0, 0, 0, 0, 0, 0)]
        [InlineData(0, 0, 0, 1, 1, 1)]
        [InlineData(1, 2, 3, -2, 3, 5)]
        public void TestDistance(double x1, double y1, double z1, double x2, double y2, double z2) {
            var categorySet = new CategorySet<Point>(Point.Distance,
                set => set.Average((x, y) => x + y, (x, d) => x/d));
            var point1 = new Point(x1, y1, z1);
            var point2 = new Point(x2, y2, z2);
            var distance = categorySet.Distance(point1, point2);
            Assert.Equal(distance, point1.DistanceTo(point2));
        }

        private static double Distance(double x, double y) => Math.Abs(x - y);
        private static double Average(IEnumerable<double> enumerable) => enumerable.Average();
    }
}