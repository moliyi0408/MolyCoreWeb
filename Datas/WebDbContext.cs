﻿using Microsoft.EntityFrameworkCore;
using MolyCoreWeb.Models.DBEntitiy;

namespace MolyCoreWeb.Datas
{
    public class WebDbContext : DbContext
    {
        // DbSet 是 Entity Framework Core 中表示資料表的類別，可以對資料庫中特定資料表操作的集合

        public DbSet<User> User { get; set; }
        public DbSet<UserProfile> UserProfile { get; set; }
        public DbSet<StockRow> Stocks { get; set; }
        public DbSet<StockInfo> StockInfo { get; set; }
        public DbSet<BusinessIndicator> BusinessIndicators { get; set; }


        // 資料庫上下文的建構函式，包括資料庫連接字串、資料庫提供者
        public WebDbContext(DbContextOptions<WebDbContext> options) : base(options)
        {
            // 用於確保資料庫已經被創建
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
            .ToTable("User")
            .HasMany(u => u.UserProfiles) //配置一對多關係(User 對 UserProfile)
            .WithOne(up => up.User)
            .HasForeignKey(up => up.UserId);

            modelBuilder.Entity<UserProfile>().HasKey(u => u.ProfileId);

            modelBuilder.Entity<StockRow>().ToTable("Stocks");

            modelBuilder.Entity<StockInfo>().ToTable("StockInfo");

            modelBuilder.Entity<BusinessIndicator>().ToTable("BusinessIndicators");

        }

    }
}
