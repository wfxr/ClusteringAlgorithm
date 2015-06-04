using System;
using System.Collections.Generic;
using System.Linq;

namespace ClusteringAlgorithm {
    public class KMeans {
        private readonly List<int> _observations;
        public KMeans(List<int> observations) { _observations = observations; }

        public List<Category> Classify(int categoriesNumber, int iterations = 10) {
            if (categoriesNumber > _observations.Count || categoriesNumber < 1)
                throw new ArgumentOutOfRangeException(
                    $"categories number overflow: {categoriesNumber}");
            if (iterations < 1)
                throw new ArgumentOutOfRangeException($"Invalid iterations: {iterations}");

            var categories = new List<Category>();

            InitCategories(categories, _observations, categoriesNumber);
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

        private void Classify(IEnumerable<int> observations, List<Category> categories) {
            foreach (var observation in observations)
            {
                var nearestCategory = FindNearestCategory(categories, observation);
                nearestCategory.Observations.Add(observation);
            }
        }

        private static void InitCategories(List<Category> categories, List<int> observations,
            int categoriesNumber) {
            var r = new Random();
            var count = observations.Count;
            var selected = new List<int>();
            for (int i = 0; i < categoriesNumber; ++i) {
                var num = r.Next(0, count);
                if (selected.Contains(num)) {
                    --i;
                    continue;
                }
                selected.Add(num);
            }

            foreach (var num in selected) {
                categories.Add(new Category {Centroid = observations[num]});
            }
        }

        private Category FindNearestCategory(List<Category> categories, int observation) {
            var minDistance = double.MaxValue;
            Category nearestCategory = null;
            foreach (var category in categories) {
                var distance = CalcDistance(observation, category.Centroid);
                if (!(minDistance > distance)) continue;
                minDistance = distance;
                nearestCategory = category;
            }
            return nearestCategory;
        }

        private static double CalcDistance(int observation, double centroid)
            => Math.Abs(observation - centroid);
    }

    public class Category {
        public double Centroid;
        public void UpdateCentroid() => Centroid = Observations.Average();
        public void ClearObservations() => Observations.Clear();
        public List<int> Observations { get; } = new List<int>();
    }
}