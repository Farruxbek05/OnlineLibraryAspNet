namespace OnlineLibraryAspNet.Class;

public class JwtOption
{
    public string Key { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int ExpiresInMinutes { get; set; }
}
