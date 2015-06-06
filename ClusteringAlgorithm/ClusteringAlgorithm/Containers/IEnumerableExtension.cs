using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace ClusteringAlgorithm.Containers {
    public static class EnumerableExtension {
        /// <summary>
        ///     计算序列的均值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="sumFunc">元素的加法定义</param>
        /// <param name="divFunc">元算的除法定义</param>
        /// <returns></returns>
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public static T Average<T>(this IEnumerable<T> source, Func<T, T, T> sumFunc,
            Func<T, double, T> divFunc) => divFunc(source.Aggregate(sumFunc), source.Count());
    }
}