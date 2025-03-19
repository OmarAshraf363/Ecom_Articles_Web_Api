using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Entites.Product
{
   public class ArticleCategory
    {
        public int ArticleId { get; set; }
        [ForeignKey(nameof(ArticleId))]
        public virtual Article Article { get; set; }

        public int CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]

        public virtual Category Category { get; set; }
    }
}
