using System;
using ClusteringAlgorithm.Containers;

namespace ClusteringAlgorithm.Algorithms {
    public class Category<T> {
        public Category() { Observations = new Set<T>(); }
        public Category(T centroid) : this() { Centroid = centroid; }

        /// <summary>
        ///     ��������
        /// </summary>
        public T Centroid { get; private set; }

        /// <summary>
        ///     ����Ĺ۲�ֵ����
        /// </summary>
        public Set<T> Observations { get; set; }

        /// <summary>
        ///     ���ؾ���Ĺ۲�ֵ��Ŀ
        /// </summary>
        public int Count => Observations.Count;

        /// <summary>
        ///     ���þ�������
        /// </summary>
        /// <param name="centroid"></param>
        public void SetCentroid(T centroid) => Centroid = centroid;

        /// <summary>
        ///     ����centroidFuncί�и��¾�������
        /// </summary>
        /// <param name="centroidFunc"></param>
        public void UpdateCentroid(Func<Set<T>, T> centroidFunc)
            => Centroid = centroidFunc(Observations);

        /// <summary>
        ///     ��������һ���۲�ֵ
        /// </summary>
        /// <param name="observation"></param>
        public void Add(T observation) => Observations.Add(observation);

        /// <summary>
        ///     ������������еĹ۲�ֵ
        /// </summary>
        public void ClearObservations() => Observations = new Set<T>();
    }
}