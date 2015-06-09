using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using Wfxr.Statistics;

// ReSharper disable InconsistentNaming

namespace ClusteringAlgorithm {
    public class Fcm {
        private static readonly MatrixBuilder<double> MatrixBuilder = Matrix<double>.Build;
        private static readonly VectorBuilder<double> VectorBuilder = Vector<double>.Build;
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

        public Fcm(Matrix<double> X) { this.X = X; }

        /// <summary>
        ///     观测值的数目
        /// </summary>
        public int n => X.ColumnCount;

        /// <summary>
        ///     观测值的维数
        /// </summary>
        public int D => X.RowCount;

        /// <summary>
        ///     对观测值集合进行聚类划分
        /// </summary>
        /// <param name="c">聚类数目</param>
        /// <param name="precision">迭代精度</param>
        /// <returns>观察值的聚类集合</returns>
        public void Cluster(int c, double precision = 0.01) {
            ValidateArgument(c, precision);
            // 创建隶属度矩阵并完成行标准化,行数为分类数目,列数为观察值数目
            var U = MatrixBuilder.Random(c, n).NormalizeRows(1.0);


            // 创建中心点矩阵,行数为观察值维数,列数为分类数目
            var C = MatrixBuilder.Dense(D, c);

            // 距离矩阵,行数为分类数目,列数为观察值数目
            var Dis = MatrixBuilder.Dense(c, n);

            // TODO:添加迭代结束条件
            var times = 10;
            while (times-- > 0) {
                // 更新中心点矩阵
                var Um = U.PointwisePower(m);
                for (var i = 0; i < c; ++i) {
                    var row = Um.Row(i);
                    var X1 = X.Clone();
                    var denominator = row.Sum();
                    for (var j = 0; j < n; ++j)
                        X1.SetColumn(j, X1.Column(j).Multiply(Um[i, j]));
                    var numerator = X1.RowSums();
                    C.SetColumn(i, numerator.Divide(denominator));
                }


                // 更新距离矩阵
                for (var i = 0; i < c; ++i)
                    for (var j = 0; j < n; ++j)
                        Dis[i, j] = Distance.Euclidean(X.Column(j), C.Column(i));

                // 更新隶属度矩阵
                for (var i = 0; i < c; ++i)
                    for (var j = 0; j < n; ++j) {
                        var sum = 0.0;
                        for (var k = 0; k < c; ++k)
                            sum += Math.Pow((Dis[i, j]/Dis[k, j]), 2/(m - 1));
                        U[i, j] = 1/sum;
                    }
            }
        }

        /// <summary>
        ///     验证参数的合理性
        /// </summary>
        /// <param name="K">聚类数目</param>
        /// <param name="precision">迭代精度</param>
        private void ValidateArgument(int K, double precision) {
            if (K > n || K < 1)
                throw new ArgumentOutOfRangeException(
                    $"categories number({K}) \">\" distinct observations number({n})");
            if (precision <= 0)
                throw new ArgumentOutOfRangeException($"Invalid {nameof(precision)}: {precision}");
        }
    }
}