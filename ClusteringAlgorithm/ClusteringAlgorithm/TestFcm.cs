using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;
using Xunit;

namespace ClusteringAlgorithm {
    public class TestFcm {
        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(9)]
        public void TestResultCategoryCount(int K) {
            var observationSet =
                Matrix<double>.Build.DenseOfArray(new double[,] {
                    {1, 2, 3, 4, 5, 6, 7, 8, 9}
                });
            var fcm = new Fcm(observationSet);
            fcm.Cluster(K);
        }

        private static double Average(ICollection<double> set) => set.Average();
        private static double Distance(double x, double y) => Math.Abs(x - y);
    }
}