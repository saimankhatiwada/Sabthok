using SabthokFoodWeb.DataAccess.Repository.IRepository;
using SabthokFoodWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SabthokFoodWeb.DataAccess.Repository
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private readonly ApplicationDbContext _context;
        public CompanyRepository(ApplicationDbContext context): base(context)
        {
            _context = context;
        }
        public void Update(Company obj)
        {
            _context.Companies.Update(obj);
        }
    }
}
