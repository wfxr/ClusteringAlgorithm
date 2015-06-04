using System;

namespace ClusteringAlgorithm {
    public class IntObervation : Observation<int> {
        public override double ToDouble => Value;
        public override int DistanceTo(Observation<int> obs) => Math.Abs(Value - obs.Value);
        public override double DistanceTo(Category<int> category) => Math.Abs(Value - category.Centroid);
        public IntObervation(int value) : base(value) {}
        public static implicit operator IntObervation(int value) => new IntObervation(value);
    }
}