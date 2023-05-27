using AspMvcUdemyPractice.Data.Data;
using AspMvcUdemyPractice.Data.Repository.IRepository;
using AspMvcUdemyPractice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspMvcUdemyPractice.Data.Repository
{
    public class ProductCategoryRepository : ProductRepository<Product>, IProductCategoryRepository
    {
        private ApplicationDbContext _db;
        public ProductCategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Product obj)
        {
            _db.Products.Update(obj);
        }
    }
}
