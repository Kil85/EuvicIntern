namespace EuvicIntern.Exceptions
{
    public class AuthorizationFailedException : Exception
    {
        public AuthorizationFailedException(string message)
            : base(message) { }
    }
}
