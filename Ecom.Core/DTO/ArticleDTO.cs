using Ecom.Core.Entites.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.DTO
{
    public record ArticleDTO( int id,string Title, string Description, string BaseImageUrl,string UserId,string UserName, DateTime CreatedAt,int Likes,int Comments, List<ArticleRowDTO>? ArticleRows);
    public record UpdatedArticleDtO(int Id , string Title, string Description, string BaseImageUrl, List<UpdatedArticleRowDTO>? UpdatedRows);

    public class ArticleDTOWithImageFeilds
    {
        public string Title { get; set; }
        public string Description { get; set; }
       
        public IFormFile BaseImageUrl { get; set; }
        public List<ArticleRowDTO>? ArticleRows { get; set; }
    }
}
