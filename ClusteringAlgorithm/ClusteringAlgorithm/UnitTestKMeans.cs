using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ClusteringAlgorithm
{
    public class UnitTestKMeans {
        private double Distance(double obs1, double obs2) => Math.Abs(obs1 - obs2);
        private double Average(ObservationSet<double> observationSet) => observationSet.Average();
        [Fact]
        public void TestCategoriesCount()
        {
            ObservationSet<double> observationSet = new ObservationSet<double> {1, 2, 3, 4, 5, 6, 7, 8, 9};
            var km = new KMeans<double>(observationSet, Distance, Average);

            Assert.Equal(km.Classify(1).Count, 1);
            Assert.Equal(km.Classify(3).Count, 3);
            Assert.Equal(km.Classify(6).Count, 6);
        }

        [Fact]
        public void TestInvalidCategroiesCount()
        {
            ObservationSet<double> observationSet = new ObservationSet<double> {1, 2, 3, 7, 8, 9};
            var km = new KMeans<double>(observationSet, Distance, Average);

            Assert.Throws<ArgumentOutOfRangeException>(() => km.Classify(0));
            Assert.Throws<ArgumentOutOfRangeException>(() => km.Classify(7));
        }
        [Fact]
        public void TestCentroiesOfResults()
        {
            ObservationSet<double> observationSet = new ObservationSet<double> {1, 2, 3, 7, 8, 9};
            var km = new KMeans<double>(observationSet, Distance, Average);

            var categories = km.Classify(2);

            Assert.Equal(categories[0].Centroid, 2.0);
            Assert.Equal(categories[1].Centroid, 8.0);
        }
        [Fact]
        public void TestSetsOfResults()
        {
            ObservationSet<double> observationSet = new ObservationSet<double> {1, 2, 3, 7, 8, 9};
            var km = new KMeans<double>(observationSet, Distance, Average);

            var categories = km.Classify(2);

            Assert.Equal(categories[0].Observations, new ObservationSet<double> { 1, 2, 3 });
            Assert.Equal(categories[1].Observations, new ObservationSet<double> { 7, 8, 9 });
        }
    }
}
