using CACES.BLL;
using CACES.BLL.Servicios.Auth;
using CACES.BLL.Servicios.ConfirmacionCorreo;
using CACES.BLL.Servicios.Medicos;
using CACES.BLL.Servicios.Paciente;
using CACES.BLL.Servicios.Perfil;
using CACES.BLL.Servicios.Usuario;
using CACES.DAL.DBContext;
using CACES.DAL.Repositorios.HistorialMedicos;
using CACES.DAL.Repositorios.Medicos;
using CACES.DAL.Repositorios.Pacientes;
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

// Registrar servicios y repositorios (añadir esta línea)
builder.Services.AddScoped<IUsuarioService, UsuarioServicio>();
builder.Services.AddTransient<IEmailServicio, EmailServicio>();
builder.Services.AddScoped<IPerfilServicio, PerfilServicio>();

// Servicios
builder.Services.AddAutoMapper(cfg => { }, typeof(MapeoClases)); // Directamente desde la documentación


builder.Services.AddScoped<IMedicoRepositorio, MedicoRepositorio>();
builder.Services.AddScoped<IPacienteServicio, PacienteServicio>();
builder.Services.AddScoped<IMedicoServicio, MedicoServicio>();
builder.Services.AddScoped<IAuthServicio, AuthServicio>();
builder.Services.AddAutoMapper(cfg => { }, typeof(MapeoClases)); // Directamente desde la documentación

builder.Services.AddControllersWithViews();
builder.Services.AddSession();


//Configura el esquema de autenticación y autorización basado en Cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login"; // Ruta a la página de inicio de sesión
        options.LogoutPath = "/Auth/Logout"; // Ruta a la página de cierre de sesión
        options.AccessDeniedPath = "/Auth/AccessDenied"; // Ruta a la página de acceso denegado
        options.ExpireTimeSpan = TimeSpan.FromHours(2); // Tiempo de expiración de la cookie
    });
builder.Services.AddAuthorizationBuilder()
     .AddPolicy("SoloAdministrador", policy => policy.RequireRole("Administrador"))
    .AddPolicy("SoloMedico", policy => policy.RequireRole("Médico"))
    .AddPolicy("SoloPaciente", policy => policy.RequireRole("Paciente"));


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
