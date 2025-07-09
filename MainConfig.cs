namespace iworfShop_backend_light;

public class MainConfig
{
    public JwtConfig JwtConfig { get; set; }
}

public class JwtConfig
{
    public string Key { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
}