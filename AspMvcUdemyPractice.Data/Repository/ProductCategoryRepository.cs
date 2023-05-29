using AspMvcUdemyPractice.Data.Data;
using AspMvcUdemyPractice.Data.Repository.IRepository;
using AspMvcUdemyPractice.DataAccess.Repository;
using AspMvcUdemyPractice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspMvcUdemyPractice.Data.Repository
{
    public class ProductCategoryRepository : Repository<Product>, IProductCategoryRepository
    {
        private ApplicationDbContext _db;
        public ProductCategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Product obj)
        {
            var objFromDb = _db.Products.FirstOrDefault(u => u.Id == obj.Id);//explicitly updating product details
            if (objFromDb != null)
            { 
                objFromDb.Title = obj.Title;
                objFromDb.Description = obj.Description;
                objFromDb.Author = obj.Author;
                objFromDb.Category = obj.Category;
                objFromDb.ListPrice = obj.ListPrice;
                objFromDb.Price = obj.Price;
                objFromDb.Price100 = obj.Price100;
                objFromDb.Price50 = obj.Price50;
                objFromDb.ISBN = obj.ISBN;

                if (obj.ImageUrl != null)
                { 
                    objFromDb.ImageUrl = obj.ImageUrl;
                }
            }
        }
    }
}
