using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ClusteringAlgorithm {
    public class CategorySet<T> : IEnumerable<Category<T>> {
        private readonly List<Category<T>> _categories;
        public CategorySet() { _categories = new List<Category<T>>(); }

        public CategorySet(IEnumerable<Category<T>> observations) {
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

        public IEnumerator<Category<T>> GetEnumerator() { return _categories.GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

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
        ///     返回无重复的CategorySet
        /// </summary>
        /// <returns></returns>
        public CategorySet<T> Distinct() => new CategorySet<T>(_categories.Distinct());

        /// <summary>
        ///     以中心点为Key进行升序排序（此排序函数要求实现IComparable接口）
        /// </summary>
        /// <returns></returns>
        public CategorySet<T> OrderByCentroids()
            => new CategorySet<T>(_categories.OrderBy(category => category.Centroid));

        /// <summary>
        ///     按keySelector进行升序排序
        /// </summary>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public CategorySet<T> OrderBy(Func<Category<T>, T> keySelector)
            => new CategorySet<T>(_categories.OrderBy(keySelector));

        /// <summary>
        ///     使用comparision对整个集合进行排序
        /// </summary>
        /// <param name="comparison"></param>
        public void Sort(Comparison<Category<T>> comparison) => _categories.Sort(comparison);
    }
}