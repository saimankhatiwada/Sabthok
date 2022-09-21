using SabthokFoodWeb.DataAccess.Repository.IRepository;
using SabthokFoodWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SabthokFoodWeb.DataAccess.Repository
{
    public class ApplicationUsersRepository : Repository<ApplicationUsers>, IApplicationUsersRepository
    {
        private readonly ApplicationDbContext _context;
        public ApplicationUsersRepository(ApplicationDbContext context): base(context)
        {
            _context = context;
        }
    }
}
