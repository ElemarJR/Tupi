namespace Tupi.Indexing.Filters
{
    public interface IFilter
    {
        bool Process(TokenSource source);
    }
}