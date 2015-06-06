using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ClusteringAlgorithm {
    public class CategorySet<T> : IEnumerable<Category<T>> {
        private readonly List<Category<T>> _categories;
        private readonly Func<ObservationSet<T>, T> _centroidFunc;
        private readonly Func<T, T, double> _distanceFunc;

        public CategorySet(Func<T, T, double> distanceFunc, Func<ObservationSet<T>, T> centroidFunc)
            : this(new List<Category<T>>(), distanceFunc, centroidFunc) {}

        public CategorySet(IEnumerable<Category<T>> observations, Func<T, T, double> distanceFunc,
            Func<ObservationSet<T>, T> centroidFunc) {
            _distanceFunc = distanceFunc;
            _centroidFunc = centroidFunc;
            _categories = observations.ToList();
        }

        /// <summary>
        ///     返回集合中聚类的数目
        /// </summary>
        public int Count => _categories.Count;

        /// <summary>
        ///     返回索引处聚类的引用
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public Category<T> this[int i] {
            get { return _categories[i]; }
            set { _categories[i] = value; }
        }

        /// <summary>
        ///     返回一个聚类集合的迭代器
        /// </summary>
        /// <returns></returns>
        public IEnumerator<Category<T>> GetEnumerator() {
            return _categories.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

        /// <summary>
        ///     清除所有聚类的观察值
        /// </summary>
        public void ClearAllObservations() {
            foreach (var category in _categories)
                category.ClearObservations();
        }

        /// <summary>
        ///     将观察值集合中所有的观察值划分到聚类中
        /// </summary>
        /// <param name="observationSet"></param>
        public void Classify(ObservationSet<T> observationSet) => observationSet.ForEach(Classify);

        /// <summary>
        ///     在集合的每一个元素上执行指定的action
        /// </summary>
        /// <param name="action"></param>
        public void ForEach(Action<Category<T>> action) {
            foreach (var observation in this)
                action(observation);
        }

        /// <summary>
        ///     将一个观察值划分到聚类中
        /// </summary>
        /// <param name="observation"></param>
        public void Classify(T observation) {
            var nearestCategory = FindNearestCategory(observation);
            nearestCategory.ObservationSet.Add(observation);
        }

        /// <summary>
        ///     寻找距离观察值最近的聚类
        /// </summary>
        /// <param name="observation"></param>
        /// <returns></returns>
        public Category<T> FindNearestCategory(T observation) {
            var minDistance = double.MaxValue;
            Category<T> nearestCategory = null;
            foreach (var category in this) {
                var distance = Distance(observation, category.Centroid);
                if (!(minDistance > distance)) continue;
                minDistance = distance;
                nearestCategory = category;
            }
            return nearestCategory;
        }

        /// <summary>
        ///     更新集合中所有聚类的中心
        /// </summary>
        /// <param name="centroidErrors"></param>
        public void UpdateCentroids(out List<double> centroidErrors) {
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
        public IEnumerable<T> Centroids() => _categories.Select(category => category.Centroid);

        /// <summary>
        ///     向集合中添加一个聚类
        /// </summary>
        /// <param name="observation"></param>
        public void Add(Category<T> observation) => _categories.Add(observation);

        /// <summary>
        ///     以中心点为Key进行升序排序（此排序函数要求实现IComparable接口）
        /// </summary>
        /// <returns></returns>
        public CategorySet<T> OrderByCentroids()
            =>
                new CategorySet<T>(_categories.OrderBy(category => category.Centroid), _distanceFunc,
                    _centroidFunc);

        /// <summary>
        ///     返回按keySelector对集合进行升序排序的结果
        /// </summary>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public CategorySet<T> OrderBy(Func<Category<T>, T> keySelector)
            => new CategorySet<T>(_categories.OrderBy(keySelector), _distanceFunc, _centroidFunc);

        /// <summary>
        ///     使用comparision对集合进行排序
        /// </summary>
        /// <param name="comparison"></param>
        public void Sort(Comparison<Category<T>> comparison) => _categories.Sort(comparison);
    }
}