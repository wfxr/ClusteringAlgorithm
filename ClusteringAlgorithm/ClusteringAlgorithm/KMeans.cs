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

        public List<Category<T>> Classify(int categoriesCount, double precision = 0.01) {
            ValidateArgument(categoriesCount, precision);

            var categories = new List<Category<T>>();
            SetRandomCentroids(categories, categoriesCount);

            ICollection<double> errs;
            do {
                ClassifyObservations(categories);
                UpdateCentroids(categories, out errs);
            } while (errs.Max() > precision);

            return categories.OrderBy(category => category.Centroid).ToList();
        }

        private void UpdateCentroids(IEnumerable<Category<T>> categories,
            out ICollection<double> errs) {
            errs = new List<double>();
            foreach (var category in categories) {
                var oldCentroid = category.Centroid;
                category.SetCentroid(ComputeCentroid(category.Observations));
                var newCentroid = category.Centroid;
                errs.Add(Distance(oldCentroid, newCentroid));
            }
        }

        private void ValidateArgument(int categoriesNumber, double precision) {
            if (categoriesNumber > _observationSet.Count() || categoriesNumber < 1)
                throw new ArgumentOutOfRangeException(
                    $"categories number overflow: {categoriesNumber}");
            if (precision <= 0)
                throw new ArgumentOutOfRangeException($"Invalid {nameof(precision)}: {precision}");
        }

        private void ClassifyObservations(List<Category<T>> categories) {
            // 清除先前分组
            foreach (var category in categories)
                category.ClearObservations();
            // 重新分组
            foreach (var observation in _observationSet) {
                var nearestCategory = NearestCategory(categories, observation);
                nearestCategory.Observations.Add(observation);
            }
        }

        private void SetRandomCentroids(ICollection<Category<T>> categories, int categoriesCount) {
            var centroidSet = ObservationSet<T>.RandomSample(_observationSet.Distinct(), categoriesCount);

            foreach (var centroid in centroidSet)
                categories.Add(new Category<T> (centroid));
        }


        private Category<T> NearestCategory(List<Category<T>> categories, T observation) {
            var minDistance = double.MaxValue;
            Category<T> nearestCategory = null;
            foreach (var category in categories) {
                var distance = Distance(observation, category);
                if (!(minDistance > distance)) continue;
                minDistance = distance;
                nearestCategory = category;
            }
            return nearestCategory;
        }
    }
}