using System;
using System.Collections.Generic;

namespace ClusteringAlgorithm {
    public class Category<T> : List<T> {
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
        public void UpdateCentroid(Func<List<T>, T> centroidFunc)
            => Centroid = centroidFunc(this);
    }
}