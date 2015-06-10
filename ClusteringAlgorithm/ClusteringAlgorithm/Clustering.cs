using MathNet.Numerics.LinearAlgebra;
// ReSharper disable InconsistentNaming

namespace ClusteringAlgorithm
{
    public abstract class Clustering
    {
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
}
