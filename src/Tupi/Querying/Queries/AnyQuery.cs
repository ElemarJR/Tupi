using System.Collections.Generic;
using System.Linq;
using Tupi.Indexing;

namespace Tupi.Querying.Queries
{
    public class AnyQuery : Query
    {
        private readonly IEnumerable<Query> _queries;

        public AnyQuery(IEnumerable<Query> queries)
        {
            _queries = queries;
        }

        public AnyQuery(params Query[] queries)
        {
            _queries = queries;
        }

        public override IEnumerable<int> Execute(InvertedIndex index)
        {
            var results = Enumerable.Empty<int>();
            return _queries.Aggregate(
                seed: Enumerable.Empty<int>(),
                func: (current, query) => current.Union(query.Execute(index))
            );
        }

        public static Query From(params string[] terms) =>
            new AllQuery(terms.Select(TermQuery.From).ToList());
    }
}
