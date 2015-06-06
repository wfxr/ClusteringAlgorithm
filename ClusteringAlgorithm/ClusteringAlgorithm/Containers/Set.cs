using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ClusteringAlgorithm.Containers {
    public class Set<T> : IList<T>, IReadOnlyList<T> {
        protected readonly List<T> Elements;
        public Set() { Elements = new List<T>(); }
        public Set(IEnumerable<T> items) { Elements = items.ToList(); }

        /// <summary>
        ///     从集合中删除指定的值
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(T item) => Elements.Remove(item);

        /// <summary>
        ///     返回集合元素的数目
        /// </summary>
        public int Count => Elements.Count;

        bool ICollection<T>.IsReadOnly => ((IList<T>) Elements).IsReadOnly;

        /// <summary>
        ///     返回索引处元素的引用
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public T this[int i] {
            get { return Elements[i]; }
            set { Elements[i] = value; }
        }

        /// <summary>
        ///     返回集合的迭代器
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator() => Elements.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        ///     向集合添加一个元素
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item) => Elements.Add(item);

        /// <summary>
        ///     清除集合中所有的元素
        /// </summary>
        public void Clear() => Elements.Clear();

        /// <summary>
        ///     返回集合是否包含指定的值
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(T item) => Elements.Contains(item);

        /// <summary>
        ///     将整个集合拷贝到一个一维数组中索引指定的位置
        /// </summary>
        /// <param name="itemsArray"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(T[] itemsArray, int arrayIndex)
            => Elements.CopyTo(itemsArray, arrayIndex);

        /// <summary>
        ///     返回指定值在集合中第一次出现的位置
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int IndexOf(T item) { return Elements.IndexOf(item); }

        /// <summary>
        ///     将一个值插入到索引指定的位置
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public void Insert(int index, T item) { Elements.Insert(index, item); }

        /// <summary>
        ///     移除索引指定位置处的值
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index) => Elements.RemoveAt(index);

        /// <summary>
        ///     提供从List到Set的隐式转换
        /// </summary>
        /// <param name="items"></param>
        public static implicit operator Set<T>(List<T> items) { return new Set<T>(items); }

        /// <summary>
        ///     返回去重复之后的集合
        /// </summary>
        /// <returns></returns>
        public Set<T> Distinct() => new Set<T>(Elements.Distinct());

        /// <summary>
        ///     返回集合的一个副本
        /// </summary>
        /// <returns></returns>
        public Set<T> Copy() => new Set<T>(this);

        /// <summary>
        ///     在集合的每一个元素上执行指定的action
        /// </summary>
        /// <param name="action"></param>
        public void ForEach(Action<T> action) {
            foreach (var item in this)
                action(item);
        }
    }
}