using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoginController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager) : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager = userManager;
    private readonly SignInManager<IdentityUser> _signInManager = signInManager;

    [HttpPost]
    public async Task<IActionResult> Login(LoginForm loginForm)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _signInManager.PasswordSignInAsync(
            loginForm.Email, loginForm.Password, false, lockoutOnFailure: false);

        if (result.Succeeded)
        {
            var user = await _userManager.FindByEmailAsync(loginForm.Email);

            return Ok(new
            {
                success = true,
                user = new
                {
                    user!.Id,
                    user.Email,
                    user.UserName
                }
            });
        }
        else
        {
            return Unauthorized(new
            {
                success = false,
                message = "Invalid email or password"
            });
        }
    }

}
