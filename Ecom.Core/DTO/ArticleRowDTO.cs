using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.DTO
{
    public record ArticleRowDTO(int Id,string? Text, string? Image,int ArticleId);
    public record UpdatedArticleRowDTO(string Id, string Text, IFormFile? Image);
    public record ArticleRowDTOWithImageFeilds(string ArticleId, string? Text, IFormFile? Image);

}
