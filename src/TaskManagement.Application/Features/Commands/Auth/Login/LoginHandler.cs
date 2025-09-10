using CSharpFunctionalExtensions;
using TaskManagement.Application.Abstractions;
using TaskManagement.Application.Abstractions.CQ;
using TaskManagement.Domain.Errors;

namespace TaskManagement.Application.Features.Commands.Auth.Login;

public class LoginHandler :
    ICommandHandler<string, LoginCommand>
{
    private readonly ITokenProvider _tokenProvider;

    public LoginHandler(ITokenProvider tokenProvider)
    {
        _tokenProvider = tokenProvider;
    }

    public Task<Result<string, ErrorList>> Handle(
        LoginCommand command,
        CancellationToken cancellationToken = default)
    {
        var  accessToken = _tokenProvider.GenerateAccessToken(command.UserName);

        return Task.FromResult(Result.Success<string, ErrorList>(accessToken));
    }
}