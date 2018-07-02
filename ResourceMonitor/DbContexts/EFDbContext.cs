using ResourceMonitor.Entities;

namespace ResourceMonitor.DbContexts
{
    using System.Data.Entity;

    /// <summary> Контекст подключения к БД </summary>
    public class EFDbContext : DbContext
    {
        public DbSet<Resource> Resources { get; set; }
    }
}