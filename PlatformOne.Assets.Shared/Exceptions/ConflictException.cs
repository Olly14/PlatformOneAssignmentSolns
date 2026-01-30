namespace PlatformOne.Assets.Shared.Exceptions;

public class ConflictException : Exception
{
    public ConflictException(string isin,string message) : base(message)
    {
        Isin = isin;
    }

    public string Isin { get; set; }
}
