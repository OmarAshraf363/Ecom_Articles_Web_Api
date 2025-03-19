using AutoMapper;
using Ecom.Core.DTO;
using Ecom.Core.Entites.Product;

namespace Ecom.ApI.Mapping
{
    public class ArticleRowMapping:Profile
    {
        public ArticleRowMapping()
        {
            CreateMap<ArticleRow, ArticleRowDTO>().ReverseMap();
            CreateMap<ArticleRow, UpdatedArticleDtO>().ReverseMap();
        }
    }
}
