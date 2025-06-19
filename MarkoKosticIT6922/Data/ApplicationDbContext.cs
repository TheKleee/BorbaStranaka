using MarkoKosticIT6922.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MarkoKosticIT6922.Data
{
    public class ApplicationDbContext : IdentityDbContext<Glasac>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Stranka> Stranke { get; set; }
        public DbSet<Argument> Argumenti { get; set; }
        public DbSet<Borba> Borbe { get; set; }
        public DbSet<Glasanje> Glasanja { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Glasanje>()
                .HasOne(g => g.Glasac)
                .WithMany(g => g.Glasanja)
                .HasForeignKey(g => g.GlasacId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Glasanje>()
                .HasOne(g => g.Borba)
                .WithMany(b => b.Glasanja)
                .HasForeignKey(g => g.BorbaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Borba>()
                .HasOne(b => b.Stranka1)
                .WithMany()
                .HasForeignKey(b => b.Stranka1Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Borba>()
                .HasOne(b => b.Stranka2)
                .WithMany()
                .HasForeignKey(b => b.Stranka2Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Borba>()
                .HasOne(b => b.Pobednik)
                .WithMany()
                .HasForeignKey(b => b.PobednikId)
                .OnDelete(DeleteBehavior.SetNull);
        }

    }
}
