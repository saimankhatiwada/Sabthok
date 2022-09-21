using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SabthokFoodWeb.Models.ViewModel
{
	public class ShoppingCartVM
	{
		public IEnumerable<ShoppingCart> ListCart { get; set; }

		public OrderHeader OrderHeader { get; set; }
	}
}
