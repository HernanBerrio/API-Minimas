using Microsoft.EntityFrameworkCore;
using MinimalWebAPIPosgresSQL.Models;

namespace MinimalWebAPIPosgresSQL.Data
{
    public class OfficeDb : DbContext
    {
        public OfficeDb(DbContextOptions<OfficeDb> options) : base(options)
        {

        }

        public DbSet<Employee> Employees => Set<Employee>();
    }
}
