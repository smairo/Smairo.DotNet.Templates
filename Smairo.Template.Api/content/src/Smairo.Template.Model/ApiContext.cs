using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Smairo.Template.Model.Entities;
namespace Smairo.Template.Model
{
    // Add-Migration Initial -Context ApiContext -o Migrations
    // Add-Migration Initial -o Migrations
    // Scaffold-DbContext 'Server=(localdb)\\mssqllocaldb;Database=MyDb' Microsoft.EntityFrameworkCore.SqlServer -ContextDir "" -OutputDir Entities
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
                    "Server=(localdb)\\mssqllocaldb;Database=MyDb",
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
