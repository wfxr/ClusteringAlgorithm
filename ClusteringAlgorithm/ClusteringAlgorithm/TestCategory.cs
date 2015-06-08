using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Wfxr.Utility.Container;
using Wfxr.Utility.Data;
using Xunit;

namespace ClusteringAlgorithm {
    public class TestCategory {
        public static IEnumerable<object[]> DoublesGroups => new[] {
            new object[] {new[] {0.0, 0.0}, 0.0},
            new object[] {new[] {-1.0, 0.0, 3.0}, 2.0/3},
            new object[] {new[] {3.6, 8.1, -9.0, 0.9}, 0.9}
        };

        [Theory]
        [MemberData(nameof(DoublesGroups))]
        public void TestUpdateCentroidOfDoubleCategory(double[] numbers, double centroid) {
            var category = new Category<double>(numbers);
            category.UpdateCentroid(set => set.Average());

            Assert.Equal(category.Centroid, centroid, 12);
        }

        [Fact]
        public void TestUpdateCentroidOfPointCategory() {
            var category = new Category<Point> {
                new Point(1, 1, 1),
                new Point(-1, 0, 1),
                new Point(3, 2, 1)
            };
            category.UpdateCentroid(set => set.Average((p1, p2) => p1 + p2, (point, d) => point/d));

            Assert.Equal(category.Centroid, new Point(1, 1, 1));
        }

        [Theory]
        [InlineData(new double[] {}, new double[] {})]
        [InlineData(new double[] {1}, new double[] {1})]
        [InlineData(new double[] {1, 3, 5}, new double[] {1, 3, 5})]
        [SuppressMessage("ReSharper", "EqualExpressionComparison")]
        public void TestEquals(double[] x, double[] y) {
            var catX = new Category<double>(x);
            var catY = new Category<double>(y);
            object objX = catX;
            object objY = catY;

            Assert.True(catX.Equals(catX));
            Assert.True(catX.Equals(catY));

            Assert.True(objX.Equals(objX));
            Assert.True(objX.Equals(objY));
        }

        [Theory]
        [InlineData(new double[] {}, null)]
        [InlineData(new double[] {}, new double[] {1})]
        [InlineData(new double[] {1}, new double[] {})]
        [InlineData(new double[] {1, 3, 5}, new double[] {1, 3})]
        public void TestNotEqual(double[] x, double[] y) {
            var catX = new Category<double>(x);
            var catY = y == null ? null : new Category<double>(y);
            object objX = catX;
            object objY = catY;

            Assert.False(catX.Equals(catY));
            Assert.False(objX.Equals(objY));
        }
    }
}