using System;
using ClusteringAlgorithm;
using MathNet.Numerics.LinearAlgebra.Double;

namespace RunoffsClassify {
    internal class Program {
        private static void Main(string[] args) {
            var data = DenseMatrix.OfArray(new double[,] {
                {
                    21105, 28013, 23900, 17330, 23882, 18710, 16024, 21172, 16931, 22831, 20178,
                    21604, 29523, 23251, 15040, 22498, 16900, 19399, 17016, 16044, 14991, 25099,
                    16632, 16875, 17200, 19227, 18212, 21789, 19427, 18522, 14759, 16078, 20643,
                    16979, 22407, 18135, 21483, 23069, 22412, 15294, 23216, 14688, 18790, 18780,
                    17259, 25747, 23452, 22394, 22198, 16990, 22009, 20732, 22219, 14331, 15607,
                    19686, 18260, 17281, 13947, 21433
                }
            });
            var kmeans = new Kmeans(data.Transpose());
            var c = 3; // 聚类数目
            var result = kmeans.Clustering(c);
            Console.WriteLine("聚类中心值:");
            Console.WriteLine(result.Center.Transpose().ToMatrixString());
            Console.WriteLine("隶属向量");
            Console.WriteLine(result.U.ToVectorString(4, 80));
            Console.WriteLine("聚类结果");

            for (var i = 0; i < c; ++i) {
                Console.Write("聚类中心:{0}",result.Center.Row(i).ToVectorString());
                var cluster = result.Clusters[i];
                for (var j = 0; j < cluster.Count; ++j) {
                    Console.Write($"{cluster[j][0]}    ");
                    if (j%10 == 9)
                        Console.WriteLine();
                }
                Console.WriteLine();
                Console.WriteLine();
            }
        }
    }
}