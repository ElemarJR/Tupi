using System;
using System.Collections.Generic;
using System.Linq;
using Tupi.Indexing;

namespace Tupi.Querying.Queries
{
    public class AllQuery : Query
    {
        private readonly IEnumerable<Query> _queries;

        public AllQuery(IEnumerable<Query> queries)
        {
            _queries = queries;
        }

        public override IEnumerable<int> Execute(InvertedIndex index)
        {
            var postings = new List<List<int>>();
            foreach (var query in _queries)
            {
                var l = query.Execute(index).ToList();

                if (!l.Any())
                {
                    return Enumerable.Empty<int>();
                }

                postings.Add(l.ToList());
            }

            postings.Sort((l1, l2) => l1.Count.CompareTo(l2.Count));

            var results = postings[0];

            for (var i = 1; i < postings.Count; i++)
            {
                results = results.Intersect(postings[1]).ToList();
            }

            return results;
        }

        public static Query From(params string[] terms) => 
            new AllQuery(terms.Select(TermQuery.From).ToList());
    }
}
