using Microsoft.AspNetCore.Mvc;
using solidcode.work.infra.Entities;

namespace solidcode.work.infra.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    protected IActionResult FromResult(TResult result)
    {
        if (result.IsSuccess)
        {
            return result.StatusCode switch
            {
                200 => Ok(result.Message),
                201 => StatusCode(201),
                204 => NoContent(),
                _ => StatusCode(result.StatusCode, result.Message)
            };
        }

        return StatusCode(result.StatusCode, new { error = result.Message });
    }

    protected IActionResult FromResult<T>(TResult<T> result)
    {
        if (result.IsSuccess)
        {
            return result.StatusCode switch
            {
                200 => Ok(result.Data),
                201 => StatusCode(201, result.Data),
                204 => NoContent(),
                _ => StatusCode(result.StatusCode, result.Data)
            };
        }

        return StatusCode(result.StatusCode, new { error = result.Message });
    }
}
