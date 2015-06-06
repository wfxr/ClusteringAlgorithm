using System;
using System.Linq;
using ClusteringAlgorithm.Algorithms;
using Wfxr.Container;

namespace RunoffsClassify {
    internal class Program {
        private static void Main(string[] args) {
            var runoffSet = new Set<double> {
                21105, 28013, 23900, 17330, 23882, 18710, 16024, 21172, 16931, 22831,
                20178, 21604, 29523, 23251, 15040, 22498, 16900, 19399, 17016, 16044,
                14991, 25099, 16632, 16875, 17200, 19227, 18212, 21789, 19427, 18522,
                14759, 16078, 20643, 16979, 22407, 18135, 21483, 23069, 22412, 15294,
                23216, 14688, 18790, 18780, 17259, 25747, 23452, 22394, 22198, 16990,
                22009, 20732, 22219, 14331, 15607, 19686, 18260, 17281, 13947, 21433
            };
            var clustering = new KMeans<double>(runoffSet, (obs1, obs2) => Math.Abs(obs1 - obs2),
                set => set.Average());
            var categories = clustering.Classify(5);
            foreach (var category in categories) {
                Console.Write("观测值数目：");
                Console.WriteLine(category.Count);
                Console.Write("聚类中心值：");
                Console.WriteLine((int)category.Centroid);
                Console.WriteLine("径流值：");
                foreach (var obs in category) {
                    Console.Write(obs + "\t");
                }
                Console.WriteLine();
                Console.WriteLine();
            }
        }
    }
}