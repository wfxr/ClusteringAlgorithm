using System;
using System.Collections.Generic;
using System.Linq;

// TODO:改进哈希值算法
namespace ClusteringAlgorithm {
    public class Category<T> : List<T> {
        public Category() { }
        public Category(IEnumerable<T> array) : base(array) { }
        public Category(T centroid, IEnumerable<T> array) : base(array) { Centroid = centroid; }

        /// <summary>
        ///     聚类中心
        /// </summary>
        public T Centroid { get; set; }

        /// <summary>
        ///     比较两个聚类是否相等
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        protected bool Equals(Category<T> other) {
            if (Count != other.Count)
                return false;
            if (!Centroid.Equals(other.Centroid)) return false;
            for (var i = 0; i < Count; ++i) {
                if (!this[i].Equals(other[i]))
                    return false;
            }
            return true;
        }

        /// <summary>
        ///     获取聚类的哈希值
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
            =>
                this.Aggregate((5381 << 16) + 5381,
                    (hash, observation) => hash ^ observation.GetHashCode());

        /// <summary>
        ///     返回聚类与obj对象是否相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Category<T>) obj);
        }

        /// <summary>
        ///     根据centroidFunc委托更新聚类中心
        /// </summary>
        /// <param name="centroidFunc"></param>
        public void UpdateCentroid(Func<List<T>, T> centroidFunc)
            => Centroid = centroidFunc(this);
    }
}