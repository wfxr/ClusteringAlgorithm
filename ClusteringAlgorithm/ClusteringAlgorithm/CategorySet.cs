using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace ClusteringAlgorithm
{
    public class CategorySet<T> :IEnumerable<Category<T>>{
        private readonly List<Category<T>> _categories;
        public CategorySet() { _categories = new List<Category<T>>(); }

        public CategorySet(IEnumerable<Category<T>> observations) {
            _categories = observations.ToList();
        }

        public int Count => _categories.Count;
        public IEnumerable<T> Centroids() => _categories.Select(category => category.Centroid);
        public List<Category<T>> ToList() => _categories.ToList();
        public void Add(Category<T> observation) => _categories.Add(observation);

        public Category<T> this[int i] {
            get { return _categories[i]; }
            set { _categories[i] = value; }
        }

        public CategorySet<T> Distinct() => new CategorySet<T>(_categories.Distinct());
        public void Sort() => _categories.Sort();
        public void Sort(Comparison<Category<T>> comparison) => _categories.Sort(comparison);
        public IEnumerator<Category<T>> GetEnumerator() { return _categories.GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
    }
}
