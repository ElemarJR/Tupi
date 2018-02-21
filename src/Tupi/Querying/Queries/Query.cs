using System.Collections.Generic;
using Tupi.Indexing;

namespace Tupi.Querying.Queries
{
    public abstract class Query
    {
        public abstract IEnumerable<int> Execute(InvertedIndex index);
    }
}
