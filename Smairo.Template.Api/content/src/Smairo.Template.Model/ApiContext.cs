using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Smairo.Template.Model.Entities;
namespace Smairo.Template.Model
{
    // Add-Migration Initial -Context ApiContext -o Migrations/ApiContext
    // Add-Migration Initial -o Migrations
    // Scaffold-DbContext 'Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Chinook' Microsoft.EntityFrameworkCore.SqlServer -ContextDir "" -OutputDir Entities/Chinook
    public class ApiContext : DbContext
    {
        public static string UsedSchema => "dbo";

        public DbSet<MyEntity> MyEntities { get; set; }
        public ApiContext(DbContextOptions<ApiContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
            {
                return;
            }

            optionsBuilder
                .UseSqlServer(
                    "Server=(localdb)\\mssqllocaldb;Database=ReportingServices;Trusted_Connection=True;MultipleActiveResultSets=true",
                    sqlOptions => sqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, UsedSchema)
                );         
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(UsedSchema);
            modelBuilder.Entity<MyEntity>(e =>
            {
                e.ToTable(nameof(MyEntity), UsedSchema);
                e.HasKey(p => p.Id);
            });
        }
    }
}
