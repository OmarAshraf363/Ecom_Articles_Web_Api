using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Entites.Product
{
  public  class ArticleRow:BaseEntity<int>
    {
        public string Text { get; set; }
        public string? Image { get; set; }

        public int ArticleId { get; set; }
        [ForeignKey("ArticleId")]
        public Article Article { get; set; }
    }
}
