using System.Collections.Generic;
using System.Linq;

namespace Tupi.Indexing
{
    public class InvertedIndex
    {
        private readonly ISet<int> _indexedDocuments = 
            new SortedSet<int>();
        public IEnumerable<int> IndexedDocuments => _indexedDocuments;

        private readonly IDictionary<string, List<Posting>> _data = 
            new Dictionary<string, List<Posting>>();

        internal void Append(string term, int documentId, long position)
        {
            if (_data.ContainsKey(term))
            {
                var posting =
                    _data[term].FirstOrDefault(p => p.DocumentId == documentId);

                if (posting == null)
                {
                    posting = new Posting(documentId);
                    _data[term].Add(posting);
                }
            
                posting.Positions.Add(position);
            }
            else
            {
                var posting = new Posting(documentId);
                posting.Positions.Add(position);
                var postings = new List<Posting> {posting};
                _data.Add(term, postings);
            }

            _indexedDocuments.Add(documentId);
        }

        public IEnumerable<Posting> GetPostingsFor(string term) => !_data.ContainsKey(term) 
            ? Enumerable.Empty<Posting>() 
            : _data[term];
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