using AutoMapper;
using Ecom.Core.DTO;
using Ecom.Core.Entites.Product;
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecom.infrastructure.Data;
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
    }
}
