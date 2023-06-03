using AspMvcUdemyPractice.Data.Data;
using AspMvcUdemyPractice.Data.Repository.IRepository;
using AspMvcUdemyPractice.DataAccess.Repository;
using AspMvcUdemyPractice.DataAccess.Repository.IRepository;
using AspMvcUdemyPractice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspMvcUdemyPractice.Data.Repository
{
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        private ApplicationDbContext _db;
        public ShoppingCartRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(ShoppingCart obj)
        {
            _db.ShoppingCarts.Update(obj);
        }
    }
}
