using System;
using System.Collections.Generic;
using ClusteringAlgorithm.ObservationTypes;
using Xunit;

namespace ClusteringAlgorithm.Containers {
    public class TestSet {
        [Fact]
        public void TestCount() {
            var sets = new Set<int>();
            Assert.Equal(sets.Count, 0);

            sets = new Set<int> {1, 3, 8};
            Assert.Equal(sets.Count, 3);
        }

        [Fact]
        public void TestIntSet() {
            var sets = new Set<int> {1, 3, 8};
            Assert.Equal(sets.Count, 3);
            Assert.Equal(sets[0], 1);
            Assert.Equal(sets[1], 3);
            Assert.Equal(sets[2], 8);
        }

        [Fact]
        public void TestDoubleSet() {
            var sets = new Set<double> {2.4, -3.1, 8.8};
            Assert.Equal(sets.Count, 3);
            Assert.Equal(sets[0], 2.4);
            Assert.Equal(sets[1], -3.1);
            Assert.Equal(sets[2], 8.8);
        }

        [Fact]
        public void TestTupleSet() {
            var sets = new Set<Tuple<int, double>> {
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
        public void TestPointSet() {
            var sets = new Set<Point> {
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
            var sets = new Set<int>(new List<int> {1, 3, 8});

            Assert.Equal(sets.Count, 3);
            Assert.Equal(sets[0], 1);
            Assert.Equal(sets[1], 3);
            Assert.Equal(sets[2], 8);
        }

        [Fact]
        public void TestAdd() {
            var intSet = new Set<int>();
            Assert.Equal(intSet.Count, 0);
            intSet.Add(3);
            Assert.Equal(intSet.Count, 1);
            Assert.Equal(intSet[0], 3);
        }

        [Fact]
        public void TestEnumerator() {
            var set1 = new Set<int> {1, 4, 8};
            var set2 = new Set<int>();
            foreach (var obs in set1)
                set2.Add(obs);
            Assert.Equal(set1, set2);
        }

        [Fact]
        public void TestDistinct() {
            var set = new Set<int> {1, 4, 8, 4, 9, 1};
            Assert.Equal(set.Distinct(), new Set<int> { 1, 4, 8, 9 });
        }

    }
}