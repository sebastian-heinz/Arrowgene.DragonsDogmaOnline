using System;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Utils
{
    public class FuzzySearch
    {
        private static List<string> GetTrigrams(string s)
        {
            s = s.ToLower().Trim();
            if (s.Length < 3)
            {
                return new List<string> { s.PadRight(3, ' ') };
            }

            var trigrams = new List<string>();
            for (int i = 0; i <= s.Length - 3; i++)
            {
                trigrams.Add(s.Substring(i, 3));
            }
            return trigrams;
        }

        private static double JaroWinklerDistance(string s1, string s2, double p = 0.1, int maxL = 4)
        {
            s1 = s1.ToLower();
            s2 = s2.ToLower();
            int len1 = s1.Length;
            int len2 = s2.Length;

            if (len1 == 0 || len2 == 0)
            {
                return 0.0;
            }

            int maxDist = Math.Max(len1, len2) / 2 - 1;
            int m = 0;
            int[] hashS1 = new int[len1];
            int[] hashS2 = new int[len2];

            for (int i = 0; i < len1; i++)
            {
                for (int j = Math.Max(0, i - maxDist); j < Math.Min(len2, i + maxDist + 1); j++)
                {
                    if (s1[i] == s2[j] && hashS2[j] == 0)
                    {
                        hashS1[i] = 1;
                        hashS2[j] = 1;
                        m++;
                        break;
                    }
                }
            }

            if (m == 0)
            {
                return 0.0;
            }

            int t = 0;
            int k = 0;
            for (int i = 0; i < len1; i++)
            {
                if (hashS1[i] == 1)
                {
                    while (hashS2[k] == 0)
                    {
                        k++;
                    }
                    if (s1[i] != s2[k])
                    {
                        t++;
                    }
                    k++;
                }
            }
            t /= 2;

            double jaro = (m / (double)len1 + m / (double)len2 + (m - t) / (double)m) / 3.0;

            int commonPrefix = 0;
            for (int i = 0; i < Math.Min(Math.Min(len1, len2), maxL); i++)
            {
                if (s1[i] != s2[i])
                {
                    break;
                }
                commonPrefix++;
            }

            return jaro + commonPrefix * p * (1 - jaro);
        }

        private static double TrigramSimilarity(string s1, string s2)
        {
            var trigrams1 = new HashSet<string>(GetTrigrams(s1));
            var trigrams2 = new HashSet<string>(GetTrigrams(s2));
            var common = trigrams1.Intersect(trigrams2).Count();
            var total = trigrams1.Union(trigrams2).Count();
            return total > 0 ? common / (double)total : 0.0;
        }

        public static List<(string Item, double Score)> Search(string query, IEnumerable<string> items, double threshold = 0.6)
        {
            var results = new List<(string Item, double Score)>();

            foreach (var item in items)
            {
                double trigramScore = TrigramSimilarity(query, item);
                double jaroScore = JaroWinklerDistance(query, item);
                double combinedScore = 0.4 * trigramScore + 0.6 * jaroScore;

                if (combinedScore >= threshold)
                {
                    results.Add((item, combinedScore));
                }
            }

            return results.OrderByDescending(x => x.Score).ToList();
        }
    }
}
