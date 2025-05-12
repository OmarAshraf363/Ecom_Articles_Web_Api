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

    public class CommentsController : BaseController
    {
        public CommentsController(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }
        [HttpGet("get-article-comments/{articleId}")]
        public async Task<IActionResult> GetArticleComments(int articleId)
        {
            try
            {
                var articleComments = await _unitOfWork.CommentRepo.GetAllAsyncWithModify(x => x.ArticleId == articleId,e=>e.User, e => e.Article);
                if (articleComments.Count <= 0)
                    return Ok(new ResponseApi(201, "Not Found Comments"));
              
                return Ok(new
                    {
                        count = articleComments.Count,
                        comments =  articleComments.Select(c => new
                        {
                            id = c.Id,
                            content = c.Content,
                            articleId = c.ArticleId,
                            createdAt = c.CreatedAt,
                            user = new
                            {
                                displayName = c.User.DisplayName,
                                email =         c.User.Email,
                                picImage = c.User.PicImage,
                                userName = c.User.UserName,
                            }
                        })


                    });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseApi(500, ex.Message));


            }
        }

        [HttpPost("add-comment")]
        public async Task<IActionResult> AddComment([FromBody] CommentDTO commentDTO)
        {
            try
            {
                var userId=User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                    return BadRequest(new ResponseApi(400, "User not found"));

                var comment = _mapper.Map<Comment>(commentDTO);
                comment.UserId = userId;
                await _unitOfWork.CommentRepo.AddAsync(comment);
                return Ok(new ResponseApi(201, "Comment added successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseApi(500, ex.Message));
            }
        }

        [HttpDelete("delete-comment/{commentId}")]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            try
            {
                var comment = await _unitOfWork.CommentRepo.GetByIdAsync(commentId);
                if (comment == null)
                    return BadRequest(new ResponseApi(400, "Comment not found"));
                await _unitOfWork.CommentRepo.DeleteAsync(commentId);
                return Ok(new ResponseApi(200, "Comment deleted successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseApi(500, ex.Message));
            }
        }

        [HttpPut("update-comment")]
        public async Task<IActionResult> UpdateComment([FromBody] UpdatedCommentDTO updatedCommentDTO)
        {
            try
            {
                var existingComment = await _unitOfWork.CommentRepo.GetByIdAsync(updatedCommentDTO.Id);
                if (existingComment == null)
                    return BadRequest(new ResponseApi(400, "Comment not found"));

               existingComment.Content = updatedCommentDTO.Content;
                await _unitOfWork.CommentRepo.UpdateAsync(existingComment);
                return Ok(new ResponseApi(200, "Comment updated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseApi(500, ex.Message));
            }
        }
    }
}
