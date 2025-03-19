using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Entites.Product
{
   public class AppUser:IdentityUser
    {
        public string DisplayName  { get; set; }
        public string PicImage { get; set; }
        public Address Address { get; set; }

        //relations
        public virtual ICollection<Article> Articles { get; set; }= new List<Article>();
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public virtual ICollection<Like> Likes { get; set; }= new List<Like>();
    }


}
