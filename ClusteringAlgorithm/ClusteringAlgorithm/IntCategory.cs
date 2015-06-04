using System.Linq;

namespace ClusteringAlgorithm {
    public class IntCategory : Category<int> {
        public IntCategory(double centroid) : base(centroid) { }

        public override void UpdateCentroid()
            => Centroid = Observations.Select(obs => obs.Value).Average();
    }
}