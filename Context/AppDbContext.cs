using Microsoft.EntityFrameworkCore;
using OnlineLibraryAspNet.Models;


namespace OnlineLibraryAspNet.Context
{
    public class AppDbContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public DbSet<Author> Authors { get; set; }
        public DbSet<Books> Books { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<WorkTable> WorkTablesLibrary { get; set; }

        string connectionString;
        public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
            connectionString = configuration.GetConnectionString("NpgSqlString");
            var orther = configuration.GetConnectionString("Other");
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(connectionString);
        }
    }
}
