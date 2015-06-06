using System;
using System.Collections.Generic;
using System.Linq;
using ClusteringAlgorithm.Containers;

namespace ClusteringAlgorithm.Algorithms {
    public class CategorySet<T> : Set<Category<T>> {
        private readonly Func<Set<T>, T> _centroidFunc;
        private readonly Func<T, T, double> _distanceFunc;

        public CategorySet(Func<T, T, double> distanceFunc, Func<Set<T>, T> centroidFunc)
            : this(new List<Category<T>>(), distanceFunc, centroidFunc) { }

        public CategorySet(IEnumerable<Category<T>> observations, Func<T, T, double> distanceFunc,
            Func<Set<T>, T> centroidFunc) : base(observations) {
            _distanceFunc = distanceFunc;
            _centroidFunc = centroidFunc;
        }

        /// <summary>
        ///     清除所有聚类的观察值
        /// </summary>
        public void ClearAllCategories() => ForEach(category => category.Clear());

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
            foreach (var category in this) {
                var distanceToThisCategory = Distance(observation, category.Centroid);
                if (distanceToThisCategory < minDistance) {
                    minDistance = distanceToThisCategory;
                    nearestCategory = category;
                }
            }
            return nearestCategory;
        }

        /// <summary>
        ///     更新所有聚类的中心
        /// </summary>
        /// <param name="centroidErrors"></param>
        public void UpdateAllCentroids(out List<double> centroidErrors) {
            centroidErrors = new List<double>();
            foreach (var category in this) {
                var oldCentroid = category.Centroid;
                category.UpdateCentroid(_centroidFunc);
                var error = Distance(oldCentroid, category.Centroid);
                centroidErrors.Add(error);
            }
        }

        /// <summary>
        ///     返回观察值之间的距离
        /// </summary>
        /// <param name="obs1"></param>
        /// <param name="obs2"></param>
        /// <returns></returns>
        public double Distance(T obs1, T obs2) => _distanceFunc(obs1, obs2);

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
    }
}