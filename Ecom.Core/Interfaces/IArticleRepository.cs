using Ecom.Core.DTO;
using Ecom.Core.Entites.Product;
using Ecom.Core.Sharing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Interfaces
{
   public interface IArticleRepository: IGenricRepository<Article>
    {
        Task<IEnumerable<ArticleDTO>> GetAllAsync(ArticleParams articleParams);
      public Task<Article> AddAsync(ArticleDTOWithImageFeilds articleDTO);
        public void DeleteAsyncWithDeleteingImage(string src);
    }
}
