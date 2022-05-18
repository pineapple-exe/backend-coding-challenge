using Microsoft.EntityFrameworkCore;
using UsaPopulation.Data.Configurations;
using UsaPopulation.Domain.Entities;

namespace UsaPopulation.Data
{
    public class UsaPopulationDbContext : DbContext
    {
        private readonly string _connectionString = "Server=localhost;Database=UsaPopulationApi;Trusted_Connection=True;";

        public DbSet<QueryLog> QueryLogs { get; set; }

        public UsaPopulationDbContext()
        {

        }

        public UsaPopulationDbContext(DbContextOptions<UsaPopulationDbContext> contextOptions) : base(contextOptions)
        {

        }

        public UsaPopulationDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new QueryLogsConfiguration());
        }
    }
}