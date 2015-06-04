using System.Linq;

namespace ClusteringAlgorithm {
    public class DoubleCategory : Category<double> {
        public DoubleCategory(double centroid) : base(centroid) { }

        public override void UpdateCentroid()
            => Centroid = Observations.Select(obs => obs.Value).Average();
    }
}