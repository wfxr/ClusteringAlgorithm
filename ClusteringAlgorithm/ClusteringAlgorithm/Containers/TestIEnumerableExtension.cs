using System.Linq;
using ClusteringAlgorithm.ObservationTypes;
using Xunit;

namespace ClusteringAlgorithm.Containers
{
    public class TestIEnumerableExtension
    {
        [Fact]
        public void TestAverageDouble() {
            var set = new Set<double> { 1.2, 3.6, 7.2 };
            Assert.Equal(4.0, set.Average((x, y) => x + y, (x, y) => x / y));
        }

        public void TestAveragePoint() {
            var set = new Set<Point> {
                new Point(1, 2, 1),
                new Point(2, 5, 3),
                new Point(0, 2, 2)
            };
            Assert.Equal(new Point(1, 3, 2), set.Average((x, y) => x + y, (x, y) => x / y));
        }
    }
}
