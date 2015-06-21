using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using Wfxr.Utility.Container;

// ReSharper disable InconsistentNaming

namespace ClusteringAlgorithm {
    public abstract class Clustering {
        protected static readonly MatrixBuilder<double> MatrixBuilder = Matrix<double>.Build;
        protected static readonly VectorBuilder<double> VectorBuilder = Vector<double>.Build;
        protected Clustering(Matrix<double> data) { this.data = data; }
        protected Matrix<double> data { get; }

        /// <summary>
        ///     观测值的数目
        /// </summary>
        protected int n => data.RowCount;

        /// <summary>
        ///     观测值的维数
        /// </summary>
        protected int d => data.ColumnCount;


        /// <summary>
        ///     计算每个观测值到各分类中心的距离
        /// </summary>
        /// <param name="C">聚类中心矩阵</param>
        /// <returns>距离矩阵</returns>
        protected Matrix<double> ComputeDistance(Matrix<double> C) {
            var dist = MatrixBuilder.Dense(C.RowCount, n);
            for (var i = 0; i < C.RowCount; ++i)
                for (var j = 0; j < n; ++j)
                    dist[i, j] = Distance.Euclidean(data.Row(j), C.Row(i));
            return dist;
        }

        /// <summary>
        ///     计算分类中心
        /// </summary>
        /// <param name="U">隶属度(加权)矩阵</param>
        /// <returns>中心矩阵</returns>
        protected Matrix<double> ComputeCenter(Matrix<double> U)
            => (U*data).PointwiseDivide(U.RowSums().ToColumnMatrix()*MatrixBuilder.Dense(1, d, 1));

        /// <summary>
        ///     验证参数是否正确
        /// </summary>
        /// <param name="c">分类数目</param>
        /// <param name="max_iter">结束条件:最大迭代次数</param>
        /// <param name="min_impro">结束条件:目标函数最小改进值</param>
        protected void ValidateArgument(int c, int max_iter, double min_impro) {
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

    /// <summary>
    ///     用以存储聚类结果的数据结构
    /// </summary>
    public class ClusterReport {
        private static readonly MatrixBuilder<double> MatrixBuilder = Matrix<double>.Build;
        private static readonly VectorBuilder<double> VectorBuilder = Vector<double>.Build;

        public ClusterReport(Matrix<double> obs, Matrix<double> center, Matrix<double> u,
            Vector<double> obj_fcn) {
            Obs = obs;
            Center = center;
            U = u;
            Idx = ComputeIdx(u);
            ObjectFunction = obj_fcn;
        }

        /// <summary>
        ///     聚类中心
        /// </summary>
        public Matrix<double> Center { get; }

        /// <summary>
        ///     计算观测值的所属聚类序列
        /// </summary>
        /// <param name="u">隶属度矩阵</param>
        /// <returns>聚类序列</returns>
        private int[] ComputeIdx(Matrix<double> u) {
            var idx = new int[ObsCount];
            for (var i = 0; i < ObsCount; ++i)
                idx[i] = u.Column(i).MaximumIndex();
            return idx;
        }

        /// <summary>
        ///     返回聚类列表
        /// </summary>
        /// <returns>聚类列表</returns>
        public List<Matrix<double>> GetClusterList() {
            var list = new List<Matrix<double>>();
            for (var cluster = 0; cluster < ClusterCount; ++cluster) {
                var rows = new List<Vector<double>>();
                for (var j = 0; j < ObsCount; ++j)
                    if (Idx[j] == cluster) // 如果该观测值属于该类,就将其加入rows
                        rows.Add(Obs.Row(j));
                list.Add(MatrixBuilder.DenseOfRows(rows));
            }
            return list;
        }

        /// <summary>
        ///     目标函数向量
        /// </summary>
        public Vector<double> ObjectFunction { get; }

        /// <summary>
        ///     观测值数目
        /// </summary>
        public int ObsCount => Obs.RowCount;

        /// <summary>
        ///     观测值维度
        /// </summary>
        public int ObsDimension => Obs.ColumnCount;

        /// <summary>
        ///     聚类数目
        /// </summary>
        public int ClusterCount => Center.RowCount;

        /// <summary>
        ///     隶属矩阵
        /// </summary>
        public Matrix<double> U { get; }

        /// <summary>
        ///     获取观测值所属聚类的索引序列
        /// </summary>
        public int[] Idx { get; }

        /// <summary>
        ///     获取观测值矩阵
        /// </summary>
        /// <value></value>
        public Matrix<double> Obs { get; }
    }
}