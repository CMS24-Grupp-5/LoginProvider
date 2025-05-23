using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;
using Presentation.Services;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]

public class LoginController(IService service) : ControllerBase
{

    private readonly IService _service = service;  

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginForm formData)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _service.LoginAsync(formData);

        if (result.Success)
        {
            return Ok(result);
        }


        return BadRequest(result);
    }
}


