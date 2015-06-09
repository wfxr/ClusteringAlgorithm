using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using Wfxr.Statistics;
// ReSharper disable InconsistentNaming

namespace ClusteringAlgorithm {
    public class Fcm {
        private static MatrixBuilder<double> MatrixBuilder = Matrix<double>.Build;
        private static VectorBuilder<double> VectorBuilder = Vector<double>.Build;
        private double _m = 2;
        public double m {
            get { return _m; }
            set {
                if (m < 1)
                    throw new ArgumentOutOfRangeException($"m:{value}");
                _m = value;
            }
        }

        private Matrix<double> X { get; }

        public Fcm(Matrix<double> X) {
            this.X = X;
        }

        /// <summary>
        ///     观测值的数目
        /// </summary>
        public int N => X.ColumnCount;
        /// <summary>
        ///     观测值的维度
        /// </summary>
        public int D => X.RowCount;

        /// <summary>
        ///     对观测值集合进行聚类划分
        /// </summary>
        /// <param name="K">聚类数目</param>
        /// <param name="precision">迭代精度</param>
        /// <returns>观察值的聚类集合</returns>
        public void Cluster(int K, double precision = 0.01) {
            ValidateArgument(K, precision);
            // 创建隶属度矩阵并完成行标准化,行数为分类数目,列数为观察值数目
            var U = MatrixBuilder.Random(K, N).NormalizeRows(1.0);

            var Um = U.PointwisePower(m);

            // 创建中心点矩阵，
            var C = MatrixBuilder.Dense(D, K);
            for (var i = 0; i < K; ++i) {
                var row = Um.Row(i);
                var X1 = X;
                var denominator = row.Sum();
                for (var j = 0; j < N; ++j)
                    X1.Column(j).Multiply(Um[i, j]);
                var numerator = X1.ColumnSums();
                //C.Add(numerator/denominator);
            }

            //// 距离矩阵
            //var D = MatrixBuilder<double>.Build.Dense(K, N);
            //for (var i = 0; i < K; ++i) {
            //    for (var j = 0; j < N; ++j)
            //        D[i,j] = 
            //}
        }

        /// <summary>
        ///     验证参数的合理性
        /// </summary>
        /// <param name="K">聚类数目</param>
        /// <param name="precision">迭代精度</param>
        private void ValidateArgument(int K, double precision) {
            if (K > N || K < 1)
                throw new ArgumentOutOfRangeException(
                    $"categories number({K}) \">\" distinct observations number({N})");
            if (precision <= 0)
                throw new ArgumentOutOfRangeException($"Invalid {nameof(precision)}: {precision}");
        }
    }
}