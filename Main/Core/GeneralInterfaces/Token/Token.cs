namespace VMFramework.Core
{
    public class Token : IToken
    {
        public object Source { get; set; }
    }

    public class TokenData<TData>
    {
        public TData Data { get; set; }
    }
}