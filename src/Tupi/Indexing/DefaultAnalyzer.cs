using System;
using Tupi.Indexing.Filters;

namespace Tupi.Indexing
{
    public class DefaultAnalyzer : IAnalyzer
    {
        private readonly IFilter[] _filters =
        {
            new ToLowerFilter(), 
            new StopWordsFilter()
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

        private static readonly Lazy<DefaultAnalyzer> LazyInstance = 
            new Lazy<DefaultAnalyzer>(() => new DefaultAnalyzer());
        public static DefaultAnalyzer Instance => LazyInstance.Value;
    }
}
