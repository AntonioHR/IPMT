using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ipmt.Engine.SuffArray
{
    public class SuffixArray
    {
        struct Suffix
        {
            public int start;
        }

        public string text;
        Suffix[] suffixes;

        private SuffixArray(string text)
        {
            this.text = text;
            this.suffixes = new Suffix[text.Length];
        }

        public static SuffixArray BuildFromText(string text)
        {
            Builder factory = new Builder(text);
            return factory.Build();
        }
        public string ToStringDebug(int maxChars = 5)
        {
            return string.Join("\n", suffixes.Select((x, i) => string.Format("{0}: {1} ({2})", i, 
                (x.start + maxChars < text.Length? text.Substring(x.start, maxChars)  : text.Substring(x.start)).Replace("\n", "\\n")
                , x.start)));
        }

        public List<int> Find(string pat)
        {
            int n = text.Length;

            int right = text.Length;
            int left = 0;

            while (left < right)
            {
                int mid = (left + right) / 2;

                bool matches = CompareStrToSuff(pat, suffixes[mid], out int cmp);

                //Go back if matches or surpasses
                if (matches || cmp < 0)
                {
                    right = mid;
                }
                else
                {
                    left = mid + 1;
                }
            }
            int firstMatch = left;
            right = n;
            while (right > left)
            {
                int mid = (left + right) / 2;

                bool matches = CompareStrToSuff(pat, suffixes[mid], out int cmp);

                //Go forward if matches or precedes
                if (matches || cmp > 0)
                {
                    left = mid + 1;
                }
                else
                {
                    right = mid;
                }
            }
            int firstNonMatch = left;

            List<int> result = new List<int>(firstNonMatch - firstMatch);
            for (int i = firstMatch; i < firstNonMatch; i++)
            {
                result.Add(suffixes[i].start);
            }
            return result;
        }

        public string MatchesToStringDebug(string pat)
        {
            var matches = Find(pat);
            int length = pat.Length;
            int maxChars = length + 5;

            return string.Join("\n", matches.OrderBy(x => x).Select((x) => string.Format("({0}): {1}", x,
                  (x + maxChars < text.Length ? text.Substring(x, maxChars) + "..." : text.Substring(x)).Replace("\n", "\\n")
                  )));
        }

        public void DebugTestComparisons(string toCompare)
        {
            int maxChars = 5;
            string toShow = string.Join("\n", suffixes.Select((x, i) =>
            {
                bool matches = CompareStrToSuff(toCompare, x, out int cmp);
                return string.Format("{0}: {1} ({2}) -> {3}, Diff: {4}", i,
                (x.start + maxChars < text.Length ? text.Substring(x.start, maxChars) : text.Substring(x.start)).Replace("\n", "\\n")
                , x.start, matches? "Match" : "Not a Match", cmp);
                }));
            Console.WriteLine(toShow);
        }

        private bool CompareStrToSuff(string pat, Suffix suff, out int comparison)
        {
            int suffLength = text.Length - suff.start;

            for (int i = 0; i < pat.Length && i < suffLength; i++)
            {
                int diff = pat[i] - text[suff.start + i];

                if (diff != 0)
                {
                    comparison = Math.Sign(diff);
                    return false;
                }
            }

            comparison = Math.Sign(pat.Length - suffLength);
            return true;
        }

        class Builder
        {
            SuffixArray result;
            string text;
            List<SuffixBuilder> suffs;
            
            public int[] sortedPositions;

            struct SuffixBuilder
            {
                Builder builder;
                public int start;
                public int[] rank;

                public SuffixBuilder(Builder builder, int start)
                {
                    this.builder = builder;
                    this.start = start;
                    rank = new int[2];
                }

                public void InitRanks()
                {
                    rank[0] = builder.text[start] - 'a';
                    rank[1] = (start + 1) < builder.text.Length ? builder.text[start+1] - 'a' : -1;
                }
            }

            public Builder(string text)
            {
                this.text = text;
                suffs = new List<SuffixBuilder>(text.Length);
                sortedPositions = new int[text.Length];
            }

            public SuffixArray Build()
            {
                result = new SuffixArray(text);

                CreateSuffs();


                suffs.Sort(Compare);

                for (int k = 4; k < text.Length * 2; k = k*2)
                {
                    UpdateRanks();
                    UpdateNextRanks(k);
                    suffs.Sort(Compare);
                }

                for (int i = 0; i < text.Length; i++)
                {
                    result.suffixes[i].start = suffs[i].start;
                }

                return result;
            }

            private void UpdateNextRanks(int k)
            {
                for (int i = 0; i < text.Length; i++)
                {
                    int nextIndex = suffs[i].start + k / 2;
                    suffs[i].rank[1] = (nextIndex < text.Length) ?
                        suffs[sortedPositions[nextIndex]].rank[0] : -1;
                }
            }

            private void UpdateRanks()
            {
                int rank = 0;
                int prevRank = suffs[0].rank[0];
                suffs[0].rank[0] = 0;
                sortedPositions[suffs[0].start] = 0;

                for (int i = 1; i < text.Length; i++)
                {
                    bool sameRankAsBefore = suffs[i].rank[0] == prevRank &&
                        suffs[i].rank[1] == suffs[i - 1].rank[1];
                    if (sameRankAsBefore)
                    {
                        suffs[i].rank[0] = rank;
                    }
                    else
                    {
                        prevRank = suffs[i].rank[0];
                        rank++;
                        suffs[i].rank[0] = rank;
                    }

                    sortedPositions[suffs[i].start] = i;
                }
            }

            private static int Compare(SuffixBuilder a, SuffixBuilder b)
            {
                return a.rank[0] != b.rank[0] ? a.rank[0] - b.rank[0] : a.rank[1] - b.rank[1];
            }

            private void CreateSuffs()
            {
                for (int i = 0; i < text.Length; i++)
                {
                    suffs.Add(new SuffixBuilder(this, i));
                    suffs[i].InitRanks();
                }
            }
            
        }
    }
}
