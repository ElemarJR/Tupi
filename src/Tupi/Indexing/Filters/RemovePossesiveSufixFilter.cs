namespace Tupi.Indexing.Filters
{
    public class RemovePossesiveSufixFilter : IFilter
    {
        public bool Process(TokenSource source)
        {
            if (source.Size <= 2)
                return true;

            if (source.EndsWith('\'')) 
            {
                source.Size--;
            }

            else if (
                source.Buffer[source.Size - 1] == 's' 
                && source.Buffer[source.Size - 2] == '\''
                )
            {
                source.Size -= 2;
            }
            return true;
        }
    }
}
