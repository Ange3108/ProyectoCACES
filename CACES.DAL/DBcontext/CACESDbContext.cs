using CACES.DAL.Entidades;
using CACES.DAL.Entidades.Roles;
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
        public DbSet<Medico> Medicos { get; set; }
        public DbSet<ApplicationUser> AspNetUsers { get; set; }
        public DbSet<AspNetUserRole> AspNetUserRoles { get; set; }
        public DbSet<AspNetRole> AspNetRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de la entidad Usuario
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.IdUsuario);
                entity.Property(e => e.IdUsuario).HasColumnName("Id_Usuario");
                entity.Property(e => e.Nombres).HasColumnName("Nombres").HasMaxLength(100).IsRequired();
                entity.Property(e => e.PrimerApellido).HasColumnName("PrimerApellido").HasMaxLength(100).IsRequired();
                entity.Property(e => e.SegundoApellido).HasColumnName("SegundoApellido").HasMaxLength(100).IsRequired();
                entity.Property(e => e.CorreoElectronico).HasColumnName("CorreoElectronico").HasMaxLength(200).IsRequired();
                entity.HasIndex(e => e.CorreoElectronico).IsUnique();
                entity.Property(e => e.DUI).HasColumnName("DUI").HasMaxLength(10).IsRequired();
                entity.HasIndex(e => e.DUI).IsUnique().HasDatabaseName("UQ_Usuarios_DUI");

                entity.Property(e => e.Telefono).HasColumnName("Telefono").HasMaxLength(30).IsRequired();
                entity.Property(e => e.Direccion).HasColumnName("Direccion").HasMaxLength(200).IsRequired();
                entity.Property(e => e.Nacimiento).HasColumnName("Nacimiento").IsRequired();
                entity.Property(e => e.FechaDeRegistro).HasColumnName("FechaDeRegistro").IsRequired();
                entity.Property(e => e.FechaDeModificacion).HasColumnName("FechaDeModificacion");
                entity.Property(e => e.Estado).HasColumnName("Estado").IsRequired().HasDefaultValue(true);
                entity.Property(e => e.PasswordHash).HasColumnName("PasswordHash").IsRequired();
                entity.Property(e => e.SecurityStamp).HasColumnName("SecurityStamp").IsRequired();
                entity.Property(e => e.twoFactorEnabled).HasColumnName("TwoFactorEnabled").HasDefaultValue(false).IsRequired();
                entity.Property(e => e.lockoutEnd).HasColumnName("LockoutEndDateUtc");
                entity.Property(e => e.LockoutEnabled).HasColumnName("LockoutEnabled").IsRequired();
                entity.Property(e => e.accessFailedCount).HasColumnName("AccessFailedCount").IsRequired();
                entity.Property(e => e.emailConfirmed).HasColumnName("EmailConfirmed").HasDefaultValue(false).IsRequired();

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

            // Configuración de la entidad Medico
            modelBuilder.Entity<Medico>(entity =>
            {
                entity.HasKey(e => e.IdMedico);


                entity.Property(e => e.IdMedico)
                    .HasColumnName("Id_Medico");

                entity.Property(e => e.IdEspecialidad)
                    .HasColumnName("Id_Especialidad");

                entity.Property(e => e.IdUsuario)
                    .HasColumnName("Id_Usuario");

                entity.Property(e => e.Experiencia)
                    .HasColumnName("Experiencia")
                    .IsRequired();

                entity.Property(e => e.Telefono)
                    .HasColumnName("Telefono")
                    .HasMaxLength(20)
                    .IsRequired();

                entity.Property(e => e.Certificaciones)
                    .HasColumnName("Certificaciones")
                    .HasMaxLength(500);

                entity.Property(e => e.FechaDeRegistro)
                    .HasColumnName("FechaDeRegistro");
                entity.Property(e => e.Foto).HasColumnName("Foto").HasMaxLength(200);

                entity.HasOne(e => e.Usuario)
                    .WithMany()
                    .HasForeignKey(e => e.IdUsuario);


                

            });



            modelBuilder.Entity<UsuarioRoles>()

          .HasKey(ur => new { ur.IdUsuario, ur.RoleId });

            modelBuilder.Entity<UsuarioRoles>()
                .HasOne(ur => ur.Usuario)
                .WithMany(u => u.UsuarioRoles)
                .HasForeignKey(ur => ur.IdUsuario)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UsuarioRoles>()
                .HasOne(ur => ur.Rol)
                .WithMany(r => r.UsuarioRoles)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable("AspNetUsers");
            });

            modelBuilder.Entity<AspNetRole>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable("AspNetRoles");
            });

            modelBuilder.Entity<AspNetUserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });
                entity.ToTable("AspNetUserRoles");
            });
        }
    }
}
