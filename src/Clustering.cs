using System.Collections.Generic;
using System.Linq;
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
        ///     根据距离矩阵计算聚类数组
        /// </summary>
        /// <param name="dist"></param>
        /// <returns></returns>
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
        ///     隶属向量
        /// </summary>
        public Vector<double> UV { get; }

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
            UV = ComputeUV();
        }

        private Vector<double> ComputeUV() {
            var uv = DenseVector.Build.Dense(U.ColumnCount);
            for (var i = 0; i < U.ColumnCount; ++i)
                uv[i] = U.Column(i).MaximumIndex();
            return uv;
        }
    }
}