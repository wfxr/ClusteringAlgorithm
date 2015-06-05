namespace ClusteringAlgorithm {
    public class Category<T> {
        public T Centroid;
        public Category() { Observations = new ObservationSet<T>(); }
        public Category(T centroid) : this() { Centroid = centroid; } 

        public ObservationSet<T> Observations { get; set; }
        public void SetCentroid(T centroid) => Centroid = centroid;
        public void Add(T observation) => Observations.Add(observation);
        public int Count => Observations.Count;
        public void ClearObservations() => Observations = new ObservationSet<T>();
    }
}