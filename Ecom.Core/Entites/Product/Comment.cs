using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Entites.Product
{
   public class Comment:BaseEntity<int>
    {
        public string Content { get; set; }

        //relations
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual AppUser User { get; set; } 
        public int ArticleId { get; set; }
        [ForeignKey(nameof(ArticleId))]
        public virtual Article Article { get; set; }
    }
}
