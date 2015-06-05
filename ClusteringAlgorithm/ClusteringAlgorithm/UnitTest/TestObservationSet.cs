using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ClusteringAlgorithm.UnitTest {
    public class TestObservationSet {
        [Fact]
        public void TestCount() {
            var intSets = new ObservationSet<int>();
            Assert.Equal(intSets.Count, 0);

            var doubleSets = new ObservationSet<double> {1.0, 3.2, 9.6};
            Assert.Equal(doubleSets.Count, 3);
        }
        [Fact]
        public void TestAdd() {
            var intSet = new ObservationSet<int>();
            Assert.Equal(intSet.Count, 0);
            intSet.Add(3);
            Assert.Equal(intSet.Count, 1);
            Assert.Equal(intSet[0], 3);

            var doubleSet = new ObservationSet<double>();
            Assert.Equal(doubleSet.Count, 0);
            doubleSet.Add(3.5);
            Assert.Equal(doubleSet.Count, 1);
            Assert.Equal(doubleSet[0], 3.5);
        }

        [Fact]
        public void TestEnumerator() {
            var intSet1 = new ObservationSet<int> { 1, 4, 8 };
            var intSet2 = new ObservationSet<int>();
            foreach (var obs in intSet1) 
                intSet2.Add(obs);
            Assert.Equal(intSet1, intSet2);

            var doubleSet1 = new ObservationSet<int> { 1, 4, 8 };
            var doubleSet2 = new ObservationSet<int>();
            foreach (var obs in doubleSet1) 
                doubleSet2.Add(obs);
            Assert.Equal(doubleSet1, doubleSet2);
        }
    }
}