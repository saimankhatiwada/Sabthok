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
	public class OrderDetails
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public int OrderId { get; set; }
		[ForeignKey("OrderId")]
		[ValidateNever]

		public OrderHeader OrderHeader { get; set; }

		[Required]
		public int ProductId { get; set; }

		[ForeignKey("ProductId")]
		[ValidateNever]

		public Products Products { get; set; }

		public int Count { get; set; }

		public double Price { get; set; }
	}
}
