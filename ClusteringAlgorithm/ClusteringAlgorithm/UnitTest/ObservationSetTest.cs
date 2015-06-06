using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ClusteringAlgorithm.UnitTest {
    public class ObservationSetTest {
        [Fact]
        public void TestCount() {
            var sets = new ObservationSet<int>();
            Assert.Equal(sets.Count, 0);

            sets = new ObservationSet<int> {1, 3, 8};
            Assert.Equal(sets.Count, 3);
        }

        [Fact]
        public void TestIntObservations() {
            var sets = new ObservationSet<int> {1, 3, 8};
            Assert.Equal(sets.Count, 3);
            Assert.Equal(sets[0], 1);
            Assert.Equal(sets[1], 3);
            Assert.Equal(sets[2], 8);
        }

        [Fact]
        public void TestDoubleObservations() {
            var sets = new ObservationSet<double> {2.4, -3.1, 8.8};
            Assert.Equal(sets.Count, 3);
            Assert.Equal(sets[0], 2.4);
            Assert.Equal(sets[1], -3.1);
            Assert.Equal(sets[2], 8.8);
        }

        [Fact]
        public void TestTupleObservations() {
            var sets = new ObservationSet<Tuple<int, double>> {
                Tuple.Create(2000, 3.2),
                Tuple.Create(2001, 4.5),
                Tuple.Create(2002, 9.6)
            };
            Assert.Equal(sets.Count, 3);
            Assert.Equal(sets[0], Tuple.Create(2000, 3.2));
            Assert.Equal(sets[1], Tuple.Create(2001, 4.5));
            Assert.Equal(sets[2], Tuple.Create(2002, 9.6));
        }

        [Fact]
        public void TestPointObservations() {
            var sets = new ObservationSet<Point> {
                new Point(1, 2, 3),
                new Point(4, 5, 6),
                new Point(7, 8, 9)
            };
            Assert.Equal(sets.Count, 3);
            Assert.Equal(sets[0], new Point(1, 2, 3));
            Assert.Equal(sets[1], new Point(4, 5, 6));
            Assert.Equal(sets[2], new Point(7, 8, 9));
        }

        [Fact]
        public void TestCreateSetFromList() {
            var sets = new ObservationSet<int>(new List<int> {1, 3, 8});

            Assert.Equal(sets.Count, 3);
            Assert.Equal(sets[0], 1);
            Assert.Equal(sets[1], 3);
            Assert.Equal(sets[2], 8);
        }

        [Fact]
        public void TestAdd() {
            var intSet = new ObservationSet<int>();
            Assert.Equal(intSet.Count, 0);
            intSet.Add(3);
            Assert.Equal(intSet.Count, 1);
            Assert.Equal(intSet[0], 3);
        }

        [Fact]
        public void TestEnumerator() {
            var set1 = new ObservationSet<int> {1, 4, 8};
            var set2 = new ObservationSet<int>();
            foreach (var obs in set1)
                set2.Add(obs);
            Assert.Equal(set1, set2);
        }

        [Fact]
        public void TestDistinct() {
            var set = new ObservationSet<int> {1, 4, 8, 4, 9, 1};
            Assert.Equal(set.Distinct(), new ObservationSet<int> { 1, 4, 8, 9 });
        }

    }
}