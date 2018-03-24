using System.Collections.Generic;
using System.Linq;
using Tupi.Indexing.Filters;

namespace Tupi.Indexing
{
    public class InvertedIndex
    {
        private readonly ISet<int> _indexedDocuments = 
            new SortedSet<int>();
        public IEnumerable<int> IndexedDocuments => _indexedDocuments;
        public object NumberOfTerms => _data.Count;

        private readonly IDictionary<CharArraySegmentKey, List<Posting>> _data = 
            new Dictionary<CharArraySegmentKey, List<Posting>>();

        internal void Append(CharArraySegmentKey term, int documentId, long position)
        {
            if (_data.TryGetValue(term, out var postings))
            {
                bool found = false;
                foreach (var posting in postings)
                {
                    if (posting.DocumentId == documentId)
                    {
                        found = true;
                        posting.Positions.Add(position);
                    }
                }

                if (!found)
                {
                    var posting = new Posting(documentId);
                    postings.Add(posting);
                }

            }
            else
            {
                var posting = new Posting(documentId);
                posting.Positions.Add(position);
                postings = new List<Posting> {posting};
                _data.Add(term.Stabilize(), postings);
            }

            _indexedDocuments.Add(documentId);
        }

        public IEnumerable<Posting> GetPostingsFor(string term)
        {
            var key = new CharArraySegmentKey(term);
            return !_data.ContainsKey(key)
                ? Enumerable.Empty<Posting>()
                : _data[key];
        } 
    }

    public class Posting
    {
        public int DocumentId { get; }
        public IList<long> Positions { get; } = new List<long>(); 

        public Posting(int documentId)
        {
            DocumentId = documentId;
        }

        public static implicit operator int(Posting entry) => 
            entry.DocumentId;
    }
}