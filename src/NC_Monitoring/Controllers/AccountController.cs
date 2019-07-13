using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NC.AspNetCore.Filters;
using NC_Monitoring.Business.Managers;
using NC_Monitoring.Data.Enums;
using NC_Monitoring.Data.Models;
using NC_Monitoring.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NC_Monitoring.Controllers
{
    public class AccountController : BaseController
    {
        private readonly ApplicationUserManager userManager;
        private readonly ApplicationSignInManager signInManager;

        public AccountController
        (
            ApplicationUserManager userManager,
            ApplicationSignInManager signInManager
        )
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet("Login")]
        [AllowAnonymous]
        [AllowAnonymousOnly]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost("Login")]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var vysledekOvereni = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (vysledekOvereni.Succeeded)
                {
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Incorect credentials.");
                    return View(model);
                }
            }

            return View(model);
        }

        public class AuthenticationViewModel
        {
            [Required]
            [EmailAddress]
            public string Username { get; set; }

            [Required]
            public string Password { get; set; }
        }

        [HttpPost("/api/authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody]AuthenticationViewModel loginDTO)
        {
            if (TryValidateModel(loginDTO))
            {
                var username = loginDTO?.Username;
                var password = loginDTO?.Password;

                var user = await userManager.FindByNameAsync(username);

                if (user != null)
                {
                    var result = await signInManager.PasswordSignInAsync(user, password, false, false);

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(Startup.SECRET);

                    //var claims = await userManager.GetClaimsAsync(user);
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Id.ToString()),
                    };

                    foreach (var role in await userManager.GetRolesAsync(user))
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }

                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Issuer = Startup.JWTIssuer,
                        Audience = Startup.JWTAudience,
                        Subject = new ClaimsIdentity(claims),
                        Expires = DateTime.UtcNow.AddDays(7),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);

                    //var token = new JwtSecurityToken(
                    //    issuer: Startup.JWTIssuer,
                    //    audience: Startup.JWTAudience,
                    //    expires: DateTime.UtcNow.AddHours(1),
                    //    claims: new Claim[]
                    //    {
                    //        new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    //    },
                    //    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
                    //    );



                    var tokenString = tokenHandler.WriteToken(token);

                    if (result.Succeeded)
                    {
                        return Ok(new
                        {
                            Id = user.Id,
                            Username = user.UserName,
                            Email = user.Email,
                            Token = tokenString,
                            Expiration = token.ValidTo,
                        });
                    }
                }
            }

            return Unauthorized();
            //return BadRequest(ModelState.GetFullErrorMessage());
        }

        [HttpPost("Logout")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet("Create")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!TryValidateModel(model))
            {
                return BadRequest(ModelState.GetFullErrorMessage());
            }

            if (model.CurrentPassword == model.Password)
            {
                return BadRequest("New password is same as old password.");
            }

            ApplicationUser user = await userManager.GetUserAsync(User);

            if (user == null)
            {
                return BadRequest("User was not found.");
            }

            IdentityResult result = await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.Password);

            if (!result.Succeeded)
            {
                return BadRequest(string.Join(", ", result.Errors.Select(x => x.Description)));
            }

            return Ok("Password was changed.");
        }

        [HttpPost("Create")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Create(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return Redirect(Request.Path.Value);
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        [HttpGet("AccessDenied")]
        public IActionResult AccessDenied()
        {
            return View();
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }
    }
}