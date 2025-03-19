﻿using Ecom.Core.Entites.Product;
using Ecom.Core.Interfaces;
using Ecom.infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Repositores
{
   public class ArticleCategoryRepo:GenralRepository<ArticleCategory>, IArticleCategoryRepo
    {
        public ArticleCategoryRepo(AppDbContext context) : base(context)
        {
        }
    }
  
}
