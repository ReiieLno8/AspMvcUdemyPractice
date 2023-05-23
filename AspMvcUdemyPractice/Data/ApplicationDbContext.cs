using AspMvcUdemyPractice.Models;
using Microsoft.EntityFrameworkCore;

namespace AspMvcUdemyPractice.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
            
        }

        public DbSet<Category> Categories { get; set; }
    }
}
