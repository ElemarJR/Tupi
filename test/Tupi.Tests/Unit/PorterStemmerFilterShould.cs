using System.IO;
using Tupi.Indexing;
using Tupi.Indexing.Filters;
using Xunit;

namespace Tupi.Tests.Unit
{
    public class PorterStemmerFilterShould
    {
        [Theory]
        [InlineData("caresses", "caress")]
        [InlineData("ponies", "poni")]
        [InlineData("ties", "ti")]
        [InlineData("caress", "caress")]
        [InlineData("cats", "cat")]
        [InlineData("feed", "feed")]
        [InlineData("agreed", "agree")]
        [InlineData("disabled", "disable")]
        [InlineData("matting", "mat")]
        [InlineData("mating", "mate")]
        [InlineData("meeting", "meet")]
        [InlineData("milling", "mill")]
        [InlineData("messing", "mess")]
        [InlineData("meetings", "meet")]
        public void PerformStep1Correctly(string from, string to)
        {
            var filter = new PorterStemmerFilter();
            using (var reader = new StringReader(from))
            {
                var tokenSource = new TokenSource(reader);
                tokenSource.Next();
                filter.PerformStep1(tokenSource);
                Assert.Equal(to, tokenSource.ToString());
            }
        }

        [Theory]
        [InlineData("wordy", "wordi")]
        [InlineData("fry", "fry")]
        public void PerformStep2Correctly(string from, string to)
        {
            var filter = new PorterStemmerFilter();
            using (var reader = new StringReader(from))
            {
                var tokenSource = new TokenSource(reader);
                tokenSource.Next();
                filter.PerformStep2(tokenSource);
                Assert.Equal(to, tokenSource.ToString());
            }
        }

        [Theory]
        [InlineData("international", "internate")]
        [InlineData("rational", "rational")]
        [InlineData("constitutional", "constitution")]
        [InlineData("energizer", "energize")]
        [InlineData("internacionalization", "internacionalize")]
        [InlineData("enumeration", "enumerate")]
        [InlineData("consolidator", "consolidate")]
        [InlineData("tropicalism", "tropical")]
        [InlineData("vandalism", "vandal")]
        [InlineData("activeness", "active")]
        [InlineData("remorsefulness", "remorseful")]
        public void PerformStep3Correctly(string from, string to)
        {
            var filter = new PorterStemmerFilter();
            using (var reader = new StringReader(from))
            {
                var tokenSource = new TokenSource(reader);
                tokenSource.Next();
                filter.PerformStep3(tokenSource);
                Assert.Equal(to, tokenSource.ToString());
            }
        }

        [Theory]
        [InlineData("intrincate", "intrinc")]
        [InlineData("denunciative", "denunci")]
        [InlineData("cannibalize", "cannibal")]
        [InlineData("madness", "mad")]
        public void PerformStep4Correctly(string from, string to)
        {
            var filter = new PorterStemmerFilter();
            using (var reader = new StringReader(from))
            {
                var tokenSource = new TokenSource(reader);
                tokenSource.Next();
                filter.PerformStep4(tokenSource);
                Assert.Equal(to, tokenSource.ToString());
            }
        }

        [Theory]
        [InlineData("elemental", "element")]
        public void PerformStep5Correctly(string from, string to)
        {
            var filter = new PorterStemmerFilter();
            using (var reader = new StringReader(from))
            {
                var tokenSource = new TokenSource(reader);
                tokenSource.Next();
                filter.PerformStep5(tokenSource);
                Assert.Equal(to, tokenSource.ToString());
            }
        }
    }
}


