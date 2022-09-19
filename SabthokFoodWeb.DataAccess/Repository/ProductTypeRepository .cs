using SabthokFoodWeb.DataAccess.Repository.IRepository;
using SabthokFoodWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SabthokFoodWeb.DataAccess.Repository
{
    public class ProductTypeRepository : Repository<Products>, IProductTypeRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductTypeRepository(ApplicationDbContext context): base(context)
        {
            _context = context;
        }
        public void Update(Products obj)
        {
            var objToUpdate = _context.Products.FirstOrDefault(u => u.Id == obj.Id);
            if(objToUpdate != null)
            {
                objToUpdate.Title = obj.Title;
                objToUpdate.Description = obj.Description;
                objToUpdate.Price = obj.Price;
                objToUpdate.Category = obj.Category;
                objToUpdate.CategoryId = obj.CategoryId;
                objToUpdate.CoverTypeId = obj.CoverTypeId;
                objToUpdate.ListPrice = obj.ListPrice;
                objToUpdate.Price50 = obj.Price50;
                objToUpdate.Price100 = obj.Price100;
                objToUpdate.CoverType = obj.CoverType;
                if(objToUpdate.ImageUrl != null)
                {
                    objToUpdate.ImageUrl = obj.ImageUrl;
                }
            }
        }
    }
}
