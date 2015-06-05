using System;

namespace ClusteringAlgorithm {
    public class Category<T> {
        public Category() { ObservationSet = new ObservationSet<T>(); }
        public Category(T centroid) : this() { Centroid = centroid; }

        /// <summary>
        ///     ��������
        /// </summary>
        public T Centroid { get; private set; }

        /// <summary>
        ///     ����Ĺ۲�ֵ����
        /// </summary>
        public ObservationSet<T> ObservationSet { get; set; }

        /// <summary>
        ///     ���ؾ���Ĺ۲�ֵ��Ŀ
        /// </summary>
        public int Count => ObservationSet.Count;

        /// <summary>
        ///     ���þ�������
        /// </summary>
        /// <param name="centroid"></param>
        public void SetCentroid(T centroid) => Centroid = centroid;

        /// <summary>
        ///     ����centroidFuncί�и��¾�������
        /// </summary>
        /// <param name="centroidFunc"></param>
        public void UpdateCentroid(Func<ObservationSet<T>, T> centroidFunc)
            => Centroid = centroidFunc(ObservationSet);

        /// <summary>
        ///     ��������һ���۲�ֵ
        /// </summary>
        /// <param name="observation"></param>
        public void Add(T observation) => ObservationSet.Add(observation);

        /// <summary>
        ///     ������������еĹ۲�ֵ
        /// </summary>
        public void ClearObservations() => ObservationSet = new ObservationSet<T>();
    }
}