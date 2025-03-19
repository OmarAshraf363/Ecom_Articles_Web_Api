using Ecom.Core.Entites.Product;
using Ecom.Core.Interfaces;
using Ecom.infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Repositores
{
    public class CommentRepo : GenralRepository<Comment>, ICommentRepo
    {
        public CommentRepo(AppDbContext context) : base(context)
        {
        }
    }
}
