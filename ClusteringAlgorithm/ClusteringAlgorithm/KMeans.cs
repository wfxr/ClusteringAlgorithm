using System;
using System.Collections.Generic;
using System.Linq;
using Wfxr.Statistics;

namespace ClusteringAlgorithm {
    public class Kmeans<T> {
        private readonly Func<List<T>, T> _centroidFunc; // 计算分类中心的委托
        private readonly Func<T, T, double> _distanceFunc; // 计算观测值距离的委托
        private readonly List<T> _observations; // 观测值集合

        public Kmeans(List<T> observations, Func<T, T, double> distanceFunc,
            Func<List<T>, T> centroidFunc) {
            _observations = observations;
            _distanceFunc = distanceFunc;
            _centroidFunc = centroidFunc;
        }

        public Kmeans(List<T> observations, Func<T, T, double> distanceFunc, Func<T, T, T> sumFunc,
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

            var categoryList = new CategorySet<T>(_distanceFunc, _centroidFunc);
            ListRandomCentroids(categoryList, categoriesCount);

            List<double> distanceOffsets;
            do {
                categoryList.ClearAllObservations();
                categoryList.Classify(_observations);
                categoryList.UpdateAllCentroids(out distanceOffsets);
            } while (distanceOffsets.Max() > precision);

            return categoryList;
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
        /// <param name="categoryList"></param>
        /// <param name="categoriesCount"></param>
        private void ListRandomCentroids(CategorySet<T> categoryList, int categoriesCount) {
            // 先去重复再抽样，否则可能取到重复的中心
            var centroids = Sampling.SampleWithOutReplacement(_observations.Distinct(),
                categoriesCount);

            centroids.ForEach(centroid => categoryList.Add(new Category<T>(centroid)));
        }
    }
}