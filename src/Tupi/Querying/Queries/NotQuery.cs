using System.Collections.Generic;
using System.Linq;
using Tupi.Indexing;

namespace Tupi.Querying.Queries
{
    public class NotQuery : Query
    {
        private readonly Query _query;

        public NotQuery(Query query)
        {
            _query = query;
        }

        public override IEnumerable<int> Execute(InvertedIndex index) => 
            index.IndexedDocuments.Except(_query.Execute(index));
    }
}
