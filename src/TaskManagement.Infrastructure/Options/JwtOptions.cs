namespace TaskManagement.Infrastructure.Options;

public class JwtOptions
{
    public const string Jwt = nameof(Jwt);
    
    public string Audience { get; init; } = string.Empty;
    
    public string Issuer { get; init; } = string.Empty;
    
    public string Key { get; init; } = string.Empty;
    
    public string ExpiredMinutesTime { get; init; } = string.Empty;
}