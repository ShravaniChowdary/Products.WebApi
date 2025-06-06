namespace Products.Api.Models
{
    public class AppSettings
    {
        public JwtSettings Jwt { get; set; } = default!;
    }

    public class JwtSettings
    {
        public string Issuer { get; set; } = default!;
        public int ExpiryInMinutes { get; set; }
        public string Key { get; set; } = default!;
    }
}
