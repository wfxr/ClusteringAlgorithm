using System;
using System.Collections.Generic;
using System.Linq;
using Wfxr.Utility.Container;

namespace ClusteringAlgorithm {
    public class CategorySet<T> : Set<Category<T>> {
        protected bool Equals(CategorySet<T> other) {
            return Equals(_centroidFunc, other._centroidFunc) && Equals(_distanceFunc, other._distanceFunc) &&
                Elements.Equals(other.Elements);
        }

        public override int GetHashCode() {
            unchecked {
                return ((_centroidFunc?.GetHashCode() ?? 0)*397) ^ (_distanceFunc?.GetHashCode() ?? 0);
            }
        }

        private readonly Func<Set<T>, T> _centroidFunc;
        private readonly Func<T, T, double> _distanceFunc;

        public CategorySet(Func<T, T, double> distanceFunc, Func<Set<T>, T> centroidFunc)
            : this(new List<Category<T>>(), distanceFunc, centroidFunc) {}

        public CategorySet(IEnumerable<Category<T>> observations, Func<T, T, double> distanceFunc,
            Func<Set<T>, T> centroidFunc) : base(observations) {
            _distanceFunc = distanceFunc;
            _centroidFunc = centroidFunc;
        }

        /// <summary>
        ///     清除所有聚类的观察值
        /// </summary>
        public void ClearAllObservations() => ForEach(category => category.Clear());

        /// <summary>
        ///     将观察值集合中所有的观察值划分到聚类中
        /// </summary>
        /// <param name="observations"></param>
        public void Classify(Set<T> observations) => observations.ForEach(Classify);

        /// <summary>
        ///     将一个观察值划分到聚类中
        /// </summary>
        /// <param name="observation"></param>
        public void Classify(T observation)
            => FindNearestCategory(observation).Add(observation);

        /// <summary>
        ///     返回距离观察值最近的聚类
        /// </summary>
        /// <param name="observation"></param>
        /// <returns></returns>
        public Category<T> FindNearestCategory(T observation) {
            var minDistance = double.MaxValue;
            Category<T> nearestCategory = null;

            foreach (var thisCategory in this) {
                var distanceToThisCategory = Distance(observation, thisCategory.Centroid);
                if (distanceToThisCategory < minDistance) {
                    minDistance = distanceToThisCategory;
                    nearestCategory = thisCategory;
                }
            }
            return nearestCategory;
        }

        /// <summary>
        ///     更新所有聚类的中心b并输出更新后中心的偏移距离
        /// </summary>
        /// <param name="distanceOffsets"></param>
        public void UpdateAllCentroids(out List<double> distanceOffsets) {
            distanceOffsets = new List<double>();
            foreach (var category in this) {
                var oldCentroid = category.Centroid;
                category.UpdateCentroid(_centroidFunc);
                var error = Distance(oldCentroid, category.Centroid);
                distanceOffsets.Add(error);
            }
        }

        /// <summary>
        ///     更新所有聚类的中心
        /// </summary>
        public void UpdateAllCentroids()
            => ForEach(category => category.UpdateCentroid(_centroidFunc));

        /// <summary>
        ///     返回观察值之间的距离
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public double Distance(T x, T y) => _distanceFunc(x, y);

        /// <summary>
        ///     返回所有的聚类中心
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> Centroids() => Elements.Select(category => category.Centroid);

        /// <summary>
        ///     返回按Centroid进行升序排序的结果（要求实现IComparable接口）
        /// </summary>
        /// <returns></returns>
        public CategorySet<T> OrderByCentroids()
            =>
                new CategorySet<T>(Elements.OrderBy(category => category.Centroid), _distanceFunc,
                    _centroidFunc);

        /// <summary>
        ///     返回按keySelector进行升序排序的结果
        /// </summary>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public CategorySet<T> OrderBy(Func<Category<T>, T> keySelector)
            => new CategorySet<T>(Elements.OrderBy(keySelector), _distanceFunc, _centroidFunc);

        /// <summary>
        ///     使用comparision对集合进行排序
        /// </summary>
        /// <param name="comparison"></param>
        public void Sort(Comparison<Category<T>> comparison) => Elements.Sort(comparison);

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CategorySet<T>) obj);
        }
    }
}