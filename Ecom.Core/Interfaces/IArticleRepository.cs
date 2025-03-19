using Ecom.Core.DTO;
using Ecom.Core.Entites.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Interfaces
{
   public interface IArticleRepository: IGenricRepository<Article>
    {
      public Task<Article> AddAsync(ArticleDTOWithImageFeilds articleDTO);
        public void DeleteAsyncWithDeleteingImage(string src);
    }
}
