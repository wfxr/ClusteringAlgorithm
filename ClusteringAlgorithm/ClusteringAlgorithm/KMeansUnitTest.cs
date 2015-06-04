﻿using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ClusteringAlgorithm
{
    public class KMeansUnitTest
    {
        [Fact]
        public void TestCategoriesCount()
        {
            IEnumerable<IntObervation> samples = new List<IntObervation> {1, 2, 3, 7, 8, 9};
            var km = new KMeans<int>(samples);

            Assert.Equal(km.Classify(1).Count, 1);
            Assert.Equal(km.Classify(3).Count, 3);
            Assert.Equal(km.Classify(6).Count, 6);
        }

        [Fact]
        public void TestInvalidCategroiesCount()
        {
            IEnumerable<IntObervation> samples = new List<IntObervation> {1, 2, 3, 7, 8, 9};
            var km = new KMeans<int>(samples);

            Assert.Throws<ArgumentOutOfRangeException>(() => km.Classify(0));
            Assert.Throws<ArgumentOutOfRangeException>(() => km.Classify(7));
        }
        [Fact]
        public void TestCentroiesOfResults()
        {
            IEnumerable<IntObervation> samples = new List<IntObervation> {1, 2, 3, 7, 8, 9};
            var km = new KMeans<int>(samples);

            var categories = km.Classify(2);

            Assert.Equal(categories[0].Centroid, 2.0);
            Assert.Equal(categories[1].Centroid, 8.0);
        }
        [Fact]
        public void TestSetsOfResults()
        {
            IEnumerable<IntObervation> samples = new List<IntObervation> {1, 2, 3, 7, 8, 9};
            var km = new KMeans<int>(samples);

            var categories = km.Classify(2);

            Assert.Equal(categories[0].Observations, new List<IntObervation> { 1, 2, 3 });
            Assert.Equal(categories[1].Observations, new List<IntObervation> { 7, 8, 9 });
        }
    }
}