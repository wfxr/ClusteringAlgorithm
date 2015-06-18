using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

// ReSharper disable InconsistentNaming

namespace ClusteringAlgorithm {
    using Cluster = List<Vector<double>>;

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
        public List<Cluster> Clusters { get; }

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

        public ClusterResult(Matrix<double> center, Matrix<double> u, IEnumerable<Cluster> clusterses,
            Vector<double> obj_fcn) {
            Center = center;
            U = u;
            Clusters = clusterses.ToList();
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