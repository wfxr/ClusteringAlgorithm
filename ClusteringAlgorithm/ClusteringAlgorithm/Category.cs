using System;
using System.Collections.Generic;

namespace ClusteringAlgorithm {
    public class Category<T> : List<T> {
        public Category() { }
        public Category(IEnumerable<T> array) : base(array) { }
        public Category(T centroid, IEnumerable<T> array) : base(array) { Centroid = centroid; }

        /// <summary>
        ///     聚类中心
        /// </summary>
        public T Centroid { get; set; }

        ///// <summary>
        /////     设置聚类中心
        ///// </summary>
        ///// <param name="centroid"></param>
        //public void SetCentroid(T centroid) => Centroid = centroid;

        /// <summary>
        ///     根据centroidFunc委托更新聚类中心
        /// </summary>
        /// <param name="centroidFunc"></param>
        public void UpdateCentroid(Func<List<T>, T> centroidFunc)
            => Centroid = centroidFunc(this);
    }
}