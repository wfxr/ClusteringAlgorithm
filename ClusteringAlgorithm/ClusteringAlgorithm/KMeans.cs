using System;
using System.Collections.Generic;
using System.Linq;
using Wfxr.Statistics;

namespace ClusteringAlgorithm {
    public class Kmeans<T> {
        private readonly Func<List<T>, T> _centroidFunc; // 计算分类中心的委托
        private readonly Func<T, T, double> _distanceFunc; // 计算观测值距离的委托
        private readonly List<T> _observationDistinct; // 无重复的观察值集合 
        private readonly IEnumerable<T> _observationsAll; // 观测值集合

        public Kmeans(IEnumerable<T> observations, Func<T, T, double> distanceFunc,
            Func<List<T>, T> centroidFunc) {
            _observationsAll = observations;
            _distanceFunc = distanceFunc;
            _centroidFunc = centroidFunc;
            _observationDistinct = _observationsAll.Distinct().ToList();
        }

        public Kmeans(IEnumerable<T> observations, Func<T, T, double> distanceFunc,
            Func<T, T, T> sumFunc, Func<T, double, T> divFunc)
            : this(observations, distanceFunc, set => divFunc(set.Aggregate(sumFunc), set.Count)) {}

        /// <summary>
        ///     进行聚类划分
        /// </summary>
        /// <param name="categoriesCount">聚类数目</param>
        /// <param name="precision">迭代精度</param>
        /// <returns>观察值的聚类集合</returns>
        public CategorySet<T> Classify(int categoriesCount, double precision = 0.01) {
            ValidateArgument(categoriesCount, precision);

            var categorySet = new CategorySet<T>(_distanceFunc, _centroidFunc);
            SetRandomCentroids(categorySet, categoriesCount);

            List<double> distanceOffsets;
            do {
                categorySet.Classify(_observationDistinct);
                categorySet.UpdateAllCentroids(out distanceOffsets);
                categorySet.ClearAllObservations();
            } while (distanceOffsets.Max() > precision);

            // 注意:此处重新对*所有*观测值分类
            categorySet.Classify(_observationsAll);
            return categorySet;
        }

        private void ValidateArgument(int categoriesCount, double precision) {
            if (categoriesCount > _observationDistinct.Count() || categoriesCount < 1)
                throw new ArgumentOutOfRangeException(
                    $"categories number({categoriesCount}) \">\" distinct observations number({_observationDistinct.Count})");
            if (precision <= 0)
                throw new ArgumentOutOfRangeException($"Invalid {nameof(precision)}: {precision}");
        }

        /// <summary>
        ///     通过随机抽样的方式设置聚类的中心
        /// </summary>
        /// <param name="categoryList"></param>
        /// <param name="categoriesCount"></param>
        private void SetRandomCentroids(CategorySet<T> categoryList, int categoriesCount) {
            var centroids = Sampling.SampleWithOutReplacement(_observationDistinct, categoriesCount);

            centroids.ForEach(centroid => categoryList.Add(new Category<T> {Centroid = centroid}));
        }
    }
}