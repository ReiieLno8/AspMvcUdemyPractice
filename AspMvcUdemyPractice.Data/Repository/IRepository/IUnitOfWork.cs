using AspMvcUdemyPractice.Data.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspMvcUdemyPractice.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepository Category { get; }
        IProductCategoryRepository ProductCategory { get; }
        ICompanyRepository CompanyCategory { get; }
        IShoppingCartRepository ShoppingCartCategory { get; }
        IApplicationUserRepository ApplicationUser { get; }
        IOrderDetailRepository OrderDetailCategory { get; }
        IOrderHeaderRepository OrderHeaderCategory { get; }

        void Save();
    }
}
