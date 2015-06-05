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
        /// �����й۲�ֵ����Ŀ
        /// </summary>
        public int Count => _observations.Count;

        /// <summary>
        /// �����������۲�ֵ������
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public T this[int i] {
            get { return _observations[i]; }
            set { _observations[i] = value; }
        }

        /// <summary>
        /// ����һ���۲�ֵ���ϵĵ�����
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator() => _observations.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// �򼯺����һ���۲�ֵ
        /// </summary>
        /// <param name="observation"></param>
        public void Add(T observation) => _observations.Add(observation);

        /// <summary>
        ///     �������ظ���ObservationSet
        /// </summary>
        /// <returns></returns>
        public ObservationSet<T> Distinct() => new ObservationSet<T>(_observations.Distinct());

        /// <summary>
        ///     �����ȡ���ɸ�����
        /// </summary>
        /// <param name="count">������Ŀ</param>
        /// <returns></returns>
        public IEnumerable<T> RandomSample(int count) => RandomSample(this, count);

        /// <summary>
        ///     �����ȡ���ɸ�����
        /// </summary>
        /// <param name="observationSet">�۲�ֵ����</param>
        /// <param name="count">������Ŀ</param>
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
        ///     �����ȡ1������
        /// </summary>
        /// <param name="observationSet">�۲�ֵ����</param>
        /// <param name="random">�����������</param>
        /// <returns></returns>
        public static T RandomSample(ObservationSet<T> observationSet, Random random)
            => observationSet[random.Next(0, observationSet.Count)];

        /// <summary>
        ///     �����ȡ1������
        /// </summary>
        /// <param name="random">�����������</param>
        /// <returns></returns>
        public T RandomSample(Random random) => RandomSample(this, random);

        /// <summary>
        /// �ڼ��ϵ�ÿһ��Ԫ����ִ��ָ����action
        /// </summary>
        /// <param name="action"></param>
        public void ForEach(Action<T> action) {
            foreach (var observation in this)
                action(observation);
        }
    }
}