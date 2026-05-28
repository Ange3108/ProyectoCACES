using CACES.DAL.Entidades;
using Microsoft.EntityFrameworkCore;

namespace CACES.DAL.DBContext
{
    public class CACESDbContext : DbContext
    {
        public CACESDbContext(DbContextOptions<CACESDbContext> options) : base(options)
        {
        }

        // DbSets para las entidades
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<HistorialMedico> HistorialesMedicos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de la entidad Usuario
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.IdUsuario);
                entity.Property(e => e.IdUsuario).HasColumnName("Id_Usuario");
                entity.Property(e => e.Nombres).HasMaxLength(100).IsRequired();
                entity.Property(e => e.PrimerApellido).HasMaxLength(100).IsRequired();
                entity.Property(e => e.SegundoApellido).HasMaxLength(100).IsRequired();
                entity.Property(e => e.CorreoElectronico).HasMaxLength(200).IsRequired();
                entity.HasIndex(e => e.CorreoElectronico).IsUnique();
                entity.Property(e => e.DUI).HasMaxLength(10).IsRequired();
                entity.HasIndex(e => e.DUI).IsUnique().HasDatabaseName("UQ_Usuarios_DUI");
                entity.Property(e => e.Foto).HasMaxLength(200).IsRequired();
                entity.Property(e => e.Telefono).HasMaxLength(30).IsRequired();
                entity.Property(e => e.FechaDeRegistro).IsRequired();
                entity.Property(e => e.FechaDeModificacion);
                entity.Property(e => e.Estado).IsRequired().HasDefaultValue(true);
            });

       
            // Configuración de la entidad HistorialMedico
            modelBuilder.Entity<HistorialMedico>(entity =>
            {
                entity.HasKey(e => e.IdHistorial);
                entity.Property(e => e.IdHistorial).HasColumnName("Id_Historial");
                entity.Property(e => e.Alergias).HasMaxLength(200).IsRequired();
                entity.Property(e => e.EnfermedadesCronicas).HasColumnName("Enfermedades_Crónicas").HasMaxLength(200).IsRequired();
                entity.Property(e => e.Detalles).HasMaxLength(100).IsRequired();
                entity.Property(e => e.TipoSangre).HasColumnName("Tipo_Sangre").HasMaxLength(10).IsRequired();
                entity.Property(e => e.Antecedentes).HasColumnName("Anteriores").HasMaxLength(50).IsRequired();
                entity.Property(e => e.FechaDeCreacion).IsRequired();
                entity.Property(e => e.FechaDeModificacion);
            });

            // Configuración de la entidad Paciente
            modelBuilder.Entity<Paciente>(entity =>
            {
                entity.HasKey(e => e.IdPaciente);
                entity.Property(e => e.IdPaciente).HasColumnName("Id_Paciente");
                entity.Property(e => e.IdUsuario).HasColumnName("Id_Usuario").IsRequired();
                entity.Property(e => e.IdHistorial).HasColumnName("Id_Historial").IsRequired();

                entity.HasOne(p => p.Usuario)
                    .WithMany()
                    .HasForeignKey(p => p.IdUsuario)
                    .HasConstraintName("FK_Paciente_Usuario");

                entity.HasOne(p => p.HistorialMedico)
                    .WithMany()
                    .HasForeignKey(p => p.IdHistorial)
                    .HasConstraintName("FK_Pacientes_Historial");
            });
        }
    }
}

