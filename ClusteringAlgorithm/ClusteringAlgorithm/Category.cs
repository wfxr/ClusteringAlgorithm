using System;

namespace ClusteringAlgorithm {
    public class Category<T> {
        public Category() { ObservationSet = new ObservationSet<T>(); }
        public Category(T centroid) : this() { Centroid = centroid; }

        /// <summary>
        ///     聚类中心
        /// </summary>
        public T Centroid { get; private set; }

        /// <summary>
        ///     聚类的观察值集合
        /// </summary>
        public ObservationSet<T> ObservationSet { get; set; }

        /// <summary>
        ///     返回聚类的观察值数目
        /// </summary>
        public int Count => ObservationSet.Count;

        /// <summary>
        ///     设置聚类中心
        /// </summary>
        /// <param name="centroid"></param>
        public void SetCentroid(T centroid) => Centroid = centroid;

        /// <summary>
        ///     根据centroidFunc委托更新聚类中心
        /// </summary>
        /// <param name="centroidFunc"></param>
        public void UpdateCentroid(Func<ObservationSet<T>, T> centroidFunc)
            => Centroid = centroidFunc(ObservationSet);

        /// <summary>
        ///     向聚类添加一个观察值
        /// </summary>
        /// <param name="observation"></param>
        public void Add(T observation) => ObservationSet.Add(observation);

        /// <summary>
        ///     清除聚类中所有的观察值
        /// </summary>
        public void ClearObservations() => ObservationSet = new ObservationSet<T>();
    }
}