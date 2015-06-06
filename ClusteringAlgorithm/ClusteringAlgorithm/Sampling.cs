using System;
using System.Collections;
using System.Collections.Generic;

namespace ClusteringAlgorithm
{
    public class Sampling<T>
    {
        /// <summary>
        ///     无重复地随机抽取若干个样本
        /// </summary>
        /// <param name="observationSet">观察值集合</param>
        /// <param name="samplingAmount">抽样数目</param>
        /// <returns></returns>
        public static ObservationSet<T> WithNoRepeatition(ObservationSet<T> observationSet,
            int samplingAmount)
        {
            if (samplingAmount > observationSet.Count)
                throw new ArgumentOutOfRangeException(
                    $"number of sampling:{samplingAmount}, number of all observations:{observationSet.Count}");
            var random = new Random();
            var result = new ObservationSet<T>();
            var rest = observationSet.Copy();
            for (var i = 0; i < samplingAmount; ++i)
            {
                var randomIndex = random.Next(0, rest.Count);
                var sample = rest.TackOutAt(randomIndex);
                result.Add(sample);
            }
            return result;
        }

        /// <summary>
        ///     有重复地抽取若干个样本
        /// </summary>
        /// <param name="observationSet"></param>
        /// <param name="samplingAmount"></param>
        /// <returns></returns>
        public static ObservationSet<T> WithRepeatition(ObservationSet<T> observationSet,
            int samplingAmount)
        {
            if (samplingAmount > observationSet.Count)
                throw new ArgumentOutOfRangeException(
                    $"number of sampling:{samplingAmount}, number of all observations:{observationSet.Count}");
            var random = new Random();
            var result = new ObservationSet<T>();
            for (var i = 0; i < samplingAmount; ++i)
                result.Add(observationSet.RandomSampling(random));
            return result;
        }

        /// <summary>
        ///     随机抽取1个样本
        /// </summary>
        /// <param name="observationSet">观察值集合</param>
        /// <param name="random">随机数产生器</param>
        /// <returns></returns>
        public static T RandomSampling(IList<T> observationSet, Random random)
            => observationSet[random.Next(0, observationSet.Count)];

    }
}
