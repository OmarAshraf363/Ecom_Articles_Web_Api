using AutoMapper;
using Ecom.Core.DTO;
using Ecom.Core.Entites.Product;

namespace Ecom.ApI.Mapping
{
    public class LikeMapping:Profile
    {
        public LikeMapping()
        {
            CreateMap<LikeDTO, Like>().ReverseMap();
        }
    }
}
