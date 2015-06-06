using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace ClusteringAlgorithm {
    public class ObservationSet<T> : IList<T> {
        private readonly List<T> _observations;
        public ObservationSet() { _observations = new List<T>(); }

        public ObservationSet(IEnumerable<T> observations) {
            _observations = observations.ToList();
        }

        public bool Remove(T item) { throw new NotImplementedException(); }

        /// <summary>
        ///     集合中观察值的数目
        /// </summary>
        public int Count => _observations.Count;

        bool ICollection<T>.IsReadOnly => ((IList<T>) _observations).IsReadOnly;

        /// <summary>
        ///     返回索引处观察值的引用
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public T this[int i] {
            get { return _observations[i]; }
            set { _observations[i] = value; }
        }

        /// <summary>
        ///     返回一个观察值集合的迭代器
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator() => _observations.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        ///     向集合添加一个观察值
        /// </summary>
        /// <param name="observation"></param>
        public void Add(T observation) => _observations.Add(observation);
        public void Clear() => _observations.Clear();
        public bool Contains(T observation) => _observations.Contains(observation);
        public void CopyTo(T[] observationArray, int arrayIndex)
            => _observations.CopyTo(observationArray, arrayIndex);

        public int IndexOf(T item) { throw new NotImplementedException(); }
        public void Insert(int index, T item) { throw new NotImplementedException(); }
        public void RemoveAt(int index) => _observations.RemoveAt(index);

        /// <summary>
        ///     返回无重复的ObservationSet
        /// </summary>
        /// <returns></returns>
        public ObservationSet<T> Distinct() => new ObservationSet<T>(_observations.Distinct());

        /// <summary>
        ///     无重复地随机抽取若干个样本
        /// </summary>
        /// <param name="count">抽样数目</param>
        /// <returns></returns>
        public ObservationSet<T> SamplingWithNoRepeatition(int count)
            => Sampling<T>.WithNoRepeatition(this, count);

        /// <summary>
        ///     有重复地随机抽取若干个样本
        /// </summary>
        /// <param name="count">抽样数目</param>
        /// <returns></returns>
        public ObservationSet<T> SamplingWithRepeatition(int count)
            => Sampling<T>.WithRepeatition(this, count);
         
        /// <summary>
        ///     随机抽取1个样本
        /// </summary>
        /// <param name="random">随机数产生器</param>
        /// <returns></returns>
        public T RandomSampling(Random random) => Sampling<T>.RandomSampling(this, random);

        /// <summary>
        /// 返回自身的一个副本
        /// </summary>
        /// <returns></returns>
        public ObservationSet<T> Copy() => new ObservationSet<T>(this); 

        /// <summary>
        /// 从集合中取走索引指定的观察值
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T TackOutAt(int index) {
            var ret = this[index];
            RemoveAt(index);
            return ret;
        }

        /// <summary>
        ///     在集合的每一个元素上执行指定的action
        /// </summary>
        /// <param name="action"></param>
        public void ForEach(Action<T> action) {
            foreach (var observation in this)
                action(observation);
        }
    }
}