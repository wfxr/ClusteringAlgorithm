using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ClusteringAlgorithm {
    public class KMeans {
        private List<Observation> _observation;
        public KMeans(List<Observation> observation) { _observation = observation; }

        public KMeans(List<int> observation) {
            _observation = new List<Observation>();
            foreach (var value in observation)
                _observation.Add(new Observation {Value = value});
        }

        public List<Category> Classify(int categoriesNumber, int iterations = 10) {
            if (categoriesNumber > _observation.Count || categoriesNumber < 1)
                throw new ArgumentOutOfRangeException(
                    $"categories number overflow: {categoriesNumber}");
            if (iterations < 1)
                throw new ArgumentOutOfRangeException($"Invalid iterations: {iterations}");

            var categories = new List<Category>();

            InitCategories(categories, _observation, categoriesNumber);

            foreach (var observation in _observation) {
                var nearestCategory = FindNearestCategory(categories, observation);
                observation.Category = nearestCategory;
            }
            return categories.OrderBy(category => category.Centroid).ToList();
        }

        private static void InitCategories(List<Category> categories, List<Observation> observations,
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
                categories.Add(new Category {Centroid = observations[num].Value});
            }
        }

        private Category FindNearestCategory(List<Category> categories, Observation observation) {
            var minDistance = double.MaxValue;
            Category nearestCategory = null;
            foreach (var category in categories) {
                var distance = CalcDistance(observation.Value, category.Centroid);
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
        public void UpdateCentroid() => Centroid = Observations.Select(o => o.Value).Average();
        public List<Observation> Observations { get; } = new List<Observation>();
    }

    public class Observation {
        public int Value;
        public Category Category;
    }
}