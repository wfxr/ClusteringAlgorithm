using System;
using System.Collections.Generic;

namespace ClusteringAlgorithm {
    public class Sampling<T> {
        private static Random Rand { get; } = new Random();

        /// <summary>
        ///     无重复地随机抽取若干个样本
        /// </summary>
        /// <param name="observationSet">观察值集合</param>
        /// <param name="sampleSize">抽样数目</param>
        /// <returns></returns>
        public static ObservationSet<T> WithNoRepeatition(ObservationSet<T> observationSet,
            int sampleSize) {
            Validate(observationSet, sampleSize);

            var result = new ObservationSet<T>();
            var rest = observationSet.Copy();
            for (var i = 0; i < sampleSize; ++i) {
                var randomIndex = Rand.Next(0, rest.Count);
                var sample = rest.TackOutAt(randomIndex);
                result.Add(sample);
            }
            return result;
        }

        /// <summary>
        ///     有重复地抽取若干个样本
        /// </summary>
        /// <param name="observationSet">观察值集合</param>
        /// <param name="sampleSize">随机数产生器</param>
        /// <returns></returns>
        public static ObservationSet<T> WithRepeatition(ObservationSet<T> observationSet,
            int sampleSize) {
            Validate(observationSet, sampleSize);

            var result = new ObservationSet<T>();
            for (var i = 0; i < sampleSize; ++i)
                result.Add(RandomSampling(observationSet));
            return result;
        }

        /// <summary>
        ///     随机抽取1个样本
        /// </summary>
        /// <param name="observationSet">观察值集合</param>
        /// <returns></returns>
        public static T RandomSampling(IList<T> observationSet)
            => observationSet[Rand.Next(0, observationSet.Count)];

        private static void Validate(ICollection<T> observationSet, int sampleSize) {
            if (sampleSize > observationSet.Count)
                throw new ArgumentOutOfRangeException(
                    $"number of sampling:{sampleSize}, number of all observations:{observationSet.Count}");
        }
    }
}