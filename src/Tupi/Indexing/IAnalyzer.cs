using System.Collections.Generic;

namespace Tupi.Indexing
{
    public interface IAnalyzer
    {
        bool Process(TokenSource source);
        IEnumerable<string> Analyze(string source);
        string AnalyzeOnlyTheFirstToken(string source);
    }
}