using System;
using System.Collections.Generic;
using System.Linq;

namespace ClusteringAlgorithm {
    public class KMeans<T> {
        public delegate T CentroidDelegate(ObservationSet<T> observationSet);

        public delegate double DistanceDelegate(T obs1, T obs2);

        private readonly CentroidDelegate _centroidDelegate;
        private readonly DistanceDelegate _distanceDelegate;
        private readonly ObservationSet<T> _observationSet;

        public KMeans(ObservationSet<T> observationSet, DistanceDelegate distanceDelegate,
            CentroidDelegate centroidDelegate) {
            _observationSet = observationSet;
            _distanceDelegate = distanceDelegate;
            _centroidDelegate = centroidDelegate;
        }

        private double Distance(T obs1, T obs2) => _distanceDelegate(obs1, obs2);
        private double Distance(T obs, Category<T> category) => Distance(obs, category.Centroid);

        private T ComputeCentroid(ObservationSet<T> observationSet)
            => _centroidDelegate(observationSet);

        public CategorySet<T> Classify(int categoriesCount, double precision = 0.01) {
            ValidateArgument(categoriesCount, precision);

            var categorySet = new CategorySet<T>();
            SetRandomCentroids(categorySet, categoriesCount);

            ICollection<double> centroidErrors;
            do {
                ClassifyObservations(categorySet);
                UpdateCentroids(categorySet, out centroidErrors);
            } while (centroidErrors.Max() > precision);

            return categorySet;
        }

        private void UpdateCentroids(CategorySet<T> categorySet,
            out ICollection<double> centroidErrors) {
            centroidErrors = new List<double>();
            foreach (var category in categorySet) {
                var oldCentroid = category.Centroid;
                category.SetCentroid(ComputeCentroid(category.Observations));
                var newCentroid = category.Centroid;
                centroidErrors.Add(Distance(oldCentroid, newCentroid));
            }
        }

        private void ValidateArgument(int categoriesCount, double precision) {
            if (categoriesCount > _observationSet.Count() || categoriesCount < 1)
                throw new ArgumentOutOfRangeException(
                    $"categories number overflow: {categoriesCount}");
            if (precision <= 0)
                throw new ArgumentOutOfRangeException($"Invalid {nameof(precision)}: {precision}");
        }

        private void ClassifyObservations(CategorySet<T> categorySet) {
            // 清除之前的分组
            foreach (var category in categorySet)
                category.ClearObservations();
            // 重新分组
            foreach (var observation in _observationSet) {
                var nearestCategory = NearestCategory(categorySet, observation);
                nearestCategory.Observations.Add(observation);
            }
        }

        private void SetRandomCentroids(CategorySet<T> categorySet, int categoriesCount) {
            var centroidSet = _observationSet.Distinct().RandomSample(categoriesCount);

            foreach (var centroid in centroidSet)
                categorySet.Add(new Category<T>(centroid));
        }


        private Category<T> NearestCategory(CategorySet<T> categorySet, T observation) {
            var minDistance = double.MaxValue;
            Category<T> nearestCategory = null;
            foreach (var category in categorySet) {
                var distance = Distance(observation, category);
                if (!(minDistance > distance)) continue;
                minDistance = distance;
                nearestCategory = category;
            }
            return nearestCategory;
        }
    }
}