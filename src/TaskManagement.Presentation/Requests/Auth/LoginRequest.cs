using TaskManagement.Application.Features.Commands.Auth.Login;

namespace TaskManagement.Presentation.Requests.Auth;

public record LoginRequest(string UserName)
{
    public LoginCommand ToCommand() => 
        new(UserName);
}