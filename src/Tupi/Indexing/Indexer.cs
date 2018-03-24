using System;
using System.Diagnostics.SymbolStore;
using System.IO;
using System.Linq;

namespace Tupi.Indexing
{
    public class StringIndexer
    {
        private readonly IAnalyzer _analyzer;

        public StringIndexer(IAnalyzer analyzer = null)
        {
            _analyzer = analyzer ?? DefaultAnalyzer.Instance;
        }

        public InvertedIndex CreateIndex(
            params string[] documents
        )
        {
            var result = new InvertedIndex();

            for (var i = 0; i < documents.Length; i++)
            {
                using (var reader = new StringReader(documents[i]))
                {
                    var tokenSource = new TokenSource(reader);

                    var tokens = tokenSource
                        .ReadAllDistinct(_analyzer.Process)
                        .ToArray();

                    foreach (var token in tokens)
                    {
                        result.Append(token, i);
                    }
                }
            }

            return result;
        }
    }
}
