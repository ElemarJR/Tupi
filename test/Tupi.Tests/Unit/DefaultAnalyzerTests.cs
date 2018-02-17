using System.IO;
using System.Linq;
using Tupi.Indexing;
using Xunit;

namespace Tupi.Tests.Unit
{
    public class DefaultAnalyzerTests
    {
        [Theory]
        [InlineData(
            "This is a simple string.",
            new[] { "simpl", "str" }
        )]
        public void TokenizationWorks(string input, string[] expected)
        {
            using (var reader = new StringReader(input))
            {
                var tokenSource = new TokenSource(reader);
                var result = tokenSource
                    .ReadAll(DefaultAnalyzer.Instance.Process)
                    .ToArray();
                Assert.Equal(expected, result);
            }
        }
    }
}
