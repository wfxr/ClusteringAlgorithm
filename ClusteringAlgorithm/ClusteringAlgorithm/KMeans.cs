using System;
using System.Collections.Generic;
using System.Linq;
using Wfxr.Statistics;
// ReSharper disable InconsistentNaming

namespace ClusteringAlgorithm {
    public class Kmeans<T> {
        private readonly Func<ICollection<T>, T> _centroidFunc; // 计算分类中心的委托
        private readonly Func<T, T, double> _distanceFunc; // 计算观测值距离的委托
        private readonly List<T> _X; // 无重复的观察值集合 
        private readonly IEnumerable<T> _XAll; // 观测值集合

        public Kmeans(IEnumerable<T> X, Func<T, T, double> distanceFunc,
            Func<ICollection<T>, T> centroidFunc) {
            _XAll = X;
            _distanceFunc = distanceFunc;
            _centroidFunc = centroidFunc;
            _X = _XAll.Distinct().ToList();
        }

        public Kmeans(IEnumerable<T> X, Func<T, T, double> distanceFunc,
            Func<T, T, T> sumFunc, Func<T, double, T> divFunc)
            : this(X, distanceFunc, set => divFunc(set.Aggregate(sumFunc), set.Count)) {}

        /// <summary>
        ///     对观测值集合进行聚类划分
        /// </summary>
        /// <param name="K">聚类数目</param>
        /// <param name="precision">迭代精度</param>
        /// <returns>观察值的聚类集合</returns>
        public CategoryList<T> Cluster(int K, double precision = 0.01) {
            ValidateArgument(K, precision);

            var categoryList = CreateCategoryListWithyRandomCentroids(K);

            List<double> distanceOffsets;
            do {
                categoryList.Classify(_X);
                categoryList.UpdateAllCentroids(out distanceOffsets);
                categoryList.ClearAllObservations();
            } while (distanceOffsets.Max() > precision);

            // 注意:此处重新对**所有**观测值分类
            categoryList.Classify(_XAll);
            return categoryList;
        }

        /// <summary>
        ///     验证参数的合理性
        /// </summary>
        /// <param name="K">聚类数目</param>
        /// <param name="precision">迭代精度</param>
        private void ValidateArgument(int K, double precision) {
            if (K > _X.Count() || K < 1)
                throw new ArgumentOutOfRangeException(
                    $"categories number({K}) \">\" distinct observations number({_X.Count})");
            if (precision <= 0)
                throw new ArgumentOutOfRangeException($"Invalid {nameof(precision)}: {precision}");
        }

        /// <summary>
        ///     通过随机抽样的方式设置聚类的中心
        /// </summary>
        /// <param name="K">聚类数目</param>
        private CategoryList<T> CreateCategoryListWithyRandomCentroids(int K) {
            var categoryList = new CategoryList<T>(_distanceFunc, _centroidFunc);
            var centroids = Sampling.SampleWithOutReplacement(_X, K);

            centroids.ForEach(centroid => categoryList.Add(new Category<T> {Centroid = centroid}));
            return categoryList;
        }
    }
}