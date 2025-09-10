using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Models;

namespace TaskManagement.Presentation.Controllers;

[ApiController]
public class ApplicationController : ControllerBase
{
    public override OkObjectResult Ok(object? value)
    {
        var envelope = Envelope.Ok(value);

        return base.Ok(envelope);
    }
}