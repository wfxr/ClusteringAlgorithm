using System;
using System.Collections.Generic;
using System.Linq;

namespace ClusteringAlgorithm {
    public class KMeans<T>{
        private readonly IEnumerable<Observation<T>> _observations;
        public KMeans(IEnumerable<Observation<T>> observations) { _observations = observations; }

        public List<Category<T>> Classify(int categoriesNumber, int iterations = 10) {
            if (categoriesNumber > _observations.Count() || categoriesNumber < 1)
                throw new ArgumentOutOfRangeException(
                    $"categories number overflow: {categoriesNumber}");
            if (iterations < 1)
                throw new ArgumentOutOfRangeException($"Invalid iterations: {iterations}");

            var categories = new List<Category<T>>();

            InitCategories(categories, categoriesNumber);
            Classify(_observations, categories);

            while (iterations-- > 0) {
                foreach (var category in categories) {
                    category.UpdateCentroid();
                    category.ClearObservations();
                }
                Classify(_observations, categories);
            }
            return categories.OrderBy(category => category.Centroid).ToList();
        }

        private void Classify(IEnumerable<Observation<T>> observations, List<Category<T>> categories) {
            foreach (var observation in observations) {
                var nearestCategory = FindNearestCategory(categories, observation);
                nearestCategory.Observations.Add(observation);
            }
        }

        private void InitCategories(ICollection<Category<T>> categories, int categoriesNumber) {
            var r = new Random();
            var observations = _observations.ToList();
            var centroids = new List<double>();
            while (centroids.Count < categoriesNumber) {
                var obsValue = observations[r.Next(0, observations.Count)].ToDouble;
                if (!centroids.Contains(obsValue))
                    centroids.Add(obsValue);
            }

            foreach (var centroid in centroids)
                categories.Add(new Category<T>(centroid));
        }

        private static Category<T> FindNearestCategory(List<Category<T>> categories, Observation<T> observation) {
            var minDistance = double.MaxValue;
            Category<T> nearestCategory = null;
            foreach (var category in categories) {
                var distance = observation.DistanceTo(category);
                if (!(minDistance > distance)) continue;
                minDistance = distance;
                nearestCategory = category;
            }
            return nearestCategory;
        }

        private static double CalcDistance(int observation, double centroid)
            => Math.Abs(observation - centroid);
    }
}