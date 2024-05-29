using Microsoft.EntityFrameworkCore;
using MolyCoreWeb.Models.DBEntitiy;

namespace MolyCoreWeb.Datas
{
    public class WebDbContext : DbContext
    {
        // DbSet 是 Entity Framework Core 中表示資料表的類別，可以對資料庫中特定資料表操作的集合

        public DbSet<User> User { get; set; }
        public DbSet<UserProfile> UserProfile { get; set; }

        // 資料庫上下文的建構函式，包括資料庫連接字串、資料庫提供者
        public WebDbContext(DbContextOptions<WebDbContext> options) : base(options)
        {
            // 用於確保資料庫已經被創建
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");

            modelBuilder.Entity<UserProfile>().HasKey(u => u.ProfileId);
        }

    }
}
