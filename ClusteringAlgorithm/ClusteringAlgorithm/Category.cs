using System;
using System.Collections.Generic;

namespace ClusteringAlgorithm {
    public class Category<T> : List<T> {
        public Category() { }
        public Category(IEnumerable<T> array) : base(array) { }
        public Category(T centroid, IEnumerable<T> array) : base(array) { Centroid = centroid; }

        /// <summary>
        ///     ��������
        /// </summary>
        public T Centroid { get; set; }

        ///// <summary>
        /////     ���þ�������
        ///// </summary>
        ///// <param name="centroid"></param>
        //public void SetCentroid(T centroid) => Centroid = centroid;

        /// <summary>
        ///     ����centroidFuncί�и��¾�������
        /// </summary>
        /// <param name="centroidFunc"></param>
        public void UpdateCentroid(Func<List<T>, T> centroidFunc)
            => Centroid = centroidFunc(this);
    }
}