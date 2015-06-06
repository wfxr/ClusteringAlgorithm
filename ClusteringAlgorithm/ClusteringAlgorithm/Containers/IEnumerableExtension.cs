using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace ClusteringAlgorithm.Containers {
    public static class EnumerableExtension {
        /// <summary>
        ///     ���ݼӷ��ͳ����Ķ���������еľ�ֵ
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="sumFunc">Ԫ�صļӷ�����</param>
        /// <param name="divFunc">Ԫ��ĳ�������</param>
        /// <returns></returns>
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public static T Average<T>(this IEnumerable<T> source, Func<T, T, T> sumFunc,
            Func<T, int, T> divFunc) => divFunc(source.Aggregate(sumFunc), source.Count());
    }
}