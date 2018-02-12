namespace Tupi.Indexing.Filters
{
    public class ToLowerFilter : IFilter
    {
        public bool Process(TokenSource source)
        {
            for (var i = 0; i < source.Size; i++)
            {
                source.Buffer[i] = char.ToLowerInvariant(source.Buffer[i]);
            }
            return true;
        }
    }
}
