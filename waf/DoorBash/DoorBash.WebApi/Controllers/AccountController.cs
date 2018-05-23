using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using DoorBash.Persistence;
using DoorBash.Persistence.DTOs;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly SignInManager<User> signInManager;

        public AccountController(SignInManager<User> signInManager)
        {
            this.signInManager = signInManager;
        }

        // api/Account/Login
        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody] UserDto user)
        {
            if (signInManager.IsSignedIn(User))
                await signInManager.SignOutAsync();

            if (ModelState.IsValid)
            {
                var res = await signInManager.PasswordSignInAsync(user.Username, user.Password, isPersistent: false, lockoutOnFailure: false);

                if (res.Succeeded)
                {
                    return Ok();
                }

                ModelState.AddModelError("", "Invalid username or password");
                return Unauthorized();
            }
            return Unauthorized();
        }

        // api/Account/Signout
        [HttpPost("Signout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();

            return Ok();
        }
    }
}
