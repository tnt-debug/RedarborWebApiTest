using Microsoft.EntityFrameworkCore;
using RedarborWebApiTest.Models;

namespace RedarborWebApiTest.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Employee> Employees { get; set; }
    }
}
