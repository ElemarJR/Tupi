using System;
using System.Diagnostics.SymbolStore;
using System.IO;
using System.Linq;

namespace Tupi.Indexing
{
    public class StringIndexer
    {
        public static InvertedIndex CreateIndex(
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
                        .ReadAll(DefaultAnalyzer.Instance.Process)
                        .Distinct()
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
