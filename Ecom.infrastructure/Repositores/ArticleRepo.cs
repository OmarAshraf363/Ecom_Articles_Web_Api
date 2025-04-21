using AutoMapper;
using Ecom.Core.DTO;
using Ecom.Core.Entites.Product;
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecom.Core.Sharing;
using Ecom.infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Repositores
{
    public class ArticleRepo : GenralRepository<Article>, IArticleRepository
    {
        private readonly AppDbContext _context;
        private readonly IImageMangmentService _imageMangmentService;
        private readonly IMapper _mapper;
        public ArticleRepo(AppDbContext context, IImageMangmentService imageMangmentService, IMapper mapper) : base(context)
        {
            _imageMangmentService = imageMangmentService;
            _mapper = mapper;
            _context = context;
        }

        public async Task<Article> AddAsync(ArticleDTOWithImageFeilds articleDTO)
        {
            if (articleDTO == null) return null;
            var imagePath = await _imageMangmentService
                .UploadImageAsync(articleDTO.BaseImageUrl, articleDTO.Title);
            var article = _mapper.Map<Article>(articleDTO);
            article.BaseImageUrl = imagePath;

            return article;

        }

        public void  DeleteAsyncWithDeleteingImage(string src)
        {
             _imageMangmentService.DeleteImageAync(src);
        }

        public async Task<IEnumerable<ArticleDTO>> GetAllAsync(ArticleParams articleParams)
        {
            var query =  _context.Articles
                .Include(e => e.Images)
                .Include(e => e.ArticleCategories)
                .Include(e => e.ArticleRows).AsNoTracking();

            if (!string.IsNullOrEmpty(articleParams.Search))
            {
                var searchWords = articleParams.Search.Split(' ');
                query = query.Where(e => searchWords.All(w =>
                e.Title.ToLower().Contains(w.ToLower()) ||
                  e.Description.ToLower().Contains(w.ToLower())
                ));
              
            }
            if (articleParams.CategoryId.HasValue)
            {
                query = query.Where(e => e.ArticleCategories.Any(c => c.CategoryId == articleParams.CategoryId));
            }
            query =  query
                .Skip((articleParams.PageNumber - 1) * articleParams.pageSize)
                .Take(articleParams.pageSize);


            var result = await query.Select(e => new ArticleDTO(

                e.Id,
                e.Title,
                e.Description,
                e.BaseImageUrl
                , e.AppUserId,
                e.User.UserName,
                e.CreatedAt ,
                e.Likes.Count,
                e.Comments.Count,
                e.ArticleRows.Select(c=>new ArticleRowDTO(c.Text,c.Image)).ToList()







                )).ToListAsync();
           
            return result;
        }
    }
}
