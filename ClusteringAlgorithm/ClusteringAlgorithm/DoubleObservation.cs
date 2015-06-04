using System;

namespace ClusteringAlgorithm {
    public class DoubleObservation : Observation<double> {
        public override double ToDouble => Value;
        public override double DistanceTo(Observation<double> obs) => Math.Abs(Value - obs.Value);
        public override double DistanceTo(Category<double> category) => Math.Abs(Value - category.Centroid);
        public DoubleObservation(double value) : base(value) {}
    }
}