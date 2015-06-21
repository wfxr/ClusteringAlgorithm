using System.Linq;
using ClusteringAlgorithm;
using MathNet.Numerics.LinearAlgebra.Double;
using Xunit;

namespace UnitTest {
    public class TestFcm {
        [Theory]
        [InlineData(2)]
        [InlineData(4)]
        public void TestResultCategoryCount(int k) {
            var data = DenseMatrix.OfArray(new double[,] {
                {1, 2, 3, 7, 8, 9, 13, 14, 15, 100, 120, 130}
            }).Transpose();
            var km = new Fcm(data);
            var result = km.Run(k);
            Assert.Equal(result.Center.RowCount, k);
            Assert.Equal(result.Idx.Count(), data.RowCount);
         }
    }
}