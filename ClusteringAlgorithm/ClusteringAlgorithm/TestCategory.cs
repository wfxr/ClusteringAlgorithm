using System.Linq;
using Wfxr.Utility.Container;
using Wfxr.Utility.Data;
using Xunit;

namespace ClusteringAlgorithm {
    public class TestCategory {
        [Fact]
        public void TestSetCentroidOfDoubleCategory() {
            var category = new Category<double>();
            category.SetCentroid(8.8);

            Assert.Equal(category.Centroid, 8.8);
        }

        [Fact]
        public void TestSetCentroidOfPointCategory() {
            var category = new Category<Point>();
            category.SetCentroid(new Point(-1, 0, 1));

            Assert.Equal(category.Centroid, new Point(-1, 0, 1));
        }

        [Fact]
        public void TestUpdateCentroidOfDoubleCategory() {
            var category = new Category<double> {1, 2, 3, 4};
            category.UpdateCentroid(set => set.Average());

            Assert.Equal(category.Centroid, 2.5);
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
        public void TestEquality() {
            var category1 = new Category<double> {1, 2};
            var categoryEqual = new Category<double> {1, 2};
            var category3 = new Category<double> {0, 2};
            var category4 = new Category<double> {1};
            var categoryEmpty = new Category<double>();
            Category<double> categoryNull = null;

            Assert.Equal(category1, category1);
            Assert.Equal(category1, categoryEqual);
            Assert.NotEqual(category1, category3);
            Assert.NotEqual(category1, category4);
            Assert.NotEqual(category1, categoryEmpty);
            Assert.NotEqual(category1, categoryNull);
        }
    }
}