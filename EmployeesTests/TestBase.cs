using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RedarborWebApiTest.Data;

namespace EmployeesTests
{
    public class TestBase
    {
        public static IConfiguration InitConfig()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.test.json")
                .AddEnvironmentVariables()
                .Build();
            return config;
        }
        protected static AppDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("Redarbor").Options;                
            var dbContext = new AppDbContext(options);
            return dbContext;
        }
    }
}
