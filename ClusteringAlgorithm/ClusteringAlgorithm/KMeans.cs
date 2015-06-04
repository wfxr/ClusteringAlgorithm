using System;
using System.Collections.Generic;
using System.Linq;

namespace ClusteringAlgorithm {
    public class KMeans<T> {
        public delegate T AverageDelegate(ObservationSet<T> observationSet);

        public delegate double DistanceDelegate(T obs1, T obs2);

        private readonly AverageDelegate _averageDelegate;
        private readonly DistanceDelegate _distanceDelegate;
        private readonly ObservationSet<T> _observationSet;

        public KMeans(ObservationSet<T> observationSet, DistanceDelegate distanceDelegate,
            AverageDelegate averageDelegate) {
            _observationSet = observationSet;
            _distanceDelegate = distanceDelegate;
            _averageDelegate = averageDelegate;
        }

        private double Distance(T obs1, T obs2) => _distanceDelegate(obs1, obs2);

        private double Distance(T obs, Category<T> category)
            => _distanceDelegate(obs, category.Centroid);

        private T Average(ObservationSet<T> observationSet) => _averageDelegate(observationSet);

        public List<Category<T>> Classify(int categoriesCount, int iterations = 10) {
            ValidateArgument(categoriesCount, iterations);

            var categories = new List<Category<T>>();
            SetRandomCentroids(categories, categoriesCount);

            while (iterations-- > 0) {
                ClassifyObservations(categories);
                ComputeCentroids(categories);
            }

            return categories.OrderBy(category => category.Centroid).ToList();
        }

        private void ComputeCentroids(IEnumerable<Category<T>> categories) {
            foreach (var category in categories)
                category.SetCentroid(Average(category.Observations));
        }

        private void ValidateArgument(int categoriesNumber, int iterations) {
            if (categoriesNumber > _observationSet.Count() || categoriesNumber < 1)
                throw new ArgumentOutOfRangeException(
                    $"categories number overflow: {categoriesNumber}");
            if (iterations < 1)
                throw new ArgumentOutOfRangeException($"Invalid iterations: {iterations}");
        }

        private void ClassifyObservations(List<Category<T>> categories) {
            // 清除先前分组
            foreach (var category in categories)
                category.ClearObservations();
            // 重新分组
            foreach (var observation in _observationSet) {
                var nearestCategory = FindNearestCategory(categories, observation);
                nearestCategory.Observations.Add(observation);
            }
        }

        private void SetRandomCentroids(ICollection<Category<T>> categories, int categoriesCount) {
            var r = new Random();
            var centroids = new List<T>();
            while (centroids.Count < categoriesCount) {
                var observation = _observationSet[r.Next(0, _observationSet.Count)];
                if (!centroids.Contains(observation))
                    centroids.Add(observation);
            }

            foreach (var centroid in centroids)
                categories.Add(new Category<T> {Centroid = centroid});
        }

        /// <summary>
        ///     将观测值进行随机分组
        /// </summary>
        /// <param name="categories"></param>
        /// <param name="categoriesCount"></param>
        private void RandomPartation(IList<Category<T>> categories, int categoriesCount) {
            while (categories.Count < categoriesCount)
                categories.Add(new Category<T>());
            var r = new Random();
            var perCategoryCount = _observationSet.Count/categoriesCount;
            foreach (var observation in _observationSet) {
                do {
                    var randomCategory = categories[r.Next(0, categoriesCount)];
                    if (randomCategory.Count < perCategoryCount) {
                        randomCategory.Add(observation);
                        break;
                    }
                } while (true);
            }
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