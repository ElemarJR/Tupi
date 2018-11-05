using System.Collections.Generic;

namespace Tupi.Indexing
{
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