using System;
using System.Collections.Generic;
using System.Linq;

namespace ClusteringAlgorithm {
    public class KMeans<T> {
        private readonly Func<ObservationSet<T>, T> _centroidFunc;
        private readonly ObservationSet<T> _observationSet;
        private readonly Func<T, T, double> _distanceFunc;

        public KMeans(ObservationSet<T> observationSet, Func<T, T, double> distanceFunc,
            Func<ObservationSet<T>, T> centroidFunc) {
            _observationSet = observationSet;
            _distanceFunc = distanceFunc;
            _centroidFunc = centroidFunc;
        }

        /// <summary>
        /// 进行聚类划分
        /// </summary>
        /// <param name="categoriesCount">聚类数目</param>
        /// <param name="precision">迭代精度</param>
        /// <returns></returns>
        public CategorySet<T> Classify(int categoriesCount, double precision = 0.01) {
            ValidateArgument(categoriesCount, precision);

            var categorySet = new CategorySet<T>(_distanceFunc, _centroidFunc);
            SetRandomCentroids(categorySet, categoriesCount);

            ICollection<double> centroidErrors;
            do {
                categorySet.ClearAllObservations();
                categorySet.Classify(_observationSet);
                categorySet.UpdateCentroids(out centroidErrors);
            } while (centroidErrors.Max() > precision);

            return categorySet;
        }
        private void ValidateArgument(int categoriesCount, double precision) {
            if (categoriesCount > _observationSet.Count() || categoriesCount < 1)
                throw new ArgumentOutOfRangeException(
                    $"categories number overflow: {categoriesCount}");
            if (precision <= 0)
                throw new ArgumentOutOfRangeException($"Invalid {nameof(precision)}: {precision}");
        }
        private void SetRandomCentroids(CategorySet<T> categorySet, int categoriesCount) {
            var centroidSet = _observationSet.Distinct().SamplingWithoutRepeating(categoriesCount);

            foreach (var centroid in centroidSet)
                categorySet.Add(new Category<T>(centroid));
        }
    }
}