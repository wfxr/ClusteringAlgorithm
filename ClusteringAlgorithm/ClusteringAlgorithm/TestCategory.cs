using System.Collections.Generic;
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

        [Fact]
        public void TestEqual() {
            var category = new Category<double> {1, 2};
            var categoryEqual = new Category<double> {1, 2};

            Assert.Equal(category, category);
            Assert.Equal(category, categoryEqual);
        }

        [Fact]
        public void TestNotEqual() {
            var category = new Category<double> {1, 2};

            var category1 = new Category<double> {0, 2};
            var category2 = new Category<double> {1};

            Assert.NotEqual(category, category1);
            Assert.NotEqual(category, category2);

            var categoryEmpty = new Category<double>();
            Category<double> categoryNull = null;

            Assert.NotEqual(category, categoryEmpty);
            Assert.NotEqual(category, categoryNull);
        }
    }
}