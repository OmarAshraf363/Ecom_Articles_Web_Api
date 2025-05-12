using AutoMapper;
using Ecom.ApI.Helper;
using Ecom.Core.DTO;
using Ecom.Core.Entites.Product;
using Ecom.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecom.ApI.Controllers
{

    public class UsersController : BaseController
    {
        private readonly UserManager<AppUser> _userManger;
        private readonly SignInManager<AppUser> _signInManager;

        public UsersController(IUnitOfWork unitOfWork, IMapper mapper, UserManager<AppUser> userManger, SignInManager<AppUser> signInManager) : base(unitOfWork, mapper)
        {
            _userManger = userManger;
            _signInManager = signInManager;
        }

        [HttpGet("get-users")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var listOfUsers= await _userManger.Users.ToListAsync();
                return Ok(new
                {
                    data = listOfUsers.Select(e => new
                    {
                        e.UserName,
                        e.Id,
                        e.DisplayName,
                        e.Email,
                        e.PicImage,
                        locked=e.LockoutEnd==null?false:true,

                    }),
                    count=listOfUsers.Count()
                });
            }
            catch (Exception ex)
            {

                return StatusCode(500, new ResponseApi(500, ex.Message));
            }
        }

        [HttpPost("lock-user")]
        public async Task<IActionResult> LockUser(string userId)
        {
            try
            {
                var user=await _userManger.FindByIdAsync(userId);
                if (user == null)
                    return BadRequest(new ResponseApi(400, "User Not Found"));

                var result=await _userManger.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddYears(100));
            
                if (result.Succeeded)
                    return Ok(new ResponseApi(200,"User is loucked"));
                return BadRequest(new ResponseApi(400,"SomeThing went wrong")); 
            
            }
            catch (Exception ex)
            {

                return StatusCode(500, new ResponseApi(500, ex.Message));
            }


        }


        [HttpPost("unlock-user")]
        public async Task<IActionResult> UnLockUser(string userId)
        {
            try
            {
                var user = await _userManger.FindByIdAsync(userId);
                if (user == null)
                    return BadRequest(new ResponseApi(400, "User Not Found"));
                var result = await _userManger.SetLockoutEndDateAsync(user, null);

                if (result.Succeeded)
                    return Ok(new ResponseApi(200, "User is unLoucked"));
                return BadRequest(new ResponseApi(400, "SomeThing went wrong"));


            }
            catch (Exception ex)
            {

                return StatusCode(500, new ResponseApi(500, ex.Message));
            }
        }

        [HttpGet("get-user-status")]
        public async Task<IActionResult> GetUserLockStatus(string userId)
        {
            try
            {
                var user = await _userManger.FindByIdAsync(userId);
                if (user == null)
                    return BadRequest(new ResponseApi(400, "User Not Found"));
                return Ok(new {status=await _userManger.IsLockedOutAsync(user)});

            }
            catch (Exception ex)
            {

                return StatusCode(500, new ResponseApi(500, ex.Message));
            }
        }

        [HttpGet("get-by-id")]
        public async Task<IActionResult> GetUserById(string userId)
        {
            try
            {
                var user = await _userManger.FindByIdAsync(userId);
                if (user == null)
                    return BadRequest(new ResponseApi(400, "User Not Found"));

                var userArticles=await _unitOfWork.ArticleRepository.GetAllAsyncWithModify(e=>e.AppUserId==userId
                ,e=>e.Comments
                ,e=>e.Likes,
                e=>e.ArticleRows);
                var userArticlesDto =  _unitOfWork.ArticleRepository.MapAriclesToArticlesDTO(userArticles );
                return Ok(new { data=userArticlesDto, userInfo= new{
                    user.DisplayName,
                    user.UserName,
                    user.PicImage,
                    user.Id,
                
                }} );
            }
            catch (Exception ex )
            {

                return StatusCode(500, new ResponseApi(500, ex.Message));
            }
        }
    }
}
