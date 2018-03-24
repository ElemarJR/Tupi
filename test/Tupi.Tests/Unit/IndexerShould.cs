using System.Linq;
using Tupi.Indexing;
using Xunit;

namespace Tupi.Tests.Unit
{
    public class IndexerShould
    {
        [Fact]
        public void AssignCorrectPositionsForEachTerm()
        {
            var documents = new[]
            {
                // 1    2  3    4   5    6    7     8   9    10  11    12
                "One item is just one item. Two items are other two items"
            };

            var index = new Indexer().CreateIndex(documents);
            var postings = index.GetPostingsFor("item");
            var positions = postings.SelectMany(p => p.Positions).ToArray();
            Assert.Equal(new long[] {2, 6, 8, 12}, positions);
        }

    }
}
