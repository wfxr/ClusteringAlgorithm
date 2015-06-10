using System;
using System.Collections.Generic;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using Wfxr.Statistics;
using Wfxr.Utility.Container;

// ReSharper disable InconsistentNaming

namespace ClusteringAlgorithm {
    using Group = List<Vector<double>>;
    using GroupList = List<List<Vector<double>>>;

    public class Kmeans {
        private static readonly MatrixBuilder<double> MatrixBuilder = Matrix<double>.Build;
        private static readonly VectorBuilder<double> VectorBuilder = Vector<double>.Build;
        public Kmeans(Matrix<double> data) { this.data = data; }
        private Matrix<double> data { get; }

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
        /// <param name="max_iter">maximum number of iterations</param>
        /// <param name="min_impro">minimum amount of improvement</param>
        /// <returns></returns>
        public void Cluster(int c, int max_iter = 100, double min_impro = 1e-5) {
            ValidateArgument(c, max_iter, min_impro);

            // 创建中心矩阵并初始化
            var C = RandomCenter(c);

            // 创建目标函数向量
            var obj_fcn = VectorBuilder.Dense(max_iter);

            // 主循环
            for (var i = 0; i < max_iter; ++i) {
                // 计算距离矩阵
                var dist = ComputeDistance(C);

                // 计算隶属向量
                var U = ComputeU(dist);

                // 计算观测值分组
                var groups = ComputeGroup(U, c);
                
                // 保存原来的中心
                var oldC = C;

                // 更新中心矩阵和价值函数
                C = ComputeCenter(groups);

                // 计算目标函数
                obj_fcn[i] = ComputeObjectFunction(oldC,C);

                // 改进程度小于指定值则结束循环
                if (i > 1 && Math.Abs(obj_fcn[i] - obj_fcn[i - 1]) < min_impro) break;
            }
        }

        private double ComputeObjectFunction(Matrix<double> oldC, Matrix<double> newC) {
            var c = newC.RowCount;
            var dis = VectorBuilder.Dense(c);
            for (var i = 0; i < c; ++i) {
                dis[i] = Distance.Euclidean(oldC.Row(i), newC.Row(i));
            }
            return dis.Sum();
        }


        private GroupList ComputeGroup(int[] U, int c) {
            var groups = new GroupList();
            for (var i = 0; i < c; ++i) {
                groups.Add(new Group());
                for (var j = 0; j < n; ++j)
                    if (U[j] == i)
                        groups[i].Add(data.Row(j));
            }
            return groups;
        }

        public Matrix<double> RandomCenter(int c) {
            var C = MatrixBuilder.Dense(c, d);
            var rands = Sampling.SampleFromRange(0, n, c); // 从0到n中随机无重复地抽取出c个索引值
            for (var i = 0; i < c; ++i) {
                var irand = rands[i];
                C.SetRow(i, data.Row(irand));
            }
            return C;
        }

        /// <summary>
        ///     计算对隶属度矩阵进行指数修正后的矩阵
        /// </summary>
        /// <param name="U">隶属度矩阵</param>
        /// <param name="expo">指数</param>
        /// <returns>修正后的矩阵</returns>
        private Matrix<double> ComputeUm(Matrix<double> U, double expo) => U.PointwisePower(expo);

        /// <summary>
        ///     计算聚类中心
        /// </summary>
        /// <param name="groups">聚类列表</param>
        /// <returns>聚类中心矩阵和目标函数值元组</returns>
        private Matrix<double> ComputeCenter(GroupList groups) {
            var c = groups.Count;
            var centers = MatrixBuilder.Dense(c, d);
            var offsets = VectorBuilder.Dense(c); // 记录中心的偏移距离
            for (var i = 0; i < c; ++i) {
                centers.SetRow(i, groups[i].Average((x, y) => x + y, (x, y) => (x/y)));
            }
            return centers;
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
        ///     计算隶属向量
        /// </summary>
        /// <param name="dist">距离矩阵</param>
        /// <returns>隶属度向量</returns>
        private int[] ComputeU(Matrix<double> dist) {
            var U = new int[n];
            for (var i = 0; i < n; ++i)
                U[i] = dist.Column(i).MinimumIndex();
            return U;
        }

        /// <summary>
        ///     验证参数是否正确
        /// </summary>
        /// <param name="c">分类数目</param>
        /// <param name="max_iter">结束条件:最大迭代次数</param>
        /// <param name="min_impro">结束条件:目标函数最小改进值</param>
        private void ValidateArgument(int c, int max_iter, double min_impro) {
            if (c > n)
                throw new ArgumentException(
                    "The clusters number should not be greater than observations number!");
            if (c < 2)
                throw new ArgumentException("The clusters number should be at least 2!");
            if (max_iter < 1)
                throw new ArgumentException("The maximum iterations should be at least 1!");
            if (min_impro < 0.0)
                throw new ArgumentException("minimum amount of improvement should not be negative!");
        }
    }
}