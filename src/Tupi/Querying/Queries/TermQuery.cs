using System;
using System.Collections.Generic;
using System.IO;
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
            From(term, DefaultAnalyzer.Instance);

        public static Query From(string term, IAnalyzer analyzer)
        {
            using (var reader = new StringReader(term))
            {
                var tokenSource = new TokenSource(reader);
                tokenSource.Next();

                if (!analyzer.Process(tokenSource))
                {
                    throw new InvalidOperationException($"Could not generate a term from: {term}");
                }

                return new TermQuery(tokenSource.ToString());
            }
        }
    }
}
