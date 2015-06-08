using System;
using System.Collections.Generic;
using System.Linq;

namespace ClusteringAlgorithm {
    public class CategorySet<T> : HashSet<Category<T>>{
        private readonly Func<List<T>, T> _centroidFunc;
        private readonly Func<T, T, double> _distanceFunc;

        public CategorySet(Func<T, T, double> distanceFunc, Func<List<T>, T> centroidFunc)
            : this(new List<Category<T>>(), distanceFunc, centroidFunc) { }

        public CategorySet(IEnumerable<Category<T>> observations, Func<T, T, double> distanceFunc,
            Func<List<T>, T> centroidFunc) : base(observations) {
            _distanceFunc = distanceFunc;
            _centroidFunc = centroidFunc;
        }


        /// <summary>
        ///     清除所有聚类的观察值
        /// </summary>
        public void ClearAllObservations() {
            foreach (var category in this)
                category.Clear();
        }

        /// <summary>
        ///     将观察值集合中所有的观察值划分到聚类中
        /// </summary>
        /// <param name="observations"></param>
        public void Classify(IEnumerable<T> observations) {
            foreach (var observation in observations)
                Classify(observation);
        }

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
        public Category<T> FindNearestCategory(T observation)
            => FindNearestCategory(this, observation);

        /// <summary>
        ///     返回距离观察值最近的聚类
        /// </summary>
        /// <param name="categorySet"></param>
        /// <param name="observation"></param>
        /// <returns></returns>
        public static Category<T> FindNearestCategory(CategorySet<T> categorySet, T observation) {
            var minDistance = double.MaxValue;
            Category<T> nearestCategory = null;

            foreach (var thisCategory in categorySet) {
                var distanceToThisCategory = categorySet.Distance(observation, thisCategory.Centroid);
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
        public void UpdateAllCentroids() {
            foreach (var category in this)
                category.UpdateCentroid(_centroidFunc);
        }

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
        public IEnumerable<T> Centroids() => this.Select(category => category.Centroid);
    }
}