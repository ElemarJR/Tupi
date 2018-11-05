﻿using System.IO;
using Tupi.Indexing.Filters;

namespace Tupi.Indexing
{
    public class Indexer
    {
        private readonly IAnalyzer _analyzer;

        public Indexer(IAnalyzer analyzer = null)
        {
            _analyzer = analyzer ?? DefaultAnalyzer.Instance;
        }

        public InvertedIndex CreateIndex(
            params Stream[] documents
        )
        {
            var result = new InvertedIndex(documents.Length);

            for (var i = 0; i < documents.Length; i++)
            {
                using (var reader = new StreamReader(documents[i]))
                {
                    ProcessDocument(result, i, reader);
                }
            }

            return result;
        }

        public InvertedIndex CreateIndex(
            params string[] documents
        )
        {
            var result = new InvertedIndex(documents.Length);

            for (var i = 0; i < documents.Length; i++)
            {
                using (var reader = new StringReader(documents[i]))
                {
                    ProcessDocument(result, i, reader);
                }
            }

            return result;
        }

        private void ProcessDocument(
            InvertedIndex result, 
            int documentIndex, 
            TextReader reader
            )
        {
            var tokenSource = new TokenSource(reader);

            while (tokenSource.Next())
            {
                if (_analyzer.Process(tokenSource))
                {

                    result.Append(
                        new CharArraySegmentKey(tokenSource.Buffer, tokenSource.Size),
                        documentIndex, tokenSource.Position);
                }
            }
        }
    }
}
