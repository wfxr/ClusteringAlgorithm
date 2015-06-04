using System.Collections.Generic;

namespace ClusteringAlgorithm {
    public class Category<T>{
        public Category(double centroid) { Centroid = centroid; }
        public double Centroid;

        public virtual void UpdateCentroid() {}
        public void ClearObservations() => Observations = new List<Observation<T>>();
        public List<Observation<T>> Observations { get; set; } = new List<Observation<T>>();
    }
}