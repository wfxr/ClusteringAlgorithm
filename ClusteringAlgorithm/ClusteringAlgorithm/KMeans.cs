using System;
using System.Collections.Generic;
using System.Linq;

namespace ClusteringAlgorithm {
    public class KMeans<T> {
        public delegate double DistanceDelegate(T obs1, T obs2);
        public delegate T AverageDelegate(ObservationSet<T> observationSet);

        private readonly ObservationSet<T> _observationSet;
        private readonly DistanceDelegate _distanceDelegate;
        private readonly AverageDelegate _averageDelegate;

        public KMeans(ObservationSet<T> observationSet, DistanceDelegate distanceDelegate, AverageDelegate averageDelegate) {
            _observationSet = observationSet;
            _distanceDelegate = distanceDelegate;
            _averageDelegate = averageDelegate;
        }

        private double Distance(T obs1, T obs2) => _distanceDelegate(obs1, obs2);

        private double Distance(T obs, Category<T> category)
            => _distanceDelegate(obs, category.Centroid);

        private T Average(ObservationSet<T> observationSet) => _averageDelegate(observationSet);

        public List<Category<T>> Classify(int categoriesNumber, int iterations = 10) {
            Validate(categoriesNumber, iterations);

            var categories = new List<Category<T>>();

            InitCategories(categories, categoriesNumber);
            Classify(_observationSet, categories);

            while (iterations-- > 0) {
                foreach (var category in categories) {
                    category.Centroid = Average(category.Observations);
                    category.ClearObservations();
                }
                Classify(_observationSet, categories);
            }
            return categories.OrderBy(category => category.Centroid).ToList();
        }

        private void Validate(int categoriesNumber, int iterations) {
            if (categoriesNumber > _observationSet.Count() || categoriesNumber < 1)
                throw new ArgumentOutOfRangeException(
                    $"categories number overflow: {categoriesNumber}");
            if (iterations < 1)
                throw new ArgumentOutOfRangeException($"Invalid iterations: {iterations}");
        }

        private void Classify(ObservationSet<T> observations, List<Category<T>> categories) {
            foreach (var observation in observations) {
                var nearestCategory = FindNearestCategory(categories, observation);
                nearestCategory.Observations.Add(observation);
            }
        }

        private void InitCategories(ICollection<Category<T>> categories, int categoriesNumber) {
            var r = new Random();
            var centroids = new List<T>();
            while (centroids.Count < categoriesNumber) {
                var obsValue = _observationSet[r.Next(0, _observationSet.Count)];
                if (!centroids.Contains(obsValue))
                    centroids.Add(obsValue);
            }

            foreach (var centroid in centroids)
                categories.Add(new Category<T>(centroid));
        }

        private Category<T> FindNearestCategory(List<Category<T>> categories, T observation) {
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