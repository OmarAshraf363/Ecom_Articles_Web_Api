using AutoMapper;
using Ecom.ApI.Helper;
using Ecom.Core.DTO;
using Ecom.Core.Entites.Product;
using Ecom.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ecom.ApI.Controllers
{

    public class AccountsController : BaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration configuration;
        public AccountsController(IUnitOfWork unitOfWork, IMapper mapper, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IConfiguration configuration) : base(unitOfWork, mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this.configuration = configuration;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {

            var user = new AppUser
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.Email,


            };
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                return BadRequest(new ResponseApi(int.Parse(result.Errors.FirstOrDefault()?.Code), result.Errors.FirstOrDefault().Description));
            }
            await _userManager.AddToRoleAsync(user, "User");
            var token=await _userManager.GenerateEmailConfirmationTokenAsync(user);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            return Ok(new
            {
                StatusCode = 200,
                Message = "User Created Successfully,you need confirm it",
                confirmationToken = token
            });
        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string token,string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return BadRequest(new ResponseApi(404, "User not found"));
            }
            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return BadRequest(new ResponseApi(400, "Invalid Token"));
            }
            return Ok(new ResponseApi(200, "Email Confirmed Successfully"));
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDto)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(loginDto.Email);
                if (user == null)
                {
                    return BadRequest(new ResponseApi(404, "User not found"));
                }
                var result = await _signInManager.PasswordSignInAsync(user, loginDto.Password, true, false);
                if (!result.Succeeded)
                {
                    if (result.IsLockedOut)
                    {
                        return BadRequest(new ResponseApi(403, "User is locked out"));
                    }
                    if (result.IsNotAllowed)
                    {
                        return BadRequest(new ResponseApi(403, "User is not allowed"));
                    }
                    if (result.RequiresTwoFactor)
                    {
                        return BadRequest(new ResponseApi(403, "User requires two factor authentication"));
                    }

                    return BadRequest(new ResponseApi(401, "Invalid Login Attemp"));
                }
                var token = GenerateJwtToken.GenerateToken(user.Id, configuration);
                var roles = await _userManager.GetRolesAsync(user);
                return Ok(new
                {
                    token,
                    roles
                });

            }
            catch (Exception ex)
            {

                return BadRequest(new ResponseApi(500, ex.Message));
            }


        }

        [HttpGet("get-user")]
        public IActionResult GetUser()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var user = _userManager.Users.FirstOrDefault(x => x.Id == userId);
                if (user == null)
                {
                    return NotFound(new ResponseApi(404, "User not found"));
                }
                return Ok(new
                {
                    displayName = user.DisplayName,
                    email = user.Email,
                    userName = user.UserName,
                    picImage = user.PicImage ?? null,
                    phoneNumber = user.PhoneNumber ?? null,
                });



            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }
        [HttpPost("update-user")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDTO updateUserDTO)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NotFound(new ResponseApi(404, "User not found"));
                }
                user.DisplayName = updateUserDTO.DisplayName;
                user.UserName = updateUserDTO.UserName;
                user.Email = updateUserDTO.Email;
                if (updateUserDTO.PicImage != null)
                {
                    user.PicImage = updateUserDTO.PicImage;
                }
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    return BadRequest(new ResponseApi(400, "Invalid Model State"));
                }
                return Ok(new ResponseApi(200, "User Updated Successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApi(500, ex.Message));
            }
        }


        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return BadRequest(new ResponseApi(404, "User not found"));
            }
            var token=await _userManager.GeneratePasswordResetTokenAsync(user);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            return Ok(new
            {
                StatusCode = 200,
                Message = "Reset Password Token Created Successfully",
                resetPasswordToken = token
            });

        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromQuery] ResetPasswordDTO resetPasswordDTO)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(resetPasswordDTO.Email);
                if (user == null)
                {
                    return BadRequest(new ResponseApi(404, "User not found"));
                }
                resetPasswordDTO.Token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(resetPasswordDTO.Token));
                var result = await _userManager.ResetPasswordAsync(user, resetPasswordDTO.Token, resetPasswordDTO.NewPassword);
                if (!result.Succeeded)
                {
                    return BadRequest(new ResponseApi(400, "Invalid Attemp !"));
                }
                return Ok(new ResponseApi(200, "Password Reset Successfully"));
            }
            return BadRequest(new ResponseApi(400, "Invalid Model State"));
        }
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO changePasswordDTO)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(changePasswordDTO.Email);
                if (user == null)
                {
                    return BadRequest(new ResponseApi(404, "User not found"));
                }
                var result = await _userManager.ChangePasswordAsync(user, changePasswordDTO.CurrentPassword, changePasswordDTO.NewPassword);
                if (!result.Succeeded)
                {
                    return BadRequest(new ResponseApi(400, "Invalid Attemp !"));
                }
                return Ok(new ResponseApi(200, "Password Changed Successfully"));
            }
            return BadRequest(new ResponseApi(400, "Invalid Model State"));
        }
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _signInManager.SignOutAsync();
                return Ok(new ResponseApi(200, "Logout Successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApi(500, ex.Message));
            }
        }

        [HttpGet("get-user-roles")]
        [Authorize]
        public async Task<IActionResult> GetUserRoles()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NotFound(new ResponseApi(404, "User not found"));
                }
                var roles = await _userManager.GetRolesAsync(user);
                return Ok(new
                {
                    roles
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApi(500, ex.Message));
            }
        }
      

    }
}
