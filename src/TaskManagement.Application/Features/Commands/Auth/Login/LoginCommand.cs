using TaskManagement.Application.Abstractions.CQ;

namespace TaskManagement.Application.Features.Commands.Auth.Login;

public record LoginCommand(string UserName) : ICommand;