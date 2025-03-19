using AutoMapper;
using Ecom.Core.DTO;
using Ecom.Core.Entites.Product;

namespace Ecom.ApI.Mapping
{
    public class ArticleMapping:Profile
    {
        public ArticleMapping()
        {
            CreateMap<Article, ArticleDTOWithImageFeilds>().ForMember(e=>e.BaseImageUrl,op=>op.Ignore())
               
                .ReverseMap();
            CreateMap<Article, UpdatedArticleDtO>().ReverseMap();
            CreateMap<Article, ArticleDTO>().ReverseMap();


        }
    }
}
