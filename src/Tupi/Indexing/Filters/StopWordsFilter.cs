using System.Collections.Generic;

namespace Tupi.Indexing.Filters
{
    // based on:
    // https://github.com/ayende/Corax/blob/master/Corax/Indexing/Filters/StopWordsFilter.cs
    public class StopWordsFilter : IFilter
    {
        private static readonly string[] DefaultStopWords =
        {
            "a", "an", "and", "are", "as", "at", "be", "but", "by", "for",
            "if", "in", "into", "is", "it", "no", "not", "of", "on", "or", "such", "that", "the",
            "their", "then", "there", "these", "they", "this", "to", "was", "will", "with"
        };

        private readonly HashSet<CharArraySegmentKey> _stopWords
            = new HashSet<CharArraySegmentKey>();

        public StopWordsFilter()
        {
            foreach (var word in DefaultStopWords)
            {
                _stopWords.Add(new CharArraySegmentKey(word.ToCharArray()));
            }
        }

        public bool Process(TokenSource source)
        {
            var term = new CharArraySegmentKey(source.Buffer, source.Size);
            return !_stopWords.Contains(term);
        }
    }
}
