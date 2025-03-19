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
   public class LikeRepo : GenralRepository<Like>, ILikeRepo
    {
        public LikeRepo(AppDbContext context) : base(context)
        {
        }
    }
}
