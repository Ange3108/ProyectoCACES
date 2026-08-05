using CACES.DAL.Entidades;
using CACES.DAL.Entidades.Configuración;
using CACES.DAL.Entidades.Roles;
using CACES.DAL.Entidades.SeguimientoPostOperatorio;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Runtime.ConstrainedExecution;

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
        public DbSet<Cita> Citas { get; set; }
        public DbSet<Receta> Recetas { get; set; }
        public DbSet<Especialidad> Especialidades { get; set; }
        public DbSet<Paquete> Paquetes { get; set; }
        public DbSet<Precios> Precios { get; set; }
        public DbSet<HorariosDisponibles> HorariosDisponibles { get; set; }
        public DbSet<Procedimiento> Procedimientos { get; set; }
        public DbSet<Cirugias> Cirugias { get; set; }
        public DbSet<UsuarioRoles> UsuarioRoles { get; set; }
        public DbSet<ArchivoHistorial> ArchivosHistorial { get; set; }
        public DbSet<Noticia> Noticias { get; set; }
        public DbSet<ConfiguracionQuirofano> ConfiguracionQuirofano { get; set; }
        public DbSet<Soporte> Soportes { get; set; }
        public DbSet<Cotizacion> Cotizaciones { get; set; }
        public DbSet<Icono> Iconos { get; set; }
        public DbSet<ConfiguracionCheckpoints> ConfiguracionCheckpoints { get; set; }
        public DbSet<SeguimientoPaciente> SeguimientoPacientes { get; set; }
        public DbSet<PreguntaSeguimiento> PreguntasSeguimiento { get; set; }
        public DbSet<AlertaStaff> AlertasStaff { get; set; } 

        public DbSet<ConfiguracionCotizacion> ConfiguracionesCotizacion { get; set; }

        public DbSet<RespuestaSeguimiento> RespuestasSeguimiento { get; set; }
        public DbSet<Convenios> Convenios { get; set; }
        public DbSet<Notificacion> Notificaciones { get; set; }
        public DbSet<Configuracion> Configuraciones { get; set; }
        public DbSet<SolicitudMedico> SolicitudesMedico { get; set; }
        public DbSet<NotificacionUsuario> NotificacionesUsuario { get; set; } 
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


            modelBuilder.Entity<ConfiguracionQuirofano>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("Id");
                entity.Property(e => e.CupoMaximoDiario).HasColumnName("CupoMaximoDiario").IsRequired();
            });

            modelBuilder.Entity<Cita>(entity =>
{
    entity.HasKey(e => e.IdCita);

    entity.ToTable("Citas");

    entity.Property(e => e.IdCita).HasColumnName("Id_Cita");
    entity.Property(e => e.IdPaciente).HasColumnName("Id_Paciente");
    entity.Property(e => e.IdMedico).HasColumnName("Id_Medico");
    entity.Property(e => e.IdEspecialidad).HasColumnName("Id_Especialidad");
    entity.Property(e => e.IdHorario).HasColumnName("Id_Horario");
    entity.Property(e => e.Fecha).HasColumnName("Fecha");
    entity.Property(e => e.Motivo)
          .HasColumnName("Motivo")
          .HasMaxLength(100)
          .IsRequired();

    entity.Property(e => e.FechaDeRegistro).HasColumnName("FechaDeRegistro");
    entity.Property(e => e.FechaDeModificacion).HasColumnName("FechaDeModificacion");
    entity.Property(e => e.Estado).HasColumnName("Estado");
    entity.Property(e => e.IdProcedimiento).HasColumnName("Id_Procedimiento");

    entity.HasOne(c => c.Paciente)
          .WithMany(p => p.Citas)
          .HasForeignKey(c => c.IdPaciente)
          .OnDelete(DeleteBehavior.Restrict)
          .HasConstraintName("FK_Citas_Pacientes");

    entity.HasOne(c => c.Medico)
          .WithMany(m => m.Citas)
          .HasForeignKey(c => c.IdMedico)
          .OnDelete(DeleteBehavior.Restrict)
          .HasConstraintName("FK_Citas_Medicos");

    entity.HasOne(c => c.Especialidad)
          .WithMany(e => e.Citas)
          .HasForeignKey(c => c.IdEspecialidad)
          .OnDelete(DeleteBehavior.Restrict)
          .HasConstraintName("FK_Citas_Especialidad");

    entity.HasOne(c => c.Horario)
          .WithMany(h => h.Citas)
          .HasForeignKey(c => c.IdHorario)
          .OnDelete(DeleteBehavior.Restrict)
          .HasConstraintName("FK_Citas_Horario");

    entity.HasOne(c => c.Procedimiento)
          .WithMany()
          .HasForeignKey(c => c.IdProcedimiento)
          .OnDelete(DeleteBehavior.Restrict)
          .HasConstraintName("FK_Citas_Procedimiento");
});
            //Configuracion Configuracion Cotizacion

            modelBuilder.Entity<ConfiguracionCotizacion>(entity =>
            {
                entity.ToTable("ConfiguracionCotizacion");

                entity.HasKey(e => e.IdConfiguracion);

                entity.Property(e => e.IdConfiguracion)
                    .HasColumnName("Id_Configuracion");

                entity.Property(e => e.PorcentajeEquipo)
                    .HasColumnType("decimal(5,2)")
                    .IsRequired();

                entity.Property(e => e.CostoEstadiaDiaria)
                    .HasColumnType("decimal(10,2)")
                    .IsRequired();

                entity.Property(e => e.PorcentajeImpuesto)
                    .HasColumnType("decimal(5,2)")
                    .IsRequired();

                entity.Property(e => e.Estado)
                    .IsRequired();

                entity.Property(e => e.FechaDeRegistro)
                    .IsRequired();

                entity.Property(e => e.FechaDeModificacion);
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

            //Configuracion de la entidad Cotizacion
            modelBuilder.Entity<Cotizacion>(entity =>
            {
                entity.HasKey(c => c.IdCotizacion);

                entity.Property(c => c.PrecioBase)
                    .HasColumnType("decimal(10,2)");

                entity.Property(c => c.Descuento)
                    .HasColumnType("decimal(10,2)");

                entity.Property(c => c.Impuesto)
                    .HasColumnType("decimal(10,2)");

                entity.Property(c => c.Total)
                    .HasColumnType("decimal(10,2)");

                entity.HasOne(c => c.Paciente)
                    .WithMany()
                    .HasForeignKey(c => c.IdPaciente)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(c => c.Medico)
                    .WithMany()
                    .HasForeignKey(c => c.IdMedico)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(c => c.Procedimiento)
                    .WithMany()
                    .HasForeignKey(c => c.IdProcedimiento)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuración de la entidad Paciente
            modelBuilder.Entity<Paciente>(entity =>
            {
                entity.HasKey(e => e.IdPaciente);

                entity.Property(e => e.IdPaciente).HasColumnName("Id_Paciente");
                entity.Property(e => e.IdUsuario).HasColumnName("Id_Usuario").IsRequired();
                entity.Property(e => e.IdHistorial).HasColumnName("Id_Historial").IsRequired();

                entity.HasOne(p => p.Usuario)
                      .WithOne(u => u.Paciente)
                      .HasForeignKey<Paciente>(p => p.IdUsuario)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(p => p.HistorialMedico)
                      .WithMany()
                      .HasForeignKey(p => p.IdHistorial)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuración de la entidad Medico
            modelBuilder.Entity<Medico>(entity =>
            {
                entity.HasKey(e => e.IdMedico);

                entity.Property(e => e.IdMedico).HasColumnName("Id_Medico");
                entity.Property(e => e.IdEspecialidad).HasColumnName("Id_Especialidad");
                entity.Property(e => e.IdUsuario).HasColumnName("Id_Usuario");
                entity.Property(e => e.Experiencia).HasColumnName("Experiencia");
                entity.Property(e => e.Certificaciones).HasColumnName("Certificaciones");
                entity.Property(e => e.FechaDeRegistro).HasColumnName("FechaDeRegistro");

                entity.HasOne(m => m.Usuario)
                      .WithOne(u => u.Medico)
                      .HasForeignKey<Medico>(m => m.IdUsuario)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(m => m.Especialidad)
                      .WithMany(e => e.Medicos)
                      .HasForeignKey(m => m.IdEspecialidad)
                      .OnDelete(DeleteBehavior.Restrict);
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
                   .WithOne(c => c.Receta)
                   .HasForeignKey<Receta>(r => r.IdCita)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("FK_Recetas_Citas");
            });


            modelBuilder.Entity<UsuarioRoles>(entity =>
            {
                entity.HasKey(e => new { e.IdUsuario, e.RoleId });

                entity.ToTable("UsuarioRoles");

                entity.HasOne(e => e.Usuario)
                    .WithMany(u => u.UsuarioRoles)
                    .HasForeignKey(e => e.IdUsuario)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Rol)
                    .WithMany(r => r.UsuarioRoles)
                    .HasForeignKey(e => e.RoleId)
                    .OnDelete(DeleteBehavior.Cascade);
            });


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



            //configuración de la entidad Icono
            modelBuilder.Entity<Icono>(entity =>
            {
                entity.HasKey(e => e.IdIcono);
                entity.Property(e => e.IdIcono).HasColumnName("Id_Icono");
                entity.Property(e => e.Codigo).HasColumnName("Codigo").IsRequired().HasMaxLength(50);
                entity.Property(e => e.Nombre).HasColumnName("Nombre").IsRequired().HasMaxLength(100);
            });

            //configuración de la entidad Especialidad
            modelBuilder.Entity<Especialidad>(entity =>
            {
                entity.HasKey(e => e.IdEspecialidad);
                entity.Property(e => e.IdEspecialidad).HasColumnName("Id_Especialidad");
                entity.Property(e => e.Nombre).HasColumnName("Nombre").IsRequired().HasMaxLength(100);
                entity.HasIndex(e => e.Nombre).IsUnique();
                entity.Property(e => e.Descripcion).HasColumnName("Descripcion").IsRequired().HasMaxLength(200);
                entity.Property(e => e.IdIcono).HasColumnName("Id_Icono").IsRequired();
                entity.Property(e => e.FechaDeRegistro).HasColumnName("FechaDeRegistro").IsRequired();
                entity.Property(e => e.Estado).HasColumnName("Estado").IsRequired();

                entity.HasOne(d => d.Icono)
                      .WithMany(p => p.Especialidades)
                      .HasForeignKey(d => d.IdIcono)
                      .OnDelete(DeleteBehavior.ClientSetNull);
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
                entity.Property(e => e.Estado).HasColumnName("Estado").IsRequired().ValueGeneratedNever();

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
                entity.Property(e => e.Estado).HasColumnName("Estado").IsRequired();
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
                entity.Property(e => e.Id_Cita).HasColumnName("Id_Cita").IsRequired();
                entity.Property(e => e.Estado).HasColumnName("Estado").IsRequired();
                entity.HasOne(d => d.Paciente)
          .WithMany(p => p.Cirugias) // <-- Apuntamos explícitamente a la colección en Paciente
          .HasForeignKey(d => d.Id_Paciente)
          .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Medico)
          .WithMany(m => m.Cirugias) // <-- Apuntamos explícitamente a la colección en Medico
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
          
                entity.HasOne(d => d.Cita)
                      .WithOne(c => c.Cirugia)
                      .HasForeignKey<Cirugias>(d => d.Id_Cita)
                      .OnDelete(DeleteBehavior.ClientSetNull);

            });

            //configuración de la entidad Precios
            modelBuilder.Entity<Precios>(entity =>
            {
                entity.ToTable("Precios");

                entity.HasKey(e => e.Id_Precio);

                entity.Property(e => e.Id_Precio)
                    .HasColumnName("Id_Precio");

                entity.Property(e => e.Id_Medico)
                    .HasColumnName("Id_Medico")
                    .IsRequired();

                entity.Property(e => e.Id_Procedimiento)
                    .HasColumnName("Id_Procedimiento")
                    .IsRequired();

                entity.Property(e => e.Costo)
                    .HasColumnName("Costo")
                    .HasColumnType("decimal(10,2)")
                    .IsRequired();

                entity.Property(e => e.Detalles)
                    .HasColumnName("Detalles")
                    .HasMaxLength(100)
                    .IsRequired();

                entity.HasOne(e => e.Medico)
                    .WithMany(m => m.Precios)
                    .HasForeignKey(e => e.Id_Medico)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Precios_Medico");

                entity.HasOne(e => e.Procedimiento)
                    .WithMany(p => p.Precios)
                    .HasForeignKey(e => e.Id_Procedimiento)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Precios_Procedimiento");
            });

            //configuración de la entidad HorariosDisponibles

            modelBuilder.Entity<HorariosDisponibles>(entity =>
{
    entity.HasKey(e => e.Id_Horario);

    entity.Property(e => e.Id_Horario).HasColumnName("Id_Horario");
    entity.Property(e => e.Id_Medico).HasColumnName("Id_Medico");
    entity.Property(e => e.DiaSemana).HasColumnName("DiaSemana");
    entity.Property(e => e.HoraInicio).HasColumnName("HoraInicio");

    entity.Property(e => e.Estado).HasColumnName("Estado");

    entity.HasOne(h => h.Medico)
          .WithMany(m => m.HorariosDisponibles)
          .HasForeignKey(h => h.Id_Medico)
          .OnDelete(DeleteBehavior.Restrict);
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

            modelBuilder.Entity<ConfiguracionCheckpoints>(entity =>
            {
                entity.HasKey(e => e.IdCheckPoint);
                entity.Property(e => e.IdCheckPoint).HasColumnName("Id_CheckPoint");
                entity.Property(e => e.DiaCheckPoint).HasColumnName("DiaCheckpoint").IsRequired();
                entity.Property(e => e.Estado).HasColumnName("Estado").IsRequired();
            });

            modelBuilder.Entity<PreguntaSeguimiento>(entity =>
            {
                entity.HasKey(e => e.IdPregunta);
                entity.Property(e => e.IdPregunta).HasColumnName("Id_Pregunta");
                entity.Property(e => e.Texto).HasColumnName("Texto").IsRequired().HasMaxLength(200);
                entity.Property(e => e.ValorMinimo).HasColumnName("ValorMinimo").IsRequired();
                entity.Property(e => e.ValorMaximo).HasColumnName("ValorMaximo").IsRequired();
                entity.Property(e => e.UmbralAlerta).HasColumnName("UmbralAlerta").IsRequired();
                entity.Property(e => e.DireccionAlerta).HasColumnName("DireccionAlerta").IsRequired();
                entity.Property(e => e.Estado).HasColumnName("Estado").IsRequired();
            });

            modelBuilder.Entity<SeguimientoPaciente>(entity =>
            {
                entity.HasKey(e => e.Id_Seguimiento);
                entity.Property(e => e.Id_Seguimiento).HasColumnName("Id_Seguimiento");
                entity.Property(e => e.Id_Cirugia).HasColumnName("Id_Cirugia").IsRequired();
                entity.Property(e => e.DiaCheckpoint).HasColumnName("DiaCheckpoint").IsRequired();
                entity.Property(e => e.FechaProgramada).HasColumnName("FechaProgramada").IsRequired();
                entity.Property(e => e.Estado).HasColumnName("Estado").IsRequired();
                entity.Property(e => e.FechaRegistro).HasColumnName("FechaRegistro");
                entity.HasOne(s => s.Cirugia)
                    .WithMany(c => c.Seguimientos)
                    .HasForeignKey(s => s.Id_Cirugia)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<AlertaStaff>(entity =>
            {
                entity.HasKey(e => e.IdAlerta);
                entity.Property(e => e.IdAlerta).HasColumnName("Id_Alerta");
                entity.Property(e => e.IdSeguimiento).HasColumnName("Id_Seguimiento").IsRequired();
                entity.Property(e => e.FechaGenerada).HasColumnName("FechaGenerada").IsRequired();
                entity.Property(e => e.Estado).HasColumnName("Estado").IsRequired();
                entity.Property(e => e.IdUsuarioAtendio).HasColumnName("Id_Usuario_Atendio");
                entity.Property(e => e.Observaciones).HasColumnName("Observaciones").HasMaxLength(500);
                entity.Property(e => e.FechaAtencion).HasColumnName("FechaAtencion");
                entity.HasOne(a => a.SeguimientoPaciente)
                      .WithMany(s => s.AlertasStaff)
                      .HasForeignKey(a => a.IdSeguimiento)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(a => a.UsuarioAtendio)
                      .WithMany()
                      .HasForeignKey(a => a.IdUsuarioAtendio)
                      .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<RespuestaSeguimiento>(entity =>
            {
                entity.HasKey(e => e.IdRespuesta);
                entity.Property(e => e.IdRespuesta).HasColumnName("Id_Respuesta");
                entity.Property(e => e.IdSeguimiento).HasColumnName("Id_Seguimiento").IsRequired();
                entity.Property(e => e.IdPregunta).HasColumnName("Id_Pregunta").IsRequired();
                entity.Property(e => e.ValorRespuesta).HasColumnName("ValorRespuesta").IsRequired();
                entity.Property(e => e.GeneroAlerta).HasColumnName("GeneroAlerta").IsRequired();
                entity.HasOne(r => r.SeguimientoPaciente)
                      .WithMany(s => s.RespuestasSeguimiento)
                      .HasForeignKey(r => r.IdSeguimiento)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(r => r.PreguntaSeguimiento)
                      .WithMany()
                      .HasForeignKey(r => r.IdPregunta)
                      .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<Convenios>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("Id");
                entity.Property(e => e.Nombre).HasColumnName("Nombre").IsRequired().HasMaxLength(100);
                entity.Property(e => e.Descripcion).HasColumnName("Descripcion").HasMaxLength(200);
                entity.Property(e => e.DescuentoPorcentaje).HasColumnName("DescuentoPorcentaje").HasColumnType("decimal(18,2)");
                entity.Property(e => e.ContactoTelefono).HasColumnName("ContactoTelefono").HasMaxLength(20);
                entity.Property(e => e.ImagenUrl).HasColumnName("ImagenUrl").HasMaxLength(200);
                entity.Property(e => e.Estado).HasColumnName("Estado").IsRequired();
                entity.Property(e => e.FechaCreacion).HasColumnName("FechaCreacion").IsRequired();
            });

            modelBuilder.Entity<Notificacion>(entity =>
            {
                entity.HasKey(e => e.Id_Notificacion);
                entity.Property(e => e.Id_Notificacion).HasColumnName("Id_Notificacion");
                entity.Property(e => e.Evento).HasColumnName("Evento").IsRequired().HasMaxLength(100);
                entity.Property(e => e.CanalPlataforma).HasColumnName("CanalPlataforma").IsRequired();
                entity.Property(e => e.CanalEmail).HasColumnName("CanalEmail").IsRequired();
                entity.Property(e => e.Estado).HasColumnName("Estado").IsRequired();
            });

            modelBuilder.Entity<Configuracion>(entity =>
            {
                entity.HasKey(e => e.IdConfiguracion);
                entity.Property(e => e.IdConfiguracion).HasColumnName("Id_Configuracion");
                entity.Property(e => e.Clave).HasColumnName("Clave").IsRequired().HasMaxLength(100);
                entity.Property(e => e.Valor).HasColumnName("Valor").IsRequired().HasMaxLength(500);
                entity.Property(e => e.Tipo).HasColumnName("Tipo").HasMaxLength(200);
                entity.Property(e => e.Categoria).HasColumnName("Categoria").IsRequired();
                entity.Property(e => e.Descripcion).HasColumnName("Descripcion").HasMaxLength(500);

            });

            modelBuilder.Entity<NotificacionUsuario>(entity =>
            {
                entity.HasKey(e => e.IdNotificacionUsuario);
                entity.Property(e => e.IdNotificacionUsuario).HasColumnName("Id_NotificacionUsuario");
                entity.Property(e => e.IdUsuario).HasColumnName("IdUsuario").IsRequired();
                entity.Property(e => e.Evento).HasColumnName("Evento").IsRequired().HasMaxLength(100);
                entity.Property(e => e.Titulo).HasColumnName("Titulo").IsRequired().HasMaxLength(200);
                entity.Property(e => e.Mensaje).HasColumnName("Mensaje").IsRequired().HasMaxLength(500);
                entity.Property(e => e.Leido).HasColumnName("Leido").IsRequired();
                entity.Property(e => e.FechaCreacion).HasColumnName("FechaCreacion").IsRequired();
                entity.Property(e => e.FechaLectura).HasColumnName("FechaLectura");
            });

            //Solicitud medico
            modelBuilder.Entity<SolicitudMedico>(entity =>
            {
                entity.ToTable("SolicitudMedico");

                entity.HasKey(e => e.IdSolicitud);

                entity.Property(e => e.IdSolicitud)
                    .HasColumnName("Id_Solicitud");

                entity.Property(e => e.Nombres)
                    .HasMaxLength(80)
                    .IsRequired();

                entity.Property(e => e.PrimerApellido)
                    .HasMaxLength(60)
                    .IsRequired();

                entity.Property(e => e.SegundoApellido)
                    .HasMaxLength(60);

                entity.Property(e => e.CorreoElectronico)
                    .HasMaxLength(120)
                    .IsRequired();

                entity.Property(e => e.Telefono)
                    .HasMaxLength(25)
                    .IsRequired();

                entity.Property(e => e.IdEspecialidad)
                    .HasColumnName("Id_Especialidad")
                    .IsRequired();

                entity.Property(e => e.AniosExperiencia)
                    .IsRequired();

                entity.Property(e => e.Certificaciones)
                    .HasMaxLength(500);

                entity.Property(e => e.Motivo)
                    .HasMaxLength(500)
                    .IsRequired();

                entity.Property(e => e.Curriculum)
                    .HasMaxLength(250);

                entity.Property(e => e.Foto)
                    .HasMaxLength(250);

                entity.Property(e => e.Estado)
                    .IsRequired();

                entity.Property(e => e.ObservacionAdministrador)
                    .HasMaxLength(500);

                entity.Property(e => e.FechaSolicitud)
                    .IsRequired();

                entity.Property(e => e.FechaRespuesta);

                entity.HasOne(e => e.Especialidad)
                    .WithMany()
                    .HasForeignKey(e => e.IdEspecialidad)
                    .OnDelete(DeleteBehavior.Restrict);
            });

        }
    }
}
