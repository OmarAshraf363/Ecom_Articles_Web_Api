using AutoMapper;
using Ecom.ApI.Helper;
using Ecom.Core.DTO;
using Ecom.Core.Entites.Product;
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecom.Core.Sharing;
using Ecom.infrastructure.Repositores.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Ecom.ApI.Controllers
{
    //[Authorize]
    public class ArticleController : BaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IImageMangmentService _imageMangmentService;
        public ArticleController(IUnitOfWork unitOfWork, IMapper mapper, UserManager<AppUser> userManager, IImageMangmentService imageMangmentService) : base(unitOfWork, mapper)
        {
            _userManager = userManager;
            _imageMangmentService = imageMangmentService;
        }
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll([FromQuery] ArticleParams articleParams)
        {
            try
            {
                var listOfArticls = await _unitOfWork.ArticleRepository
                    .GetAllAsync(articleParams);
                //if (listOfArticls.Count() == 0)
                //{
                //    return BadRequest(new ResponseApi(400));
                //}
                int totalCount = await _unitOfWork.ArticleRepository.CountAsunc();
                return Ok(new Pagination<ArticleDTO>(articleParams.pageSize, articleParams.PageNumber, totalCount, listOfArticls));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseApi(500, ex.Message));
            }
        }
        [HttpGet("get-user-article")]
        public async Task<IActionResult> GetUserArticles([FromQuery]ArticleParams articleParams)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null) 
                    return BadRequest(new ResponseApi(400,"User Not Found To Get Articles"));
                articleParams.UserId = userId;
                var userArticles = await _unitOfWork.ArticleRepository.GetAllAsync(articleParams);
                int totalCount = userArticles.Count();


                return Ok(new Pagination<ArticleDTO>(articleParams.pageSize, articleParams.PageNumber, totalCount, userArticles));
            }
            catch (Exception ex)
            {

                return StatusCode(500, new ResponseApi(500, ex.Message));
            }
        }

        [HttpGet("get-atricleRows-by-articleId/{articleId}")]
        public async Task<IActionResult> GetArticleRows(int articleId)
        {
            try
            {
                var articleRows = await _unitOfWork.ArticleRowRepo.GetAllAsyncWithModify(e => e.ArticleId == articleId);
                if (articleRows.Count is 0)
                {
                    return BadRequest(new ResponseApi(400, "Article dont have any rows"));
                }
                return Ok(articleRows);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseApi(500, ex.Message));
            }
        }
        [HttpGet("get-by-id/{id}")]

        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var article = await _unitOfWork.ArticleRepository
                    .GetByIdAsync(id, e => e.ArticleRows, e => e.ArticleCategories, e => e.Images, e => e.User, e => e.Likes, e => e.Comments);
                if (article is null)
                {
                    return BadRequest(new ResponseApi(400));
                }
                var articleDTO = new ArticleDTO(article.Id, article.Title, article.Description, article.BaseImageUrl, article.AppUserId, article.User.UserName, article.CreatedAt, article.Likes.Count(), article.Comments.Count(), article.ArticleRows.Select(e => new ArticleRowDTO(e.Id,e.Text, e.Image,e.ArticleId)).ToList(),article.User?.PicImage);
                return Ok(articleDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseApi(500, ex.Message));
            }
        }

        [HttpGet("get-article-row-by-id/{id}")]

        public async Task<IActionResult> GetArticleRowById(int id)
        {
            try
            {
                var articleRow = await _unitOfWork.ArticleRowRepo
                    .GetByIdAsync(id);
                if (articleRow is null)
                {
                    return BadRequest(new ResponseApi(400));
                }
                var articleRowDTO = new ArticleRowDTO(articleRow.Id,articleRow.Text,articleRow.Image,articleRow.ArticleId);
                return Ok(articleRowDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseApi(500, ex.Message));
            }
        }


        [HttpPost("add-article")]
        public async Task<IActionResult> CreateArticle([FromForm] ArticleDTOWithImageFeilds articleDTO)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (userId is null)
                {
                    return BadRequest(new ResponseApi(400, "User Id is null ,please login"));
                }


                //prepare Image and Article rows with no save only prepare
                var article = await _unitOfWork.ArticleRepository.PrepareArticleAsync(articleDTO);
                // insert User Id 
                article.AppUserId = userId;

                article.User = await _userManager.FindByIdAsync(userId);




                // save real article in data base after prepare 

                await _unitOfWork.ArticleRepository.AddAsync(article);

                return Ok(new { response = new ResponseApi(200, "Article Added Successfully"), id = article.Id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseApi(500, ex.Message));

            }
        }
        [HttpPost("add-article-row")]
        public async Task<IActionResult> AddArticleRow([FromForm] ArticleRowDTOWithImageFeilds addArticleRowDTO)
        {
            try
            {
                //only prepare no save like add-article
                var articleRow = await _unitOfWork.ArticleRowRepo.PrepareArticleRowAsync(addArticleRowDTO);
                // save real article in data base
                await _unitOfWork.ArticleRowRepo.AddAsync(articleRow);

                return Ok(new ResponseApi(200, "Article Row Added Successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseApi(500, ex.Message));
            }
        }


        [HttpPut("update-article")]
        public async Task<IActionResult> UpdateArticle(UpdateArticleDTO model)
        {
            try
            {
                var article = await _unitOfWork.ArticleRepository.GetByIdAsync(int.Parse(model.Id));
                if (article == null)
                    return BadRequest(new ResponseApi(400, "Not found this article"));
                article.Title = model.Title;
                article.Description = model.Description;
                if (model.BaseImageUrl != null && model.BaseImageUrl.Length > 0)
                {
                    //delete existing image
                    if (article.BaseImageUrl != null)
                    {
                        _imageMangmentService.DeleteImageAync(article.BaseImageUrl);
                    }
                    var newImagePath = await _imageMangmentService.UploadImageAsync(model.BaseImageUrl, "baseImages");

                        article.BaseImageUrl = newImagePath;

                }
                await _unitOfWork.ArticleRepository.UpdateAsync(article);
                return Ok(new ResponseApi(200, "Article Updated Successfully"));

            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseApi(500, ex.Message));
            }
        }
        [HttpPut("update-article-row")]
        public async Task<IActionResult> UpdateArticleRow([FromForm]UpdatedArticleRowDTO model)
        {
            try
            {

                var existingArticleRow = await _unitOfWork.ArticleRowRepo.GetByIdAsync(int.Parse( model.Id));
                if (existingArticleRow == null)
                    return BadRequest(new ResponseApi(400,"Not found articlerow"));

                existingArticleRow.Text=model.Text;
                if (model.Image!=null && model.Image.Length>0)
                {
                    if (existingArticleRow.Image!=null)
                    {
                        _imageMangmentService.DeleteImageAync(existingArticleRow.Image);
                    }
                    var newImagePath = await _imageMangmentService.UploadImageAsync(model.Image, "rowsimage");
                    existingArticleRow.Image = newImagePath;
                }
                
                await _unitOfWork.ArticleRowRepo.UpdateAsync(existingArticleRow);
                return Ok(new ResponseApi(200, "Article Row Updated Successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseApi(500, ex.Message));
            }
        }


        [HttpDelete("delete-article/{id}")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            try
            {
                var article = await _unitOfWork.ArticleRepository.GetByIdAsync(id);
                if (article == null)
                {
                    return BadRequest(new ResponseApi(400));
                }
                _unitOfWork.ArticleRepository.DeleteAsyncWithDeleteingImage(article.BaseImageUrl);
                await _unitOfWork.ArticleRepository.DeleteAsync(id);
                return Ok(new ResponseApi(200, "Article and article images Deleted Successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseApi(500, ex.Message));
            }
        }
        [HttpDelete("delete-article-row/{id}")]
        public async Task<IActionResult> DeleteArticleRow(int id)
        {
            try
            {
                await _unitOfWork.ArticleRowRepo.DeleteAsync(id);

                return Ok(new ResponseApi(200, "Article Row Deleted Successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseApi(500, ex.Message));
            }
        }

        [HttpPost("save-article")]
        public IActionResult SaveArticle([FromBody] int articleId)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                    return BadRequest(new ResponseApi(400, "User not logged in"));

                var id = articleId.ToString();

                var savedArticles = Helper.CockiesConfig.GetCookie(HttpContext, "saved");

                List<string> savedIds;
                if (string.IsNullOrWhiteSpace(savedArticles))
                {
                    savedIds = new List<string>();
                }
                else
                {
                    savedIds = savedArticles.Split(' ', StringSplitOptions.RemoveEmptyEntries).Distinct().ToList();
                }

                if (savedIds.Contains(id))
                {
                    return Conflict(new ResponseApi(409, "Article already saved"));
                }

                savedIds.Add(id);
                var updatedCookie = string.Join(" ", savedIds);
                Helper.CockiesConfig.SetCookie(HttpContext, "saved", updatedCookie);

                return Ok(new ResponseApi(200, "Article saved successfully"));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResponseApi(500, "An error occurred while saving the article"));
            }
        }

        [HttpGet("get-saved-articles")]
        public async Task<IActionResult> GetSavedArticles([FromQuery]ArticleParams articleParams)
        {
            try
            {
                List<int> intIdes=new List<int>();
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                    return BadRequest(new ResponseApi(400, "User not logged in"));

                var aticlesIdes = Helper.CockiesConfig.GetCookie(HttpContext, "saved");
                if (aticlesIdes==null)
                {
                    return Ok(new ResponseApi(400, "Not Found Saved Articles"));
                }
                var listIdes=aticlesIdes.Split(' ').ToList();

                foreach (var id in listIdes)
                {
                    intIdes.Add(int.Parse(id));
                
                }
                articleParams.SavedArticlesId = intIdes;

                var articles = await _unitOfWork.ArticleRepository.GetAllAsync(articleParams);
                int totalCount = articles.Count();


                return Ok(new Pagination<ArticleDTO>(articleParams.pageSize, articleParams.PageNumber, totalCount, articles));


            }
            catch (Exception)
            {

                return StatusCode(500, new ResponseApi(500, "An error occurred while saving the article"));

            }

        }


        [HttpPost("unsave-article")]
        public IActionResult UnSaveArticle([FromBody]int articleId)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                    return BadRequest(new ResponseApi(400, "User not logged in"));

                var ides = Helper.CockiesConfig.GetCookie(HttpContext, "saved");
                if (ides == null)
                    return Ok(new ResponseApi(400, "Not Found Saved Articles"));
                var listOfIdes = ides.Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();

                listOfIdes.RemoveAll(e => int.TryParse(e, out var id) && id == articleId);
                var updatedCookie = string.Join(" ", listOfIdes);
                Helper.CockiesConfig.SetCookie(HttpContext, "saved", updatedCookie);
                return Ok(new ResponseApi(200, "Article is Un Saved"));



            }
            catch (Exception ex)
            {
                return StatusCode(500,ex);

            }

        }




    }
}
