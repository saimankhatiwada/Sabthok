using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SabthokFoodWeb.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepository Category { get; }
        IItemTypeRepository CoverType { get; }

        IProductTypeRepository Product { get; }

        ICompanyRepository Company { get; }

        IShoppingCartRepository ShoppingCart { get; }

        IApplicationUsersRepository ApplicationUsers { get; }

        IOrderHeaderRepository OrderHeader { get; }

        IOrderDetailsRepository OrderDetails { get; }

        void Save();
    }
}
