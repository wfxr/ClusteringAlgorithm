﻿using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;
using Xunit;

namespace ClusteringAlgorithm {
    public class TestFcm {
        [Theory]
        [InlineData(2)]
        [InlineData(4)]
        public void TestResultCategoryCount(int k) {
            var data = DenseMatrix.OfArray(new double[,] {
                {1, 2, 3, 7, 8, 9, 13, 14, 15, 100, 120, 130}
            }).Transpose();
            var km = new Kmeans(data);
            var result = km.Clustering(k);
            Assert.Equal(result.Center.RowCount, k);
            Assert.Equal(result.U.Count(), data.RowCount);
         }
    }
}