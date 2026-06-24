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
        public DbSet<AspNetRole> AspNetRoles { get; set; }
        public DbSet<AspNetUserRole> AspNetUserRoles { get; set; }
        public DbSet<Cita> Citas { get; set; }
        public DbSet<Receta> Recetas { get; set; }
        public DbSet<Especialidad> Especialidades { get; set; }
        public DbSet<Paquete> Paquetes { get; set; }
        public DbSet<Precios> Precios { get; set; }
        public DbSet<HorariosDisponibles> HorariosDisponibles { get; set; }
        public DbSet<Procedimiento> Procedimientos { get; set; }
        public DbSet<Cirugias> Cirugias { get; set; }
        public DbSet<ArchivoHistorial> ArchivosHistorial { get; set; }
        public DbSet<UsuarioRoles> UsuarioRoles { get; set; }

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
                entity.Property(e => e.Telefono).HasColumnName("Telefono").HasMaxLength(20).IsRequired();
                entity.Property(e => e.Direccion).HasColumnName("Direccion").HasMaxLength(250).IsRequired();
                entity.Property(e => e.Nacimiento).HasColumnName("Nacimiento").IsRequired();
                entity.Property(e => e.FechaDeRegistro).HasColumnName("FechaDeRegistro").IsRequired();
                entity.Property(e => e.FechaDeModificacion).HasColumnName("FechaDeModificacion");
                entity.Property(e => e.Estado).HasColumnName("Estado").IsRequired();
                entity.Property(e => e.PasswordHash).HasColumnName("PasswordHash").IsRequired();
                entity.Property(e => e.SecurityStamp).HasColumnName("SecurityStamp").IsRequired();
                entity.Property(e => e.Foto).HasColumnName("Foto").HasMaxLength(200);
                entity.Property(e => e.TwoFactorEnabled).HasColumnName("TwoFactorEnabled").HasDefaultValue(false).IsRequired();
                entity.Property(e => e.LockoutEnd).HasColumnName("LockoutEndDateUtc");
                entity.Property(e => e.AccessFailedCount).HasColumnName("AccessFailedCount").IsRequired();
                entity.Property(e => e.EmailConfirmed).HasColumnName("EmailConfirmed").HasDefaultValue(false).IsRequired();
                entity.Property(e => e.Edad).HasColumnName("Edad").IsRequired();
                entity.HasOne(p => p.Paciente)
                .WithMany()
                .HasForeignKey(p => p.IdUsuario)// Apunta a la propiedad IdUsuario
                .IsRequired(false);
            });

            modelBuilder.Entity<Cita>(entity =>
            {
                entity.HasKey(e => e.IdCita);
                entity.Property(e => e.IdCita).HasColumnName("Id_Cita");
                entity.Property(e => e.IdPaciente).HasColumnName("Id_Paciente");
                entity.Property(e => e.IdMedico).HasColumnName("Id_Medico");
                entity.Property(e => e.IdEspecialidad).HasColumnName("Id_Especialidad");
                entity.Property(e => e.FechaCita).HasColumnName("Fecha");
                entity.Property(e => e.Hora).HasColumnName("Hora");
                entity.Property(e => e.Motivo).HasColumnName("Motivo").HasMaxLength(100);
                entity.Property(e => e.FechaCita).HasColumnName("FechaCita");
                entity.Property(e => e.FechaDeRegistro).HasColumnName("FechaDeRegistro");
                entity.Property(e => e.FechaDeModificacion).HasColumnName("FechaDeModificacion");
                entity.Property(e => e.Estado).HasColumnName("Estado");
                entity.HasOne(c => c.Receta)
                .WithMany()
                .HasForeignKey(c => c.IdCita);
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
                entity.Property(e => e.Medicamentos).HasColumnName("Medicamentos").IsRequired();
                entity.Property(e => e.Antecedentes).HasColumnName("Antecedentes");
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

                entity.HasOne(p => p.Cita)
                .WithMany()
                .HasForeignKey(p => p.IdPaciente);
            });

            // Configuración de la entidad Medico
            modelBuilder.Entity<Medico>(entity =>
            {
                entity.HasKey(e => e.IdMedico);
                entity.Property(e => e.IdMedico).HasColumnName("Id_Medico");
                entity.Property(e => e.IdEspecialidad).HasColumnName("Id_Especialidad").IsRequired();
                entity.Property(e => e.IdUsuario).HasColumnName("Id_Usuario");
                entity.Property(e => e.FechaDeRegistro).HasColumnName("FechaDeRegistro").IsRequired();
                entity.Property(e => e.Experiencia).HasColumnName("Experiencia").IsRequired();
                entity.Property(e => e.Certificaciones).HasColumnName("Certificaciones").HasMaxLength(500);
                entity.HasOne(e => e.Usuario).WithMany().HasForeignKey(e => e.IdUsuario);
                entity.HasOne(e => e.Especialidad).WithMany(es => es.Medicos).HasForeignKey(e => e.IdEspecialidad);
            });

            modelBuilder.Entity<Receta>(entity =>
            {
                entity.HasKey(e => e.IdReceta);
                entity.Property(e => e.IdReceta).HasColumnName("Id_Receta");
                entity.Property(e => e.IdCita).HasColumnName("Id_Cita");
                entity.Property(e => e.Medicamentos).HasColumnName("Medicamentos").IsRequired();
                entity.Property(e => e.Instrucciones).HasColumnName("Instrucciones");
                entity.Property(e => e.FechaDeRegistro).HasColumnName("FechaDeRegistro");
                entity.Property(e => e.FechaDeVencimiento).HasColumnName("FechaDeVencimiento");

                // Relación simple de uno a uno/muchos sin colecciones inversas
                entity.HasOne(r => r.Cita)
                      .WithMany()
                      .HasForeignKey(r => r.IdCita);
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


            // Configuración de ApplicationUser
            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable("AspNetUsers");
            });

            // Configuración de AspNetRole
            modelBuilder.Entity<AspNetRole>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable("AspNetRoles");
            });

            // Configuración de AspNetUserRole
            modelBuilder.Entity<AspNetUserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });
                entity.ToTable("AspNetUserRoles");
            });

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


            //configuración de la entidad Especialidad
            modelBuilder.Entity<Especialidad>(entity =>
            {
                entity.HasKey(e => e.IdEspecialidad);
                entity.Property(e => e.IdEspecialidad).HasColumnName("Id_Especialidad");
                entity.Property(e => e.Nombre).HasColumnName("Nombre").IsRequired().HasMaxLength(100);
                entity.HasIndex(e => e.Nombre).IsUnique();
                entity.Property(e => e.Descripcion).HasColumnName("Descripcion").IsRequired().HasMaxLength(200);
                entity.Property(e => e.Icono).HasColumnName("Icono").IsRequired().HasMaxLength(200);
                entity.Property(e => e.FechaDeRegistro).HasColumnName("FechaDeRegistro").IsRequired();
                entity.Property(e => e.Estado).HasColumnName("Estado").IsRequired();
            });

            //configuración de la entidad Paquete

            modelBuilder.Entity<Paquete>(entity =>
            {
                entity.HasKey(e => e.IdPaquete);
                entity.Property(e => e.IdPaquete).HasColumnName("Id_Paquete");
                entity.Property(e => e.Nombre).HasColumnName("Nombre").IsRequired().HasMaxLength(50);
                entity.Property(e => e.Descripcion).HasColumnName("Descripcion").IsRequired().HasMaxLength(200);
                entity.Property(e => e.Duracion).HasColumnName("Duracion").IsRequired().HasMaxLength(50);
                entity.Property(e => e.Precio).HasColumnName("Precio").IsRequired();
                entity.Property(e => e.FechaDeRegistro).HasColumnName("FechaDeRegistro").IsRequired();
                entity.Property(e => e.Estado).HasColumnName("Estado").IsRequired().HasDefaultValue(true);

            });
            //configuración de la entidad Procedimiento
            modelBuilder.Entity<Procedimiento>(entity =>
            {
                entity.HasKey(e => e.Id_Procedimiento);
                entity.Property(e => e.Id_Procedimiento).HasColumnName("Id_Procedimiento");
                entity.Property(e => e.Id_Especialidad).HasColumnName("Id_Especialidad").IsRequired();
                entity.Property(e => e.Nombre).HasColumnName("Nombre").IsRequired().HasMaxLength(100);
                entity.Property(e => e.Descripcion).HasColumnName("Descripcion").HasMaxLength(200);
                entity.Property(e => e.PrecioBase).HasColumnName("PrecioBase");
                entity.Property(e => e.Estado).HasColumnName("Estado").IsRequired().HasDefaultValue(true);
                entity.HasOne(d => d.Especialidad)
                  .WithMany(p => p.Procedimientos)
                  .HasForeignKey(d => d.Id_Especialidad)
                  .OnDelete(DeleteBehavior.ClientSetNull);
            });

            //configuración de la entidad Cirugias
            modelBuilder.Entity<Cirugias>(entity =>
            {
                entity.HasKey(e => e.Id_Cirugia);
                entity.Property(e => e.Id_Cirugia).HasColumnName("Id_Cirugia");
                entity.Property(e => e.Id_Paciente).HasColumnName("Id_Paciente").IsRequired();
                entity.Property(e => e.Id_Medico).HasColumnName("Id_Medico").IsRequired();
                entity.Property(e => e.Id_Procedimiento).HasColumnName("Id_Procedimiento").IsRequired();
                entity.Property(e => e.Id_Horario).HasColumnName("Id_Horario").IsRequired();
                entity.Property(e => e.Estado).HasColumnName("Estado").IsRequired().HasDefaultValue(true);
                entity.HasOne(d => d.Paciente)
                      .WithMany()
                      .HasForeignKey(d => d.Id_Paciente)
                      .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Medico)
                      .WithMany()
                      .HasForeignKey(d => d.Id_Medico)
                      .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Procedimiento)
                      .WithMany(p => p.Cirugias)
                      .HasForeignKey(d => d.Id_Procedimiento)
                      .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Horario)
                      .WithMany(h => h.Cirugias)
                      .HasForeignKey(d => d.Id_Horario)
                      .OnDelete(DeleteBehavior.ClientSetNull);

            });

            //configuración de la entidad Precios

            modelBuilder.Entity<Precios>(entity =>
            {
                entity.HasKey(e => e.Id_Precio);
                entity.Property(e => e.Id_Precio).HasColumnName("Id_Precio");
                entity.Property(e => e.Id_Medico).HasColumnName("Id_Medico").IsRequired();
                entity.Property(e => e.Id_Procedimiento).HasColumnName("Id_Procedimiento").IsRequired();
                entity.Property(e => e.Costo).HasColumnName("Costo").IsRequired();
                entity.Property(e => e.Detalles).HasColumnName("Detalles").IsRequired().HasMaxLength(100);
                entity.HasOne(d => d.Procedimiento)
                  .WithMany(p => p.Precios)
                  .HasForeignKey(d => d.Id_Procedimiento)
                  .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Medico)
                      .WithMany()
                      .HasForeignKey(d => d.Id_Medico)
                      .OnDelete(DeleteBehavior.ClientSetNull);
            });

            //configuración de la entidad HorariosDisponibles

            modelBuilder.Entity<HorariosDisponibles>(entity =>
            {
                entity.HasKey(e => e.Id_Horario);
                entity.Property(e => e.Id_Horario).HasColumnName("Id_Horario");
                entity.Property(e => e.Id_Medico).HasColumnName("Id_Medico").IsRequired();
                entity.Property(e => e.DiaSemana).HasColumnName("DiaSemana").IsRequired();
                entity.Property(e => e.HoraInicio).HasColumnName("HoraInicio").IsRequired();
                entity.Property(e => e.HoraFin).HasColumnName("HoraFin").IsRequired();
                entity.Property(e => e.Activo).HasColumnName("Activo").IsRequired().HasDefaultValue(true);
                entity.HasOne(d => d.Medico)
                  .WithMany()
                  .HasForeignKey(d => d.Id_Medico)
                  .OnDelete(DeleteBehavior.ClientSetNull);
            });

            // Configuración de la entidad ArchivoHistorial
            modelBuilder.Entity<ArchivoHistorial>(entity =>
            {
                entity.HasKey(e => e.IdArchivo);

                entity.Property(e => e.IdArchivo)
                    .HasColumnName("Id_Archivo");

                entity.Property(e => e.IdHistorial)
                    .HasColumnName("Id_Historial")
                    .IsRequired();

                entity.Property(e => e.NombreArchivo)
                    .HasColumnName("NombreArchivo")
                    .HasMaxLength(200)
                    .IsRequired();

                entity.Property(e => e.RutaArchivo)
                    .HasColumnName("RutaArchivo")
                    .HasMaxLength(500)
                    .IsRequired();

                entity.Property(e => e.TipoArchivo)
                    .HasColumnName("TipoArchivo")
                    .HasMaxLength(50);

                entity.Property(e => e.FechaDeSubida)
                    .HasColumnName("FechaDeSubida");

                entity.HasOne(e => e.HistorialMedico)
                    .WithMany()
                    .HasForeignKey(e => e.IdHistorial)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
