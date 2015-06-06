using System.Linq;
using Container;
using Data;
using Xunit;

namespace ClusteringAlgorithm.Algorithms {
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
    }
}