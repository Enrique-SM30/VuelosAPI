using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace VuelosAPI.Models
{
    public partial class sistem21_salidasvuelosContext : DbContext
    {
        public sistem21_salidasvuelosContext()
        {
        }

        public sistem21_salidasvuelosContext(DbContextOptions<sistem21_salidasvuelosContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Vuelo> Vuelo { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8_general_ci")
                .HasCharSet("utf8");

            modelBuilder.Entity<Vuelo>(entity =>
            {
                entity.HasKey(e => e.Codigo)
                    .HasName("PRIMARY");

                entity.ToTable("vuelo");

                entity.Property(e => e.Codigo).HasMaxLength(6);

                entity.Property(e => e.Destino).HasMaxLength(45);

                entity.Property(e => e.Estado).HasMaxLength(45);

                entity.Property(e => e.Hora).HasMaxLength(5);

                entity.Property(e => e.Puerta).HasMaxLength(3);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
