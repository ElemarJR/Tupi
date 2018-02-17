using System.Collections.Generic;
using System.Linq;
using Tupi.Indexing;

namespace Tupi.Querying
{
    public class Searcher
    {
        private readonly InvertedIndex _index;

        public Searcher(InvertedIndex index)
        {
            _index = index;
        }


        public IEnumerable<int> Search(params string[] terms)
        {
            // no terms
            if (terms.Length == 0)
            {
                return Enumerable.Empty<int>();
            }

            var postings = new List<List<int>>();
            for (var i = 0; i < terms.Length; i++)
            {
                // there is a term with no results
                var l = _index.GetPostingsFor(terms[i]).ToList();

                if (!l.Any())
                {
                    return l;
                }

                postings.Add(l);
            }

            // 
            postings.Sort((l1, l2) => l1.Count.CompareTo(l2.Count));

            var results = postings[0];

            for (var i = 1; i < postings.Count; i++)
            {
                results = results.Intersect(postings[1]).ToList();
            }

            return results;
        }
    }
}
