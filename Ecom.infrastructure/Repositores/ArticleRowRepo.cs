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
    public class ArticleRowRepo : GenralRepository<ArticleRow>, IArticleRow
    {
        private readonly IImageMangmentService _imageMangmentService;

        public ArticleRowRepo(AppDbContext context, IImageMangmentService imageMangmentService) : base(context)
        {
            _imageMangmentService = imageMangmentService;
        }
        public async Task<ArticleRow> AddAsync(ArticleRowDTOWithImageFeilds model)
        {
           
            if (model.Image!=null)
            {
                var imagePath = await _imageMangmentService
               .UploadImageAsync(model.Image, "rowsImage");
                var articleRow = new ArticleRow()
                {
                    Text = model.Text,
                    ArticleId = int.Parse( model.ArticleId),
                    Image = imagePath

                };
                return articleRow;
            }
            else
            {
                var articleRow = new ArticleRow()
                {
                    Text = model.Text,
                    ArticleId = int.Parse(model.ArticleId),
                };
                return articleRow;
            }

        }
    }
}
