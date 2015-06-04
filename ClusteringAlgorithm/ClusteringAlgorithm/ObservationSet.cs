using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ClusteringAlgorithm {
    public class ObservationSet<T> : IEnumerable<T>{
        private readonly List<T> _observations;
        public ObservationSet() { _observations = new List<T>(); }

        public ObservationSet(IEnumerable<T> observations) {
            _observations = observations.ToList();
        }
        public int Count => _observations.Count;
        public List<T> ToList() => _observations.ToList();
        public IEnumerator<T> GetEnumerator() { return _observations.GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
        public void Add(T observation) => _observations.Add(observation);
        public T this[int i] {
            get { return _observations[i]; }
            set { _observations[i] = value; }
        }
    }
}