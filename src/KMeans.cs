using System;
using System.Collections.Generic;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using Wfxr.Statistics;
// ReSharper disable InconsistentNaming

namespace ClusteringAlgorithm {
    public class Kmeans : Clustering {
        public Kmeans(Matrix<double> data) : base(data) { }

        /// <summary>
        ///     使用K均值算法对数据集进行聚类
        /// </summary>
        /// <param name="c">聚类数目</param>
        /// <param name="max_iter">最大迭代次数</param>
        /// <param name="min_impro">最小改进量</param>
        /// <returns>包含聚类中心,隶属向量和目标函数向量的数据结构</returns>
        public ClusterResult Clustering(int c, int max_iter = 100, double min_impro = 1e-5) {
            ValidateArgument(c, max_iter, min_impro);

            // 创建中心矩阵并初始化
            var C = RandomCenter(c);

            // 创建距离矩阵
            var dist = MatrixBuilder.Dense(c, n);

            // 创建隶属度矩阵
            var U = MatrixBuilder.Dense(c, n);

            // 创建目标函数向量
            var obj_fcn = VectorBuilder.Dense(max_iter);

            // 主循环
            for (var i = 0; i < max_iter; ++i) {
                // 更新距离矩阵
                dist = ComputeDistance(C);

                // 更新隶属度矩阵
                U = ComputeU(dist);

                // 保存原来的中心
                var oldC = C;

                // 更新中心矩阵和价值函数
                C = ComputeCenter(U);

                // 添加目标函数值
                obj_fcn[i] = ComputeObjectFunction(oldC, C);

                // 改进程度小于指定值则结束循环
                if (i > 1 && Math.Abs(obj_fcn[i] - obj_fcn[i - 1]) < min_impro) break;
            }


            // 创建聚类列表
            var clusters = ComputeCluster(dist);

            return new ClusterResult(C, U, clusters, obj_fcn);
        }

        /// <summary>
        ///     计算目标函数值
        /// </summary>
        /// <param name="oldC"></param>
        /// <param name="newC"></param>
        /// <returns></returns>
        private double ComputeObjectFunction(Matrix<double> oldC, Matrix<double> newC) {
            var c = newC.RowCount;
            var dis = VectorBuilder.Dense(c);
            for (var i = 0; i < c; ++i) {
                dis[i] = Distance.Euclidean(oldC.Row(i), newC.Row(i));
            }
            return dis.Sum();
        }

        /// <summary>
        ///     随机选取观测值作为聚类中心
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private Matrix<double> RandomCenter(int c) {
            var C = MatrixBuilder.Dense(c, d);
            var rands = Sampling.SampleFromRange(0, n, c); // 从0到n中随机无重复地抽取出c个索引值
            for (var i = 0; i < c; ++i) {
                var irand = rands[i];
                C.SetRow(i, data.Row(irand));
            }
            return C;
        }

        /// <summary>
        ///     计算每个观测值到各聚类中心的距离
        /// </summary>
        /// <param name="C">聚类中心矩阵</param>
        /// <returns>距离矩阵</returns>
        private Matrix<double> ComputeDistance(Matrix<double> C) {
            var dist = MatrixBuilder.Dense(C.RowCount, n);
            for (var i = 0; i < C.RowCount; ++i)
                for (var j = 0; j < n; ++j)
                    dist[i, j] = Distance.Euclidean(data.Row(j), C.Row(i));
            return dist;
        }

        /// <summary>
        ///     计算隶属矩阵
        /// </summary>
        /// <param name="dist">距离矩阵</param>
        /// <returns>隶属矩阵</returns>
        private Matrix<double> ComputeU(Matrix<double> dist) {
            var U = MatrixBuilder.Dense(dist.RowCount, n, 0);
            for (var i = 0; i < n; ++i) 
                U[dist.Column(i).MinimumIndex(), i] = 1;
            return U;
        }
    }
}