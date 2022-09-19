using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SabthokFoodWeb.Models
{
    public class ShoppingCart
    {
        public Products product { get; set; }

        [Range(1,1000,ErrorMessage ="Enter between 1 and 1000")]

        public int Count { get; set; }
    }
}
