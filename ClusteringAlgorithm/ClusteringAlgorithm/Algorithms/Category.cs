using System;
using Wfxr.Container;

namespace ClusteringAlgorithm.Algorithms {
    public class Category<T> : Set<T> {
        public Category() { }
        public Category(T centroid) : this() { Centroid = centroid; }

        /// <summary>
        ///     聚类中心
        /// </summary>
        public T Centroid { get; private set; }

        /// <summary>
        ///     设置聚类中心
        /// </summary>
        /// <param name="centroid"></param>
        public void SetCentroid(T centroid) => Centroid = centroid;

        /// <summary>
        ///     根据centroidFunc委托更新聚类中心
        /// </summary>
        /// <param name="centroidFunc"></param>
        public void UpdateCentroid(Func<Set<T>, T> centroidFunc)
            => Centroid = centroidFunc(Elements);
    }
}