using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.DTO
{
    public record CommentDTO(int ArticleId, string Content, string UserId);
    public record UpdatedCommentDTO(int Id, string Content);

}
