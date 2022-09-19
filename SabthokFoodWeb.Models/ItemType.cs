using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SabthokFoodWeb.Models
{
    public class ItemType
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Item")]
        [Required]
        [MaxLength(50)]

        public string Name { get; set; }
    }
}
