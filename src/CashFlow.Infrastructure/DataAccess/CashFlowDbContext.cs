using CashFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CashFlow.Infrastructure.DataAccess;

internal class CashFlowDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Expense> Expenses { get; set; }
    public DbSet<User> Users { get; set; }
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Expense>(model =>
        {
            model.ToTable("Expenses");
            model.HasKey(x => x.Id);
            model.Property(x => x.Id).HasColumnType("BIGINT").IsRequired();
            model.Property(x => x.UserId).HasColumnType("BIGINT").IsRequired();
            model.Property(x => x.Title).HasColumnType("VARCHAR(64)").IsRequired();
            model.Property(x => x.Description).HasColumnType("VARCHAR(256)");
            model.Property(x => x.Amount).HasColumnType("DECIMAL(18,2)").IsRequired();
            model.Property(x => x.PaymentType).HasColumnType("INT").IsRequired();
            model.Property(x => x.Date).HasColumnType("DATETIME").IsRequired();
        });
        
        modelBuilder.Entity<User>(model =>
        {
            model.ToTable("Users");
            model.HasKey(x => x.Id);
            model.Property(x => x.Id).HasColumnType("BIGINT").IsRequired();
            model.Property(x => x.Name).HasColumnType("VARCHAR(64)").IsRequired();
            model.Property(x => x.Email).HasColumnType("VARCHAR(256)").IsRequired();
            model.Property(x => x.Password).HasColumnType("VARCHAR(256)").IsRequired();
            model.Property(x => x.UserIdentifier).HasColumnType("VARCHAR(128)").IsRequired();
            model.Property(x => x.Role).HasColumnType("VARCHAR(32)").IsRequired();
        });
    }
}
