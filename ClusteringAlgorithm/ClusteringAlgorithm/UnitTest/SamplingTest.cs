using System;
using System.Linq;
using Xunit;

namespace ClusteringAlgorithm.UnitTest
{
    public class SamplingTest
    {
        [Fact]
        public void TestSamplingWithNoRepeatition() {
            // 如果观察值集合本身有重复，则抽样结果也可能出现重复
            // 所以应先使集合去重复，然后才能确保得到正确的测试结果
            var set = new ObservationSet<int> {1, 4, 8, 5, 9, 7}.Distinct();
            var samples = Sampling.WithNoRepeatition(set, 3);
            Assert.Equal(samples.Count, 3);
            foreach (var sample in samples)
                Assert.Contains(sample, set);
            Assert.Equal(samples.Distinct(), samples);

            Assert.Throws<ArgumentOutOfRangeException>(
                () => Sampling.WithNoRepeatition(set, 100));
        }
        [Fact]
        public void TestSamplingWithRepeatition() {
            var set = new ObservationSet<int> {1, 4, 8, 5, 9, 7};
            var samples = Sampling.WithRepeatition(set, 3);

            Assert.Equal(samples.Count, 3);
            foreach (var sample in samples)
                Assert.Contains(sample, set);
            Assert.Throws<ArgumentOutOfRangeException>(
                () => Sampling.WithNoRepeatition(set, 100));
        }
        [Fact]
        public void TestRandomSampling() {
            var set = new ObservationSet<int> {1, 4, 8, 5, 9, 7};
            var sample = Sampling.RandomSampling(set);

            Assert.Contains(sample, set);
        }
    }
}
