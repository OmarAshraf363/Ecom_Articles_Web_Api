using AutoMapper;
using Ecom.ApI.Helper;
using Ecom.Core.DTO;
using Ecom.Core.Entites.Product;
using Ecom.Core.Interfaces;
using Ecom.Core.Sharing;
using Ecom.infrastructure.Repositores.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.ApI.Controllers
{

    public class ArticleController : BaseController
    {
        public ArticleController(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll([FromQuery]ArticleParams articleParams)
        {
            try
            {
                var listOfArticls = await _unitOfWork.ArticleRepository
                    .GetAllAsync(articleParams);
                if (listOfArticls.Count()==0)
                {
                    return BadRequest(new ResponseApi(400));
                }
                int totalCount = await _unitOfWork.ArticleRepository.CountAsunc();
                return Ok(new Pagination<ArticleDTO>(articleParams.pageSize,articleParams.PageNumber,totalCount,listOfArticls));
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
                    return BadRequest(new ResponseApi(400,"Article dont have any rows"));
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
                    .GetByIdAsync(id,e=>e.ArticleRows,e=>e.ArticleCategories,equals=>equals.Images);
                if (article is null)
                {
                    return BadRequest(new ResponseApi(400));
                }
                return Ok(article);
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

               var article= await _unitOfWork.ArticleRepository.AddAsync(articleDTO);
                if (articleDTO. ArticleRows !=null)
                {
                    article.ArticleRows = articleDTO. ArticleRows.Select(e => new ArticleRow()
                    {
                        Text = e.Text,
                        Image = e.Image,
                        ArticleId = article.Id
                    }).ToList();
                }
                await _unitOfWork.ArticleRepository.AddAsync(article);
                return Ok(new ResponseApi(200,"Article Added Successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseApi(500,ex.Message));

            }
        }
        [HttpPost("add-article-row")]
        public async Task<IActionResult> AddArticleRow(ArticleRowDTO addArticleRowDTO)
        {
            try
            {
                //var articleRow = new ArticleRow()
                //{
                //    ArticleId = addArticleRowDTO.ArticleId,
                //    Text = addArticleRowDTO.Text,
                //    Image = addArticleRowDTO.Image
                //};
                var articleRow = _mapper.Map<ArticleRow>(addArticleRowDTO);
                await _unitOfWork.ArticleRowRepo.AddAsync(articleRow);
                return Ok(new ResponseApi(200, "Article Row Added Successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseApi(500, ex.Message));
            }
        }


        [HttpPut("update-article")]
        public async Task<IActionResult> UpdateArticle(UpdatedArticleDtO updatedArticleDTO)
        {
            try
            {
                try
                {
                    var article =  _mapper.Map<Article>(updatedArticleDTO);
                    //Manual adding of ArticleRows
                    if (updatedArticleDTO.UpdatedRows != null)
                    {
                            var updatedArticleRow=updatedArticleDTO.UpdatedRows.Select(e => new ArticleRow()
                            {
                                Id = e.Id,
                                Text = e.Text,
                                Image = e.Image,
                                ArticleId = article.Id,
                            }).ToList();
                        if (updatedArticleRow != null)
                        {
                            foreach (var articleRow in updatedArticleRow)
                            {
                                await _unitOfWork.ArticleRowRepo.UpdateAsync(articleRow);
                            }
                        }
                    }

                    await _unitOfWork.ArticleRepository.UpdateAsync(article);
                    return Ok(new ResponseApi(200,"Article Updated Successfully"));


                }
                catch (Exception ex)
                {

                    return BadRequest(new ResponseApi(400, "Faild Map and Update" + $":{ex.Message}"));
                }


            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseApi(500, ex.Message));
            }
        }
        [HttpPut("update-article-row")]
        public async Task<IActionResult> UpdateArticleRow(UpdatedArticleRowDTO uppdatedArticleRowDTO)
        {
            try
            {
                var existingArticleRow = await _unitOfWork.ArticleRowRepo.GetByIdAsync(uppdatedArticleRowDTO.Id);


                var articleRow = new ArticleRow()
                {
                    ArticleId = existingArticleRow.ArticleId,
                    Id = existingArticleRow.Id,
                    Text = uppdatedArticleRowDTO.Text,
                    Image = uppdatedArticleRowDTO.Image
                };
                await _unitOfWork.ArticleRowRepo.UpdateAsync(articleRow);
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
                _unitOfWork.ArticleRepository.DeleteAsyncWithDeleteingImage(article.Title);
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
    }
}
