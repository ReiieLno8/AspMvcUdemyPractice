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
    internal class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private ApplicationDbContext _db;
        public CompanyRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
            public void Update(Company obj)
        {
            _db.Companies.Update(obj);
        }
    }
}
