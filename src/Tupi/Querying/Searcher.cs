using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tupi.Indexing;
using Tupi.Querying.Queries;

namespace Tupi.Querying
{
    public class Searcher
    {
        private readonly InvertedIndex _index;

        public Searcher(InvertedIndex index)
        {
            _index = index;
        }

        public IEnumerable<int> Search(string query, IAnalyzer analyzer)
        {
            using (var reader = new StringReader(query))
            {
                var terms = new TokenSource(reader)
                    .ReadAll(DefaultAnalyzer.Instance.Process)
                    .ToArray();
                return Search(terms);
            }
        }

        public IEnumerable<int> Search(params string[] terms) => 
            Search(AllQuery.From(terms));

        public IEnumerable<int> Search(Query query) 
            => query.Execute(_index);
    }
}
