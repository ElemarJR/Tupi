using System.IO;
using Tupi.Indexing;
using Tupi.Indexing.Filters;
using Xunit;

namespace Tupi.Tests.Unit
{
    public class RemovePossesiveSuffixShould
    {
        [Theory]
        [InlineData("boys's", "boys")]
        [InlineData("boys'", "boys")]
        [InlineData("boys", "boys")]
        public void RemovePossesiveSufixWhenItIsPresent(string from, string to)
        {
            var filter = new RemovePossesiveSufixFilter();
            using (var reader = new StringReader(from))
            {
                var tokenSource = new TokenSource(reader);
                tokenSource.Next();
                Assert.True(filter.Process(tokenSource));
                Assert.Equal(to, tokenSource.ToString());
            }
        }
    }
}
