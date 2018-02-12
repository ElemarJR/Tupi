namespace Tupi.Indexing
{
    public interface IAnalyzer
    {
        bool Process(TokenSource source);
    }
}