using Microsoft.AspNetCore.Mvc;
using Solidcode.Work.Infra.Entities;

namespace Solidcode.Work.Infra.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    // ----------------------------------------
    // Non-generic results (Delete, commands)
    // ----------------------------------------
    protected IActionResult FromResult(TResponse result)
    {
        if (result.IsSuccess)
        {
            return result.StatusCode switch
            {
                200 => Ok(result.Message),
                201 => StatusCode(201),
                204 => NoContent(), // ✔ allowed for commands
                _ => StatusCode(result.StatusCode, result.Message)
            };
        }

        return StatusCode(
            result.StatusCode,
            new { error = result.Message });
    }

    // ----------------------------------------
    // Generic results (Queries)
    // ----------------------------------------
    protected IActionResult FromResult<T>(TResponse<T> result)
    {
        if (result.IsSuccess)
        {
            // 🚫 NEVER return 204 for data
            // UI clients always expect JSON
            return Ok(result.Data);
        }

        return StatusCode(
            result.StatusCode,
            new { error = result.Message });
    }
}
