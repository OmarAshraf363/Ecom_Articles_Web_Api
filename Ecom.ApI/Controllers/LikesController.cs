using AutoMapper;
using Ecom.ApI.Helper;
using Ecom.Core.DTO;
using Ecom.Core.Entites.Product;
using Ecom.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecom.ApI.Controllers
{

    public class LikesController : BaseController
    {
        public LikesController(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }
        [HttpGet("get-article-likes/{articleId}")]
        public async Task<IActionResult> GetArticleLikes(int articleId)
        {
            try
            {
                var articleLikes = await _unitOfWork.LikeRepo.GetAllAsyncWithModify(x => x.ArticleId == articleId,e=>e.User, e => e.Article);
                if (articleLikes.Count <= 0)
                    return BadRequest(new ResponseApi(400));

                return Ok(articleLikes.Select(e => new
                {
                    isLike = e.IsLiked,
                    count = articleLikes.Count,
                 user = new {
                     displayName = e.User.DisplayName,
                     email = e.User.Email,
                     userName = e.User.UserName,
                     picImage = e.User.PicImage ?? null,
                     phoneNumber = e.User.PhoneNumber ?? null,

                 }
                }));

            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseApi(500, ex.Message));


            }
        }

        [HttpPost("toggle-like")]
        public async Task<IActionResult> ToggleLike([FromBody] LikeDTO likeDTo)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                    return Unauthorized(new ResponseApi(401));


                var existingLike = (await _unitOfWork.LikeRepo.GetAllAsyncWithModify(x => x.ArticleId == likeDTo.ArticleId && x.UserId == userId)).FirstOrDefault();
                //if the user has already liked the article, then remove the like
                if (existingLike != null)
                {
                    await _unitOfWork.LikeRepo.DeleteAsync(existingLike.Id);
                    return Ok(new ResponseApi(200,"User dislike this article"));
                }
                var like = _mapper.Map<Like>(likeDTo);
                like.UserId=userId;
                await _unitOfWork.LikeRepo.AddAsync(like);
                await _unitOfWork.Commit();
                return Ok(new ResponseApi(201,"User like this article"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseApi(500, ex.Message));
            }
        }
    }
}
