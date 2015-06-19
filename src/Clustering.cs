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
        ///     计算观测值的聚类划分
        /// </summary>
        /// <param name="dist">距离矩阵</param>
        /// <returns>聚类列表</returns>
        protected List<Matrix<double>> ComputeCluster(Matrix<double> dist) {
            var c = dist.RowCount;

            // PopulateDefault调用默认构造函数将将数组的每个元素赋值
            var listlist = new List<List<Vector<double>>>(c).PopulateDefault();
            // dist.Column(i).MinimumIndex()即距离矩阵中第i列距离最小的索引,
            // 也就是距离第i个观测值最近的聚类中心的索引
            for (var i = 0; i < n; ++i)
                listlist[dist.Column(i).MinimumIndex()].Add(data.Row(i));

            return listlist.Select(list => MatrixBuilder.DenseOfRowVectors(list)).ToList();
        }


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
    public class ClusterResult {
        /// <summary>
        ///     聚类中心
        /// </summary>
        public Matrix<double> Center { get; }

        /// <summary>
        ///     聚类列表
        /// </summary>
        public List<Matrix<double>> Clusters { get; }

        /// <summary>
        ///     目标函数向量
        /// </summary>
        public Vector<double> ObjectFunction { get;}

        /// <summary>
        ///     观测值所属类别的索引序列
        /// </summary>
        public Vector<double> IDX { get; }

        /// <summary>
        ///     隶属矩阵
        /// </summary>
        public Matrix<double> U { get;} 

        public ClusterResult(Matrix<double> center, Matrix<double> u, List<Matrix<double>> clusterses,
            Vector<double> obj_fcn) {
            Center = center;
            U = u;
            Clusters = clusterses;
            ObjectFunction = obj_fcn;
            IDX = ComputeIDX(u);
        }

        private Vector<double> ComputeIDX(Matrix<double> u) {
            var uv = DenseVector.Build.Dense(U.ColumnCount);
            for (var i = 0; i < U.ColumnCount; ++i)
                uv[i] = U.Column(i).MaximumIndex();
            return uv;
        }
    }
}