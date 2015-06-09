using MathNet.Numerics.LinearAlgebra;
using Xunit;

namespace ClusteringAlgorithm {
    public class TestFcm {
        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(3)]
        [InlineData(3)]
        [InlineData(9)]
        public void TestResultCategoryCount(int c) {
            var observationSet =
                Matrix<double>.Build.DenseOfArray(new double[,] {
                    {1, 2, 3, 7, 8, 9, 13, 14, 15}
                });
            var fcm = new Fcm(observationSet.Transpose());
            var result = fcm.Cluster(c);
        }
    }
}