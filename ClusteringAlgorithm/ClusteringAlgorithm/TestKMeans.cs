using System;
using System.Collections.Generic;
using System.Linq;
using Wfxr.Utility.Data;
using Xunit;

namespace ClusteringAlgorithm {
    public class TestKmeans {
        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(9)]
        public void TestResultCategoryCount(int count) {
            var observationSet = new List<double> {1, 2, 3, 4, 5, 6, 7, 8, 9};
            var km = new Kmeans<double>(observationSet, Distance, Average);

            var resultCategoryCount = km.Classify(count).Count;
            Assert.Equal(resultCategoryCount, count);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(10)]
        [InlineData(20)]
        public void TestInvalidClassifyCount(int count) {
            var observationSet = new List<double> {1, 2, 3, 4, 5, 6, 7, 8, 9};
            var km = new Kmeans<double>(observationSet, Distance, Average);

            Assert.Throws<ArgumentOutOfRangeException>(() => km.Classify(count));
        }

        [Theory]
        [InlineData(new[] {0.0, 0, 0}, 1)]
        [InlineData(new[] {1.0, 2, 3}, 1)]
        [InlineData(new[] {1.0, 2, 7, 8, 9}, 2)]
        [InlineData(new[] {1.0, 2, 7, 8, 9, 21, 22}, 3)]
        public void TestClassifyResult(double[] observations, int categoryCount) {
            // K-means聚类算法可以使分类结果中每个观测值到其聚类中心的距离最近
            // 但符合此约束的分类结果可能存在多个,所以不能保证获得全局最优解
            var observationSet = new List<double>(observations);
            var km = new Kmeans<double>(observationSet, Distance, Average);

            var resultCategorySet = km.Classify(categoryCount);
            var resultObservationsCount = resultCategorySet.Sum(category => category.Count);

            // 确认所有观测值被分类,并且每个观测值距离自己所在类的中心最近
            Assert.Equal(resultObservationsCount, observations.Count());
            foreach (var thisCategory in resultCategorySet) {
                foreach (var observation in thisCategory) {
                    var nearestCategory = resultCategorySet.FindNearestCategory(observation);
                    Assert.Same(nearestCategory, thisCategory);
                }
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(7)]
        public void Test2DTuple(int categoryCount) {
            var observations = new List<Tuple<double, double>> {
                Tuple.Create(1.0, 1.0), Tuple.Create(0.0, 1.0),
                Tuple.Create(1.0, 3.0), Tuple.Create(4.0, 9.0),
                Tuple.Create(5.0, 4.0), Tuple.Create(4.0, 5.0),
                Tuple.Create(4.0, 4.0)
            };
            var km = new Kmeans<Tuple<double, double>>(observations,
                (x, y) => Math.Sqrt((x.Item1 - y.Item1)*(x.Item1 - y.Item1) +
                                    (x.Item2 - y.Item2)*(x.Item2 - y.Item2)),
                (x, y) => Tuple.Create(x.Item1 + y.Item1, x.Item2 + y.Item2),
                (x, d) => Tuple.Create(x.Item1/d, x.Item2/d));

            var resultCategorySet = km.Classify(categoryCount);
            var resultObservationsCount = resultCategorySet.Sum(category => category.Count);

            Assert.Equal(resultObservationsCount, observations.Count());

            foreach (var thisCategory in resultCategorySet) {
                foreach (var observation in thisCategory) {
                    var nearestCategory = resultCategorySet.FindNearestCategory(observation);
                    Assert.Same(nearestCategory, thisCategory);
                }
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(7)]
        public void Test3DPoint(int categoryCount) {
            var observations = new List<Point> {
                new Point(1.0, 1.0, 1.0),
                new Point(0.0, 1.0, 1.0),
                new Point(1.0, 1.0, 0.0),
                new Point(4.0, 5.0, 5.0),
                new Point(5.0, 4.0, 4.0),
                new Point(4.0, 5.0, 4.0),
                new Point(4.0, 4.0, 4.0)
            };
            var km = new Kmeans<Point>(observations, Point.Distance, (x, y) => x + y,
                (x, d) => x/d);

            var resultCategorySet = km.Classify(categoryCount);
            var resultObservationsCount = resultCategorySet.Sum(category => category.Count);

            Assert.Equal(resultObservationsCount, observations.Count());

            foreach (var thisCategory in resultCategorySet) {
                foreach (var observation in thisCategory) {
                    var nearestCategory = resultCategorySet.FindNearestCategory(observation);
                    Assert.Same(nearestCategory, thisCategory);
                }
            }
        }

        private static double Average(List<double> set) => set.Average();
        private static double Distance(double x, double y) => Math.Abs(x - y);
    }
}