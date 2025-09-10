using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Features.Commands.Auth.Login;
using TaskManagement.Application.Models;
using TaskManagement.Presentation.Requests.Auth;

namespace TaskManagement.Presentation.Controllers;

[Route("[controller]")]
public class AuthController : ApplicationController
{
    [HttpPost]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request,
        [FromServices] LoginHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(
            request.ToCommand(),
            cancellationToken);

        if (result.IsFailure)
            return BadRequest(Envelope.Error(result.Error));

        return Ok(Envelope.Ok(result.Value));
    }
}