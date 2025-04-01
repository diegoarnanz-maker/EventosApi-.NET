using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventosApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EventosApi.Configurations
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Tipo> Tipos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Evento> Eventos { get; set; }
        public DbSet<Reserva> Reservas { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Conversión de enums de Evento
            modelBuilder.Entity<Evento>()
                .Property(e => e.Estado)
                .HasConversion<string>();

            modelBuilder.Entity<Evento>()
                .Property(e => e.Destacado)
                .HasConversion<string>();

            // Relaciones explícitas de RESERVAS
            modelBuilder.Entity<Reserva>()
                .HasOne(r => r.Usuario)
                .WithMany(u => u.Reservas)
                .HasForeignKey(r => r.Username);

            modelBuilder.Entity<Reserva>()
                .HasOne(r => r.Evento)
                .WithMany(e => e.Reservas)
                .HasForeignKey(r => r.IdEvento);

            // Restricción UNIQUE compuesta
            modelBuilder.Entity<Reserva>()
                .HasIndex(r => new { r.IdEvento, r.Username })
                .IsUnique();

        }

    }
}