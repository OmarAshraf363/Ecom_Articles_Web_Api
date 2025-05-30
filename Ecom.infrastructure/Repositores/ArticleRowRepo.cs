﻿using Ecom.Core.DTO;
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
        public async Task<ArticleRow> PrepareArticleRowAsync(ArticleRowDTOWithImageFeilds model)
        {
            if (model==null)
            {
                return null;
            }

            var articleRow = new ArticleRow()
            {
                Text = model.Text,
                ArticleId = int.Parse(model.ArticleId),

            };



            if (model.Image!=null)
            {
                var imagePath = await _imageMangmentService
               .UploadImageAsync(model.Image, "rowsImage");
               articleRow.Image = imagePath;
                return articleRow;
            }
            else
            {
              
                return articleRow;
            }

        }
    }
}
