using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspMvcUdemyPractice.Data.DbInitializer
{
    public interface IDbInitializer
    {
        void Initialize(); // this method will be responsible for creating admin, user and roles of our website
    }
}
