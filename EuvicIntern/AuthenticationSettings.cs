namespace EuvicIntern
{
    public class AuthenticationSettings
    {
        public string JwtKey { get; set; }
        public string JwtIssuer { get; set; }
        public int JwtExpiredDays { get; set; }
    }
}
