using System.Collections.Generic;
using System.Linq;

namespace Tupi.Indexing
{
    public class InvertedIndex
    {
        
        private readonly IDictionary<string, List<int>> _data = 
            new Dictionary<string, List<int>>();

        public object NumberOfTerms => _data.Count;

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

        public IEnumerable<int> GetPostingsFor(string term) => !_data.ContainsKey(term) 
            ? Enumerable.Empty<int>() 
            : _data[term];
    }
}