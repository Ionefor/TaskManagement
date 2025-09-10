namespace TaskManagement.Application.Abstractions;

public interface ITokenProvider
{
    string GenerateAccessToken(string userName);
}