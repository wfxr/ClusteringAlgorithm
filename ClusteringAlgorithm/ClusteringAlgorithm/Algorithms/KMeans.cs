using System;
using System.Collections.Generic;
using System.Linq;
using Wfxr.Container;
using Wfxr.Statistics;

namespace ClusteringAlgorithm.Algorithms {
    public class KMeans<T> {
        private readonly Func<Set<T>, T> _centroidFunc; // 计算分类中心的委托
        private readonly Func<T, T, double> _distanceFunc; // 计算观测值距离的委托
        private readonly Set<T> _observations; // 观测值集合

        public KMeans(Set<T> observations, Func<T, T, double> distanceFunc,
            Func<Set<T>, T> centroidFunc) {
            _observations = observations;
            _distanceFunc = distanceFunc;
            _centroidFunc = centroidFunc;
        }

        public KMeans(Set<T> observations, Func<T, T, double> distanceFunc, Func<T, T, T> sumFunc,
            Func<T, double, T> divFunc)
            : this(observations, distanceFunc, set => divFunc(set.Aggregate(sumFunc), set.Count))
        { }

        /// <summary>
        ///     进行聚类划分
        /// </summary>
        /// <param name="categoriesCount">聚类数目</param>
        /// <param name="precision">迭代精度</param>
        /// <returns></returns>
        public CategorySet<T> Classify(int categoriesCount, double precision = 0.01) {
            ValidateArgument(categoriesCount, precision);

            var categorySet = new CategorySet<T>(_distanceFunc, _centroidFunc);
            SetRandomCentroids(categorySet, categoriesCount);

            List<double> centroidErrors;
            do {
                categorySet.ClearAllCategories();
                categorySet.Classify(_observations);
                categorySet.UpdateAllCentroids(out centroidErrors);
            } while (centroidErrors.Max() > precision);

            return categorySet;
        }

        private void ValidateArgument(int categoriesCount, double precision) {
            if (categoriesCount > _observations.Count() || categoriesCount < 1)
                throw new ArgumentOutOfRangeException(
                    $"categories number overflow: {categoriesCount}");
            if (precision <= 0)
                throw new ArgumentOutOfRangeException($"Invalid {nameof(precision)}: {precision}");
        }

        /// <summary>
        ///     通过随机抽样的方式设置聚类的中心
        /// </summary>
        /// <param name="categorySet"></param>
        /// <param name="categoriesCount"></param>
        private void SetRandomCentroids(CategorySet<T> categorySet, int categoriesCount) {
            // 先去重复再抽样，否则可能取到重复的中心
            var centroids = Sampling.SampleWithOutReplacement(_observations.Distinct(),
                categoriesCount);

            centroids.ForEach(centroid => categorySet.Add(new Category<T>(centroid)));
        }
    }
}