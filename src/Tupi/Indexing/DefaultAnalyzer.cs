using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tupi.Indexing.Filters;

namespace Tupi.Indexing
{
    public class DefaultAnalyzer : IAnalyzer
    {
        private DefaultAnalyzer() { }

        private readonly IFilter[] _filters =
        {
            new ToLowerFilter(), 
            new StopWordsFilter(),
            new RemovePossesiveSufixFilter(), 
            new PorterStemmerFilter()
        };

        public bool Process(TokenSource source)
        {
            for (var i = 0; i < _filters.Length; i++)
            {
                if (!_filters[i].Process(source))
                    return false;
            }
            return true;
        }

        public IEnumerable<string> Analyze(string source)
        {
            using (var reader = new StringReader(source))
            {
                var tokenSource = new TokenSource(reader);
                return tokenSource.ReadAll(Process).ToArray();
            }
        }

        public string AnalyzeOnlyTheFirstToken(string source)
        {
            using (var reader = new StringReader(source))
            {
                var tokenSource = new TokenSource(reader);
                tokenSource.Next();
                Process(tokenSource);
                return tokenSource.ToString();
            }
        }

        private static readonly Lazy<DefaultAnalyzer> LazyInstance = 
            new Lazy<DefaultAnalyzer>(() => new DefaultAnalyzer());
        public static DefaultAnalyzer Instance => LazyInstance.Value;
    }
}
