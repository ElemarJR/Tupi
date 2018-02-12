using System.Collections.Generic;
using System.Linq;

namespace Tupi.Indexing
{
    public class InvertedIndex
    {
        
        private readonly IDictionary<string, List<int>> _data = 
            new Dictionary<string, List<int>>();
        internal void Append(string term, int documentId)
        {
            if (_data.ContainsKey(term))
            {
                _data[term].Add(documentId);
            }
            else
            {
                var postings = new List<int> {documentId};
                _data.Add(term, postings);
            }
        }

        public IEnumerable<int> Search(params string[] terms)
        {
            // no terms
            if (terms.Length == 0)
            {
                return Enumerable.Empty<int>();
            }

            // only one term
            if (terms.Length == 1)
            {
                return _data.ContainsKey(terms[0])
                    ? _data[terms[0]]
                    : Enumerable.Empty<int>();
            }

            var postings = new List<List<int>>();
            for (var i = 0; i < terms.Length; i++)
            {
                // there is a term with no results
                if (!_data.ContainsKey(terms[i]))
                {
                    return Enumerable.Empty<int>();
                }
                postings.Add(_data[terms[i]]);
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