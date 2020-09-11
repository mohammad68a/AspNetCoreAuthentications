namespace Auth05_JwtTokenServer
{
    public static class Constants
    {
        public const string Issuer = Audience;
        public const string Audience = "http://localhost:10957";
        public const string Secret = "not_too_short_secret_otherwise_it_might_raise_error";
    }
}
