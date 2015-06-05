using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ClusteringAlgorithm {
    public class ObservationSet<T> : IEnumerable<T> {
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

        public ObservationSet<T> Distinct() => new ObservationSet<T>(_observations.Distinct());

        public static IEnumerable<T> RandomSample(ObservationSet<T> observationSet, int count) {
            var r = new Random();
            var ret = new ObservationSet<T>();
            while (count > 0) {
                var obs = observationSet[r.Next(0, observationSet.Count)];
                if (!ret.Contains(obs)) {
                    ret.Add(obs);
                    count--;
                }
            }
            return ret;
        }

        public static T RandomSample(ObservationSet<T> observationSet, Random r)
            => observationSet[r.Next(0, observationSet.Count)];

        public static T RandomSample(ObservationSet<T> observationSet)
            => observationSet[new Random().Next(0, observationSet.Count)];
    }
}