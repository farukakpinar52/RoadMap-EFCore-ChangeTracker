using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;

namespace EntityStates.DAL.Entities;

public partial class ChangeTrackerDbContext : DbContext
{
    public ChangeTrackerDbContext()
    {
    }

    public ChangeTrackerDbContext(DbContextOptions<ChangeTrackerDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Parca> Parcas { get; set; }

    public virtual DbSet<Urun> Uruns { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Data Source=DESKTOP-P7KA77K\\SQLEXPRESS; Database = QueriesDB; User ID=sa;Password=1234; TrustServerCertificate=true");
        
    }
   

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Parca>(entity =>
        {
            entity.HasIndex(e => e.UrunId, "IX_Parcas_UrunId");

            entity.HasOne(d => d.Urun).WithMany(p => p.Parcas).HasForeignKey(d => d.UrunId);
        });

        modelBuilder.Entity<Urun>(entity =>
        {
            entity.HasMany(d => d.ParcasNavigation).WithMany(p => p.Uruns)
                .UsingEntity<Dictionary<string, object>>(
                    "UrunParca",
                    r => r.HasOne<Parca>().WithMany().HasForeignKey("ParcaId"),
                    l => l.HasOne<Urun>().WithMany().HasForeignKey("UrunId"),
                    j =>
                    {
                        j.HasKey("UrunId", "ParcaId");
                        j.ToTable("UrunParcas");
                        j.HasIndex(new[] { "ParcaId" }, "IX_UrunParcas_ParcaId");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
