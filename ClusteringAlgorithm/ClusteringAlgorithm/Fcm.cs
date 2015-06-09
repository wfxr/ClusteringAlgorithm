using System;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;

// ReSharper disable InconsistentNaming

namespace ClusteringAlgorithm {
    public class Fcm {
        private static readonly MatrixBuilder<double> MatrixBuilder = Matrix<double>.Build;
        private static readonly VectorBuilder<double> VectorBuilder = Vector<double>.Build;

        private Matrix<double> data { get; }

        public Fcm(Matrix<double> data) { this.data = data; }

        /// <summary>
        ///     观测值的数目
        /// </summary>
        public int n => data.RowCount;

        /// <summary>
        ///     观测值的维数
        /// </summary>
        public int d => data.ColumnCount;

        /// <summary>
        ///     Data set clustering using fuzzy c-means clustering.
        /// </summary>
        /// <param name="c">number of clusters</param>
        /// <param name="expo">exponent for the matrix U</param>
        /// <param name="max_iter">maximum number of iterations</param>
        /// <param name="min_impro">minimum amount of improvement</param>
        /// <returns></returns>
        public FcmResult Cluster(int c, double expo = 2.0, int max_iter = 100,
            double min_impro = 1e-5) {
            ValidateArgument(c, expo, max_iter, min_impro);

            // 创建隶属度矩阵并完成行标准化,行数为分类数目,列数为观察值数目
            var U = MatrixBuilder.Random(c, n).NormalizeRows(1.0);

            // 创建中心矩阵,行数为分类数目,列数为观察值维数
            var C = MatrixBuilder.Dense(c, d);

            // 目标函数向量
            var obj_fcn = VectorBuilder.Dense(max_iter);

            // 主循环
            for (var i = 0; i < max_iter; ++i) {
                obj_fcn[i] = SetpFcm(ref U, out C, c, expo);

                // 改进程度小于指定值则结束循环
                if (i > 1 && Math.Abs(obj_fcn[i] - obj_fcn[i - 1]) < min_impro) break;
            }
            return new FcmResult(C, U, obj_fcn);
        }

        private double SetpFcm(ref Matrix<double> U, out Matrix<double> C, int c, double expo) {
            var mf = U.Clone().PointwisePower(expo);

            C = (mf*data).PointwiseDivide(mf.RowSums().ToColumnMatrix()*MatrixBuilder.Dense(1, d, 1));

            var Dis = MatrixBuilder.Dense(c, n);
            for (var i = 0; i < c; ++i)
                for (var j = 0; j < n; ++j)
                    Dis[i, j] = Distance.Euclidean(data.Row(j), C.Row(i));

            // 计算目标函数值
            var obj_fcn = Dis.PointwisePower(2).PointwiseMultiply(mf).RowSums().Sum();

            // 更新隶属度矩阵
            var tmp = Dis.PointwisePower(-2/(expo - 1));
            U = tmp.PointwiseDivide(MatrixBuilder.Dense(c, 1, 1)*tmp.ColumnSums().ToRowMatrix());

            return obj_fcn;
        }


        private void ValidateArgument(int c, double expo, int max_iter, double min_impro) {
            if (c > n)
                throw new ArgumentException(
                    "The clusters number should not be greater than observations number!");
            if (c < 2)
                throw new ArgumentException("The clusters number should be at least 2!");
            if (expo <= 1.0)
                throw new ArgumentException("The exponent should be greater than 1!");
            if (max_iter < 1)
                throw new ArgumentException("The maximum iterations should be at least 1!");
            if (min_impro < 0.0)
                throw new ArgumentException("minimum amount of improvement should not be negative!");
        }

        /// <summary>
        ///     用以存储FCM聚类结果的数据结构
        /// </summary>
        public class FcmResult {
            public FcmResult(Matrix<double> center, Matrix<double> u, Vector<double> obj_fcn) {
                Center = center;
                U = u;
                ObjectFunction = obj_fcn;
            }

            /// <summary>
            ///     聚类中心
            /// </summary>
            public Matrix<double> Center;

            /// <summary>
            ///     隶属度矩阵
            /// </summary>
            public Matrix<double> U;

            /// <summary>
            ///     目标函数向量
            /// </summary>
            public Vector<double> ObjectFunction;
        }
    }
}