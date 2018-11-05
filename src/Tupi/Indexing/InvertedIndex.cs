using System.Collections.Generic;
using System.Linq;
using Tupi.Indexing.Filters;

namespace Tupi.Indexing
{
    public class InvertedIndex
    {
        private readonly int _maxDocumentId;

        internal InvertedIndex(int maxDocumentId)
        {
            _maxDocumentId = maxDocumentId;
        }

        private readonly ISet<int> _indexedDocuments = 
            new SortedSet<int>();
        public IEnumerable<int> IndexedDocuments => _indexedDocuments;
        public object NumberOfTerms => _data.Count;

        private readonly IDictionary<CharArraySegmentKey, Posting[]> _data = 
            new Dictionary<CharArraySegmentKey, Posting[]>();

        internal void Append(CharArraySegmentKey term, int documentId, long position)
        {
            if (_data.TryGetValue(term, out var postings))
            {
                if (postings[documentId] == null)
                {
                    postings[documentId] = new Posting(documentId);
                }

                postings[documentId].Positions.Add(position);
            }
            else
            {
                var posting = new Posting(documentId);
                posting.Positions.Add(position);

                postings = new Posting[_maxDocumentId];
                postings[documentId] = posting;
                _data.Add(term.Stabilize(), postings);
            }

            _indexedDocuments.Add(documentId);
        }

        public IEnumerable<Posting> GetPostingsFor(string term)
        {
            var key = new CharArraySegmentKey(term);
            return !_data.ContainsKey(key)
                ? Enumerable.Empty<Posting>()
                : _data[key].Where(posting => posting != null);
        } 
    }
}