using AutoMapper;
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
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly IImageMangmentService _imageMangmentService;
        private readonly IMapper _mapper;

        public UnitOfWork(AppDbContext context,IImageMangmentService imageMangmentService,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

            _imageMangmentService = imageMangmentService;
            CategoryRepo = new CategoryRepo(_context);
            ArticleCategoryRepo = new ArticleCategoryRepo(_context);
            ArticleRepository = new ArticleRepo(_context,_imageMangmentService,_mapper);
            ImageRepo = new ImageRepo(_context);
            LikeRepo = new LikeRepo(_context);
            CommentRepo = new CommentRepo(_context);
            ArticleRowRepo = new ArticleRowRepo(_context,   _imageMangmentService);


        }

        public ICategoryRepository CategoryRepo { get; }

        public IArticleCategoryRepo ArticleCategoryRepo { get; }

        public IArticleRepository ArticleRepository { get; }

        public IImageRepo ImageRepo { get; }

        public ILikeRepo LikeRepo { get; }

        public ICommentRepo CommentRepo { get; }
        public IArticleRow ArticleRowRepo { get; }

        public async Task Commit()
        {
            await _context.SaveChangesAsync();
        }
    }
}
