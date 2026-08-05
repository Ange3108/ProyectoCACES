
using CACES.BLL.Servicios.AlertaStaff;
using CACES.BLL.Servicios.ArchivosHistorial;
using CACES.BLL.Servicios.Auth;
using CACES.BLL.Servicios.Cirugia;
using CACES.BLL.Servicios.Citas;
using CACES.BLL.Servicios.Configuracion;
using CACES.BLL.Servicios.ConfiguracionCheckPoints;
using CACES.DAL.Repositorios.SolicitudMedicos;
using CACES.BLL.Servicios.Convenios;
using CACES.BLL.Servicios.Cotizaciones;
using CACES.BLL.Servicios.Especialidad;
using CACES.BLL.Servicios.Especialidad.ProyectoCACES.CACES.BLL.Servicios;
using CACES.BLL.Servicios.HistorialMedicos;
using CACES.BLL.Servicios.Horario;
using CACES.BLL.Servicios.Icono;
using CACES.BLL.Servicios.Medicos;
using CACES.BLL.Servicios.Notificacion;
using CACES.BLL.Servicios.Paciente;
using CACES.BLL.Servicios.Paquete;
using CACES.BLL.Servicios.Perfil;
using CACES.BLL.Servicios.Precio;
using CACES.BLL.Servicios.PreguntasPOp;
using CACES.BLL.Servicios.Procedimientos;
using CACES.BLL.Servicios.Quirofano;
using CACES.BLL.Servicios.Recetas;
using CACES.BLL.Servicios.RespuestaSeguimiento;
using CACES.BLL.Servicios.Roles;
using CACES.BLL.Servicios.SeguimientoPaciente;
using CACES.BLL.Servicios.Soportes;
using CACES.BLL.Servicios.Usuario;
using CACES.DAL.DBContext;
using CACES.DAL.Repositorios.ArchivosHistorial;
using CACES.DAL.Repositorios.Base;
using CACES.DAL.Repositorios.Cirugia;
using CACES.DAL.Repositorios.Citas;
using CACES.DAL.Repositorios.Convenios;
using CACES.DAL.Repositorios.Cotizaciones;
using CACES.DAL.Repositorios.Especialidades;
using CACES.DAL.Repositorios.HistorialMedicos;
using CACES.DAL.Repositorios.Horarios;
using CACES.BLL.Servicios.Precio;
using CACES.DAL.Repositorios.Medicos;
using CACES.DAL.Repositorios.Pacientes;
using CACES.DAL.Repositorios.Paquetes;
using CACES.DAL.Repositorios.Precio;
using CACES.DAL.Repositorios.Procedimientos;
using CACES.DAL.Repositorios.Quirofano;
using CACES.DAL.Repositorios.Recetas;
using CACES.DAL.Repositorios.Roles;
using CACES.DAL.Repositorios.SeguimientoPaciente;
using CACES.DAL.Repositorios.Soportes;
using CACES.DAL.Repositorios.Usuario;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using CACES.DAL.Repositorios.Precio;
using CACES.BLL.Servicios.SolicitudMedico;


var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddRazorPages();

// Register EF Core DbContext (SQLServer
// ). Update the connection string in appsettings.json
builder.Services.AddDbContext<CACESDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Inyección de dependencias para repositorios, servicios, etc.

// Agregar esto:
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
builder.Services.AddScoped<IPacienteRepositorio, PacienteRepositorio>();
builder.Services.AddScoped<IMedicoRepositorio, MedicoRepositorio>();
builder.Services.AddScoped<IHistorialMedicoRepositorio, HistorialMedicoRepositorio>();
builder.Services.AddScoped<IRolRepositorio, RolRepositorio>();
builder.Services.AddScoped<ICitaRepositorio, CitaRepositorio>();
builder.Services.AddScoped<IEspecialidadRepositorio, EspecialidadRepositorio>();
builder.Services.AddScoped<IPaqueteRepositorio, PaqueteRepositorio>();
builder.Services.AddScoped<IProcedimientosRepositorio, ProcedimientosRepositorio>();
builder.Services.AddScoped<IArchivoHistorialRepositorio, ArchivoHistorialRepositorio>();
builder.Services.AddScoped<IQuirofanoRepositorio, QuirofanoRepositorio>();
builder.Services.AddScoped<IHorariosRepositorio, HorariosRepositorio>();
builder.Services.AddScoped<IPrecioServicio, PrecioServicio>();
builder.Services.AddScoped<IRecetaRepositorio, RecetaRepositorio>();
builder.Services.AddScoped<ICotizacionRepositorio, CotizacionRepositorio>();
builder.Services.AddScoped<IRolRepositorio, RolRepositorio>();
builder.Services.AddScoped<ISeguimientoPacienteRepositorio, SeguimientoPacienteRepositorio>();
builder.Services.AddScoped<INotificacionRepositorio, NotificacionRepositorio>();
builder.Services.AddScoped<IConfiguracionRepositorio, ConfiguracionRepositorio>();
builder.Services.AddScoped<IConvenioRepositorio, ConvenioRepositorio>();
builder.Services.AddScoped<INotificacionUsuarioRepositorio, NotificacionUsuarioRepositorio>();
builder.Services.AddScoped<ISolicitudMedicoRepositorio,SolicitudMedicoRepositorio>();
builder.Services.AddScoped(typeof(IRepositorioGenerico<>), typeof(RepositorioGenerico<>));
builder.Services.AddScoped<IPrecioRepositorio, PrecioRepositorio>();
builder.Services.AddScoped<ICirugiaRepositorio, CirugiaRepositorio>();
// Servicios
builder.Services.AddScoped<IUsuarioService, UsuarioServicio>();
builder.Services.AddTransient<IEmailServicio, EmailServicio>();
builder.Services.AddScoped<ICotizacionServicio, CotizacionServicio>();
builder.Services.AddScoped<ICitaServicio, CitaServicio>();
builder.Services.AddScoped<IPacienteServicio, PacienteServicio>();
builder.Services.AddScoped<IMedicoServicio, MedicoServicio>();
builder.Services.AddScoped<ISolicitudMedicoServicio,SolicitudMedicoServicio>();
builder.Services.AddScoped<IAuthServicio, AuthServicio>();
builder.Services.AddScoped<IPerfilServicio, PerfilServicio>();
builder.Services.AddScoped<IRolServicio, RolServicio>();
builder.Services.AddScoped<IEspecialidadServicio, EspecialidadServicio>();
builder.Services.AddScoped<IPaqueteServicio, PaqueteServicio>();
builder.Services.AddScoped<IProcedimientosServicio, ProcedimientosServicio>();
builder.Services.AddScoped<IHistorialMedicoServicio, HistorialMedicoServicio>();
builder.Services.AddScoped<IArchivoHistorialServicio, ArchivoHistorialServicio>();
builder.Services.AddScoped<IQuirofanoServicio, QuirofanoServicio>();
builder.Services.AddScoped<IHorarioServicio, HorarioServicio>();
builder.Services.AddScoped<ISoporteRepositorio, SoporteRepositorio>();
builder.Services.AddScoped<ISoporteServicio, SoporteServicio>();
builder.Services.AddScoped<IIconoServicio, IconoServicio>();

builder.Services.AddScoped<IRecetaServicio, RecetaServicio>();
builder.Services.AddScoped<IConfiguracionCheckPointsServicio, ConfiguracionCheckPointServicio>();
builder.Services.AddScoped<IPreguntasPOpServicio, PreguntasPOpServicio>();
builder.Services.AddScoped<ISeguimientoPacienteServicio, SeguimientoPacienteServicio>();
builder.Services.AddScoped<IRespuestaSeguimientoServicio, RespuestaSeguimientoServicio>();
builder.Services.AddScoped<IAlertaStaffServicio, AlertaStaffServicio>();
builder.Services.AddScoped<IConvenioServicio, ConvenioServicio>();
builder.Services.AddScoped<IPrecioServicio, PrecioServicio>();
builder.Services.AddScoped<ICirugiaServicio, CirugiaServicio>();


builder.Services.AddScoped<INotificacionServicio, NotificacionServicio>();
builder.Services.AddScoped<IConfiguracionServicio, ConfiguracionServicio>();
builder.Services.AddScoped<INotificadorServicio, NotificadorServicio>();
builder.Services.AddScoped<INotificacionUsuarioServicio, NotificacionUsuarioServicio>();
builder.Services.AddScoped<IPrecioRepositorio, PrecioRepositorio>();






builder.Services.AddControllersWithViews();
builder.Services.AddSession();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login_Logout/Login"; // Ruta a la página de inicio de sesión
        options.LogoutPath = "/Login_Logout/Logout"; // Ruta a la página de cierre de sesión
        options.AccessDeniedPath = "/Login_Logout/Login"; // Ruta a la página de acceso denegado
        options.ExpireTimeSpan = TimeSpan.FromHours(2); // Tiempo de expiración de la cookie
    });
builder.Services.AddAuthorizationBuilder()
     .AddPolicy("SoloAdministrador", policy => policy.RequireRole("Administrador"))
    .AddPolicy("SoloMedico", policy => policy.RequireRole("Medico"))
    .AddPolicy("SoloPaciente", policy => policy.RequireRole("Paciente"));

//Por favor no borrar esta linea sino no descarga la info de cirugias
QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    app.UseHsts();
}

app.UseHttpsRedirection();
app.MapStaticAssets();
app.UseRouting();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.MapRazorPages();


app.Run();