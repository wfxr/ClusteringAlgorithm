using System;
using System.Collections.Generic;
using System.Linq;

namespace ClusteringAlgorithm.Statistics {
    public class Sampling {
        private static Random Rand { get; } = new Random();

        /// <summary>
        ///     无重复地随机抽取指定数目的样本
        /// </summary>
        /// <param name="set">样本总体</param>
        /// <param name="count">抽样数目</param>
        /// <returns></returns>
        public static List<T> SampleWithOutReplacement<T>(IReadOnlyList<T> set, int count) {
            Validate(set, count);

            var result = new List<T>();
            var rest = set.ToList();
            for (var i = 0; i < count; ++i) {
                var randomIndex = Rand.Next(0, rest.Count);
                var sample = rest[randomIndex];
                result.Add(sample);
                rest.RemoveAt(randomIndex);
            }
            return result;
        }

        /// <summary>
        ///     有重复地随机抽取指定数目的样本
        /// </summary>
        /// <param name="set">样本总体</param>
        /// <param name="count">抽样数目</param>
        /// <returns></returns>
        public static List<T> Sample<T>(IReadOnlyList<T> set, int count) {
            Validate(set, count);

            var result = new List<T>();
            for (var i = 0; i < count; ++i)
                result.Add(Sample(set));
            return result;
        }

        /// <summary>
        ///     随机抽取1个样本
        /// </summary>
        /// <param name="set">观察值集合</param>
        /// <returns></returns>
        public static T Sample<T>(IReadOnlyList<T> set) => set[Rand.Next(0, set.Count)];

        private static void Validate<T>(IReadOnlyCollection<T> set, int count) {
            if (count > set.Count)
                throw new ArgumentOutOfRangeException(
                    $"samples to draw:{count}, samples in set:{set.Count}");
        }
    }
}