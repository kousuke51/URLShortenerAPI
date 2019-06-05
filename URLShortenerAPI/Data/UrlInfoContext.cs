using Microsoft.EntityFrameworkCore;
using URLShortenerAPI.Models;

namespace URLShortenerAPI.Data
{
    public class UrlInfoContext : DbContext
    {

        public UrlInfoContext(DbContextOptions<UrlInfoContext> options) : base(options)
        {
        }

        public DbSet<UrlInfo> UrlInfo { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //To avoid a table called "UrlInfos" from being created
            modelBuilder.Entity<UrlInfo>().ToTable("UrlInfo");
        }
    }
}
