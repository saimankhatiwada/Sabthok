using SabthokFoodWeb.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SabthokFoodWeb.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Category = new CategoryRepository(_context);
            CoverType = new ItemTypeRepository(_context);
            Product = new ProductTypeRepository(_context);
        }
        public ICategoryRepository Category { get; private set; }
        public IItemTypeRepository CoverType { get; private set; }
        public IProductTypeRepository Product { get; private set; }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
