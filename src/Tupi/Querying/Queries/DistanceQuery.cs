using System;
using System.Collections.Generic;
using Tupi.Indexing;

namespace Tupi.Querying.Queries
{
    public class DistanceQuery : Query
    {
        private readonly string _term1;
        private readonly string _term2;
        private readonly int _maxDistance;

        public DistanceQuery(string term1, string term2, int maxDistance)
        {
            _term1 = term1;
            _term2 = term2;
            _maxDistance = maxDistance;
        }

        public override IEnumerable<int> Execute(InvertedIndex index)
        {
            using (var p1 = index.GetPostingsFor(_term1).GetEnumerator())
            using (var p2 = index.GetPostingsFor(_term2).GetEnumerator())
            {
                var hasElementsP1 = p1.MoveNext();
                var hasElementsP2 = p2.MoveNext();
                while (hasElementsP1 && hasElementsP2)
                {
                    if (p1.Current.DocumentId == p2.Current.DocumentId)
                    {
                        var pp1 = p1.Current.Positions;
                        var pp2 = p2.Current.Positions;
                        for (var i = 0; i < pp1.Count; i++)
                        {
                            for (var j = 0; j < pp2.Count; j++)
                            {
                                if (Math.Abs(pp1[i] - pp2[j]) <= _maxDistance)
                                {
                                    yield return p1.Current.DocumentId;
                                    break;
                                }
                                if (pp2[j] > pp1[i])
                                {
                                    break;
                                }
                            }
                        }

                        hasElementsP1 = p1.MoveNext();
                        hasElementsP2 = p2.MoveNext();
                    }
                    else if (p1.Current.DocumentId < p2.Current.DocumentId)
                    {
                        hasElementsP1 = p1.MoveNext();
                    }
                    else
                    {
                        hasElementsP2 = p2.MoveNext();
                    }
                }
            }
        }
    }
}
