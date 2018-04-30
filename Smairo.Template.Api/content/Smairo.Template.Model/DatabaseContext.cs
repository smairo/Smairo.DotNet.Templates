using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
namespace Smairo.Template.Model
{
    /// <summary>
    /// This is for CF migrations only. Querying and data manipulation happens in repository (that is using Dapper for queryengine)
    /// </summary>
    public class DatabaseContext : DbContext
    {
        private const string Schema = "MySchema";

        public DatabaseContext(DbContextOptions options)
            : base(options)
        {
        }

        /// <summary>
        /// Hack to make sure that CLI CF can be done.
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured) return;
            optionsBuilder.UseSqlServer(
                "Server=(localdb)\\mssqllocaldb;Database=Smairo.Template;Trusted_Connection=True;",
                x => x.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schema)
            );           
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Schema);
            modelBuilder.Entity<Entities.Sample>().ToTable("Sample");
        }

        public DbSet<Entities.Sample> Samples { get; set; }
    }
}