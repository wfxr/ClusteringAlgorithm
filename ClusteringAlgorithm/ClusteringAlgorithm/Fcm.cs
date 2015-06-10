﻿using System;
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

            // 创建隶属度矩阵并执行行标准化
            var U = MatrixBuilder.Random(c, n).NormalizeRows(1.0);

            // 创建中心矩阵
            var C = MatrixBuilder.Dense(c, d);

            // 创建目标函数向量
            var obj_fcn = VectorBuilder.Dense(max_iter);

            // 主循环
            for (var i = 0; i < max_iter; ++i) {
                // 隶属度指数修正矩阵
                var Um = ComputeUm(U, expo);

                // 更新中心矩阵
                C = ComputeCenter(Um);

                // 距离矩阵
                var dist = ComputeDistance(C);

                // 计算目标函数值
                obj_fcn[i] = ComputeObjectFunctionn(dist, Um);

                // 更新隶属度矩阵
                U = ComputeU(dist, expo);

                // 改进程度小于指定值则结束循环
                if (i > 1 && Math.Abs(obj_fcn[i] - obj_fcn[i - 1]) < min_impro) break;
            }
            return new FcmResult(C, U, obj_fcn);
        }

        /// <summary>
        ///     计算对隶属度矩阵进行指数修正后的矩阵
        /// </summary>
        /// <param name="U">隶属度矩阵</param>
        /// <param name="expo">指数</param>
        /// <returns>修正后的矩阵</returns>
        private Matrix<double> ComputeUm(Matrix<double> U, double expo) => U.PointwisePower(expo);

        /// <summary>
        ///     计算分类中心
        /// </summary>
        /// <param name="mf">隶属度修正矩阵</param>
        /// <returns>中心矩阵</returns>
        private Matrix<double> ComputeCenter(Matrix<double> mf)
            => (mf*data).PointwiseDivide(mf.RowSums().ToColumnMatrix()*MatrixBuilder.Dense(1, d, 1));

        /// <summary>
        ///     计算每个观测值到各分类中心的距离
        /// </summary>
        /// <param name="C">分类中心矩阵</param>
        /// <returns>距离矩阵</returns>
        private Matrix<double> ComputeDistance(Matrix<double> C) {
            var dist = MatrixBuilder.Dense(C.RowCount, n);
            for (var i = 0; i < C.RowCount; ++i)
                for (var j = 0; j < n; ++j)
                    dist[i, j] = Distance.Euclidean(data.Row(j), C.Row(i));
            return dist;
        }

        /// <summary>
        ///     计算目标函数值
        /// </summary>
        /// <param name="dist">距离矩阵</param>
        /// <param name="mf">隶属度修正矩阵</param>
        /// <returns>目标函数值</returns>
        private double ComputeObjectFunctionn(Matrix<double> dist, Matrix<double> mf)
            => dist.PointwisePower(2).PointwiseMultiply(mf).RowSums().Sum();

        /// <summary>
        ///     计算隶属度
        /// </summary>
        /// <param name="dist">距离矩阵</param>
        /// <param name="expo">指数</param>
        /// <returns>隶属度矩阵</returns>
        private Matrix<double> ComputeU(Matrix<double> dist, double expo) {
            var tmp = dist.PointwisePower(-2/(expo - 1));
            return
                tmp.PointwiseDivide(MatrixBuilder.Dense(dist.RowCount, 1, 1)*
                                    tmp.ColumnSums().ToRowMatrix());
        }

        /// <summary>
        ///     验证参数是否正确
        /// </summary>
        /// <param name="c">分类数目</param>
        /// <param name="expo">指数</param>
        /// <param name="max_iter">结束条件:最大迭代次数</param>
        /// <param name="min_impro">结束条件:目标函数最小改进值</param>
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