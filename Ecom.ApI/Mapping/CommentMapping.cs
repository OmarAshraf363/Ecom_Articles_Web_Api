using AutoMapper;
using Ecom.Core.DTO;
using Ecom.Core.Entites.Product;

namespace Ecom.ApI.Mapping
{
    public class CommentMapping:Profile
    {
        public CommentMapping()
        {
            CreateMap<Comment, CommentDTO>().ReverseMap();
        }
    }
}
