using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SabthokFoodWeb.Models;

namespace SabthokFoodWeb.DataAccess.Repository.IRepository
{
    public interface IItemTypeRepository : IRepository<ItemType>
    {
        void Update(ItemType obj);
    }
}
