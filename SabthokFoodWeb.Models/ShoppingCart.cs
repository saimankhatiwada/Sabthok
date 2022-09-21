using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SabthokFoodWeb.Models
{
    public class ShoppingCart
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        [ValidateNever]
        public Products Product { get; set; }

        [Range(1,1000,ErrorMessage ="Enter between 1 and 1000")]

        public int Count { get; set; }

        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public ApplicationUsers ApplicationUser { get; set; }

        [NotMapped]
        public double Price { get; set; }
    }
}
