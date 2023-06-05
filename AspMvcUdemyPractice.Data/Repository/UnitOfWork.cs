using AspMvcUdemyPractice.DataAccess.Repository.IRepository;
using AspMvcUdemyPractice.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspMvcUdemyPractice.Data.Repository.IRepository;
using AspMvcUdemyPractice.Data.Repository;
using AspMvcUdemyPractice.Models;

namespace AspMvcUdemyPractice.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public ICategoryRepository Category { get; private set; }
        public IProductCategoryRepository ProductCategory { get; private set; }
        public ICompanyRepository CompanyCategory { get; private set; }
        public IShoppingCartRepository ShoppingCartCategory { get; private set; }
        public IApplicationUserRepository ApplicationUser { get; private set; }
        public IOrderHeaderRepository OrderHeaderCategory { get; private set; }
        public IOrderDetailRepository OrderDetailCategory { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            ApplicationUser = new ApplicationUserRepository(_db);    
            Category = new CategoryRepository(_db);
            ProductCategory = new ProductCategoryRepository(_db);
            CompanyCategory = new CompanyRepository(_db);
            ShoppingCartCategory = new ShoppingCartRepository(_db);
            OrderDetailCategory = new OrderDetailRepository(_db);
            OrderHeaderCategory = new OrderHeaderRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
