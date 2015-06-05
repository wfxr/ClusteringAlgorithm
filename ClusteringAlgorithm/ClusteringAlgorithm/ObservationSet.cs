using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ClusteringAlgorithm {
    public class ObservationSet<T> : IEnumerable<T> {
        private readonly List<T> _observations;
        public ObservationSet() { _observations = new List<T>(); }
        public ObservationSet(IEnumerable<T> observations) {
            _observations = observations.ToList();
        }

        /// <summary>
        /// 集合中观察值的数目
        /// </summary>
        public int Count => _observations.Count;

        /// <summary>
        /// 返回索引处观察值的引用
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public T this[int i] {
            get { return _observations[i]; }
            set { _observations[i] = value; }
        }

        /// <summary>
        /// 返回一个观察值集合的迭代器
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator() => _observations.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// 向集合添加一个观察值
        /// </summary>
        /// <param name="observation"></param>
        public void Add(T observation) => _observations.Add(observation);

        /// <summary>
        ///     返回无重复的ObservationSet
        /// </summary>
        /// <returns></returns>
        public ObservationSet<T> Distinct() => new ObservationSet<T>(_observations.Distinct());

        /// <summary>
        ///     随机抽取若干个样本
        /// </summary>
        /// <param name="count">抽样数目</param>
        /// <returns></returns>
        public IEnumerable<T> RandomSample(int count) => RandomSample(this, count);

        /// <summary>
        ///     随机抽取若干个样本
        /// </summary>
        /// <param name="observationSet">观察值集合</param>
        /// <param name="count">抽样数目</param>
        /// <returns></returns>
        public static IEnumerable<T> RandomSample(ObservationSet<T> observationSet, int count) {
            var r = new Random();
            var ret = new ObservationSet<T>();
            while (count > 0) {
                var obs = observationSet[r.Next(0, observationSet.Count)];
                if (!ret.Contains(obs)) {
                    ret.Add(obs);
                    count--;
                }
            }
            return ret;
        }

        /// <summary>
        ///     随机抽取1个样本
        /// </summary>
        /// <param name="observationSet">观察值集合</param>
        /// <param name="random">随机数产生器</param>
        /// <returns></returns>
        public static T RandomSample(ObservationSet<T> observationSet, Random random)
            => observationSet[random.Next(0, observationSet.Count)];

        /// <summary>
        ///     随机抽取1个样本
        /// </summary>
        /// <param name="random">随机数产生器</param>
        /// <returns></returns>
        public T RandomSample(Random random) => RandomSample(this, random);

        /// <summary>
        /// 在集合的每一个元素上执行指定的action
        /// </summary>
        /// <param name="action"></param>
        public void ForEach(Action<T> action) {
            foreach (var observation in this)
                action(observation);
        }
    }
}