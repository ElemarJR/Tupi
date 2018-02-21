using System.IO;
using System.Linq;
using Tupi.Indexing;
using Xunit;

namespace Tupi.Tests.Unit
{
    public class DefaultAnalyzerShould
    {
        [Theory]
        [InlineData(
            "This is a simple string.",
            new[] { "simpl", "str" }
        )]
        public void TokenizeStrings(string input, string[] expected)
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
