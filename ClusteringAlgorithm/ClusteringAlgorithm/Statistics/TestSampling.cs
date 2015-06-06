﻿using System;
using System.Collections.Generic;
using System.Linq;
using ClusteringAlgorithm.Containers;
using Xunit;

namespace ClusteringAlgorithm.Statistics {
    public class TestSampling {
        [Fact]
        public void TestSampleWithOutReplacement() {
            // 如果观察值集合本身有重复，则抽样结果也可能出现重复
            // 所以应先使集合去重复，然后才能确保得到正确的测试结果
            var set = new Set<int> {1, 4, 8, 5, 9, 7}.Distinct();
            var samples = Sampling.SampleWithOutReplacement(set, 3);

            Assert.Equal(samples.Count, 3);
            Assert.Equal(samples.Distinct(), samples);

            Assert.Throws<ArgumentOutOfRangeException>(
                () => Sampling.SampleWithOutReplacement(set, 100));
        }

        [Fact]
        public void TestSampleOneItem() {
            var set = new Set<int> {1, 4, 8, 5, 9, 7};
            var samples = Sampling.Sample(set, 3);

            Assert.Equal(samples.Count, 3);

            Assert.Throws<ArgumentOutOfRangeException>(() => Sampling.Sample(set, 100));
        }

        [Fact]
        public void TestSampleNumberOfItems() {
            var set = new Set<int> {1, 4, 8, 5, 9, 7};
            var sample = Sampling.Sample(set);

            Assert.Contains(sample, set);
        }
    }
}