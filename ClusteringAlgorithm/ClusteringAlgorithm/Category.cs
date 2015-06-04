namespace ClusteringAlgorithm {
    public class Category<T>{
        public Category(T centroid) {
            Centroid = centroid; 
            Observations = new ObservationSet<T>();
        }
        public T Centroid;

        //public T Average(ObservationSet<T> observationSet) {

        //}
        //public void UpdateCentroid() => Centroid = Average(Observations);
        public void ClearObservations() => Observations = new ObservationSet<T>();
        public ObservationSet<T> Observations { get; set; }
    }
}