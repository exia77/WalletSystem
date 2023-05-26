using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer.DatabaseContext
{
    public class DbLayerContext : DbContext
    {
        public DbLayerContext(DbContextOptions<DbLayerContext> options) : base(options)
        { 
        }

        public DbSet<UsersModelObject>? Users { get; set; }
        public DbSet<AccountsModelObject>? Accounts { get; set; }

        public DbSet<TransactionHistoryModelObjects>? TransactionHistory { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UsersModelObject>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(x=> x.Id);
                entity.Property(x => x.Username).IsRequired();
                entity.Property(x => x.Password).IsRequired();
                entity.Property(x => x.RegisterDate).IsRequired();
            });

            modelBuilder.Entity<AccountsModelObject>(entity =>
            {
                entity.ToTable("Accounts");
                entity.HasKey(x => x.Id);
                entity.HasOne(x => x.Users)
                .WithOne(x => x.Accounts)
                .HasForeignKey<AccountsModelObject>(x => x.UserId);
                entity.Property(x => x.AccountNumber)
                .IsRequired()
                .HasMaxLength(12);
                entity.Property(x => x.Balance)
                .IsRequired();
                entity.Property(x=>x.UpdatedDate)
                .IsRequired();
               
            });

            modelBuilder.Entity<TransactionHistoryModelObjects>().HasNoKey();
        }
    }
}
