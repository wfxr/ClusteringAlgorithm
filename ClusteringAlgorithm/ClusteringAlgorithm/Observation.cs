namespace ClusteringAlgorithm {
    public abstract class Observation<T>
    {
        protected Observation(T value) { Value = value; } 
        public T Value { get; set; }
        public abstract double ToDouble { get; }
        public abstract T DistanceTo(Observation<T> obs);
        public abstract double DistanceTo(Category<T> category);
    }
}