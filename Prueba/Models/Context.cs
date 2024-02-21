using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Prueba.Models
{
    /// <summary>
    /// Clase que representa el contexto de la base de datos para la aplicación.
    /// </summary>
    public partial class Context : DbContext
    {
        public Context()
        {
        }

        /// <summary>
        /// Constructor que recibe opciones de configuración del contexto.
        /// </summary>
        /// <param name="options">Opciones de configuración del contexto.</param>
        public Context(DbContextOptions<Context> options)
            : base(options)
        {
        }

        /// <summary>
        /// Conjunto de entidades para la tabla de clientes en la base de datos.
        /// </summary>
        public virtual DbSet<Cliente> Clientes { get; set; }

        /// <summary>
        /// Conjunto de entidades para la tabla de cuentas de inversión en la base de datos.
        /// </summary>
        public virtual DbSet<CuentaInversion> CuentaInversions { get; set; }

        /// <summary>
        /// Configuración del contexto cuando se está configurando con opciones.
        /// </summary>
        /// <param name="optionsBuilder">Constructor de opciones del contexto.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer("Server=DESKTOP-F0OIA87\\SQLEXPRESS;Database=Prueba;User Id=prueba;Password=prueba12;TrustServerCertificate=True;");

        /// <summary>
        /// Configuración del modelo de la base de datos.
        /// </summary>
        /// <param name="modelBuilder">Constructor de modelos de la base de datos.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Cliente__3214EC07522E0F07");

                entity.ToTable("Cliente");

                entity.Property(e => e.Id)
                    .HasColumnName("Id")
                    .ValueGeneratedOnAdd()
                    .IsRequired();

                entity.Property(e => e.FechaNacimiento).HasColumnName("Fecha_Nacimiento");
                entity.Property(e => e.Nombre).HasMaxLength(100);
                entity.Property(e => e.Rut).HasMaxLength(20);
            });


            modelBuilder.Entity<CuentaInversion>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__CuentaIn__3214EC076FDCC851");

                entity.ToTable("CuentaInversion");

                entity.Property(e => e.Id)
                    .HasColumnName("Id")
                    .ValueGeneratedOnAdd()
                    .IsRequired();

                entity.Property(e => e.IdCliente).HasColumnName("Id_Cliente");
                entity.Property(e => e.NombreCuenta)
                    .HasMaxLength(100)
                    .HasColumnName("Nombre_Cuenta");

                entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.CuentaInversions)
                    .HasForeignKey(d => d.IdCliente)
                    .HasConstraintName("FK__CuentaInv__Id_Cl__52593CB8");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
