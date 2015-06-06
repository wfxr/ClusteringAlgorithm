using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace ClusteringAlgorithm {
    public class ObservationSet<T> : IList<T>, IReadOnlyList<T> {
        private readonly List<T> _observations;
        public ObservationSet() { _observations = new List<T>(); }

        public ObservationSet(IEnumerable<T> observations) {
            _observations = observations.ToList();
        }

        /// <summary>
        ///     �Ӽ�����ɾ��ָ����ֵ
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(T item) => _observations.Remove(item);

        /// <summary>
        ///     �����й۲�ֵ����Ŀ
        /// </summary>
        public int Count => _observations.Count;

        bool ICollection<T>.IsReadOnly => ((IList<T>) _observations).IsReadOnly;

        /// <summary>
        ///     �����������۲�ֵ������
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public T this[int i] {
            get { return _observations[i]; }
            set { _observations[i] = value; }
        }

        /// <summary>
        ///     ����һ���۲�ֵ���ϵĵ�����
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator() => _observations.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        ///     �򼯺����һ���۲�ֵ
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
        ///     �������ظ���ObservationSet
        /// </summary>
        /// <returns></returns>
        public ObservationSet<T> Distinct() => new ObservationSet<T>(_observations.Distinct());

        /// <summary>
        /// ���������һ������
        /// </summary>
        /// <returns></returns>
        public ObservationSet<T> Copy() => new ObservationSet<T>(this); 

        /// <summary>
        ///     �ڼ��ϵ�ÿһ��Ԫ����ִ��ָ����action
        /// </summary>
        /// <param name="action"></param>
        public void ForEach(Action<T> action) {
            foreach (var observation in this)
                action(observation);
        }
    }
}