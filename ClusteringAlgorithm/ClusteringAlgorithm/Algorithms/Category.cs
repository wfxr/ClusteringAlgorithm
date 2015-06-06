using System;
using Wfxr.Container;

namespace ClusteringAlgorithm.Algorithms {
    public class Category<T> : Set<T> {
        public Category() { }
        public Category(T centroid) : this() { Centroid = centroid; }

        /// <summary>
        ///     ��������
        /// </summary>
        public T Centroid { get; private set; }

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
            => Centroid = centroidFunc(Elements);
    }
}