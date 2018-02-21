using System.Collections.Generic;
using Tupi.Indexing;

namespace Tupi.Querying.Queries
{
    public class TermQuery : Query
    {
        private readonly string _term;

        public TermQuery(string term)
        {
            _term = term;
        }

        public override IEnumerable<int> Execute(InvertedIndex index) 
            => index.GetPostingsFor(_term);

        public static Query From(string term) =>
            new TermQuery(term);
    }
}
