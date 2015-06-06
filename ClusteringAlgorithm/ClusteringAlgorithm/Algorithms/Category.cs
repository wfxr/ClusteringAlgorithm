using System;
using ClusteringAlgorithm.Containers;

namespace ClusteringAlgorithm.Algorithms {
    public class Category<T> {
        public Category() { Observations = new Set<T>(); }
        public Category(T centroid) : this() { Centroid = centroid; }

        /// <summary>
        ///     聚类中心
        /// </summary>
        public T Centroid { get; private set; }

        /// <summary>
        ///     聚类的观察值集合
        /// </summary>
        public Set<T> Observations { get; set; }

        /// <summary>
        ///     返回聚类的观察值数目
        /// </summary>
        public int Count => Observations.Count;

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
            => Centroid = centroidFunc(Observations);

        /// <summary>
        ///     向聚类添加一个观察值
        /// </summary>
        /// <param name="observation"></param>
        public void Add(T observation) => Observations.Add(observation);

        /// <summary>
        ///     清除聚类中所有的观察值
        /// </summary>
        public void ClearObservations() => Observations = new Set<T>();
    }
}