using CACES.BLL;
using CACES.BLL.Servicios.ArchivosHistorial;
using CACES.BLL.Servicios.Auth;
using CACES.BLL.Servicios.Citas;
using CACES.BLL.Servicios.ConfirmacionCorreo;
using CACES.BLL.Servicios.Cotizaciones;
using CACES.BLL.Servicios.Especialidad;
using CACES.BLL.Servicios.Especialidad.ProyectoCACES.CACES.BLL.Servicios;
using CACES.BLL.Servicios.HistorialMedicos;
using CACES.BLL.Servicios.Horario;
using CACES.BLL.Servicios.Icono;
using CACES.BLL.Servicios.Medicos;
using CACES.BLL.Servicios.Paciente;
using CACES.BLL.Servicios.Paquete;
using CACES.BLL.Servicios.Perfil;
using CACES.BLL.Servicios.Procedimientos;
using CACES.BLL.Servicios.Quirofano;
using CACES.BLL.Servicios.Recetas;
using CACES.BLL.Servicios.Roles;
using CACES.BLL.Servicios.Soportes;
using CACES.BLL.Servicios.Usuario;
using CACES.DAL.DBContext;
using CACES.DAL.Repositorios.ArchivosHistorial;
using CACES.DAL.Repositorios.Citas;
using CACES.DAL.Repositorios.Cotizaciones;
using CACES.DAL.Repositorios.Especialidades;
using CACES.DAL.Repositorios.HistorialMedicos;
using CACES.DAL.Repositorios.Horarios;
using CACES.DAL.Repositorios.Icono;
using CACES.DAL.Repositorios.Medicos;
using CACES.DAL.Repositorios.Pacientes;
using CACES.DAL.Repositorios.Paquetes;
using CACES.DAL.Repositorios.Procedimientos;
using CACES.DAL.Repositorios.Quirofano;
using CACES.DAL.Repositorios.Recetas;
using CACES.DAL.Repositorios.Roles;
using CACES.DAL.Repositorios.Soportes;
using CACES.DAL.Repositorios.Usuario;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;


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
builder.Services.AddScoped<IIconoRepositorio, IconoRepositorio>();
builder.Services.AddScoped<IRecetaRepositorio, RecetaRepositorio>();
builder.Services.AddScoped<ICotizacionRepositorio, CotizacionRepositorio>();

// Servicios
builder.Services.AddScoped<IUsuarioService, UsuarioServicio>();
builder.Services.AddTransient<IEmailServicio, EmailServicio>();
builder.Services.AddScoped<ICotizacionServicio, CotizacionServicio>();
builder.Services.AddScoped<ICitaServicio, CitaServicio>();
builder.Services.AddScoped<IPacienteServicio, PacienteServicio>();
builder.Services.AddScoped<IMedicoServicio, MedicoServicio>();

builder.Services.AddScoped<IAuthServicio, AuthServicio>();
builder.Services.AddScoped<IPerfilServicio, PerfilServicio>();
builder.Services.AddScoped<IRolRepositorio, RolRepositorio>();
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