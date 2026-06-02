using CACES.BLL;
using CACES.BLL.Servicios.ConfirmacionCorreo;
using CACES.BLL.Servicios.Paciente;
using CACES.BLL.Servicios.Usuario;
using CACES.DAL.DBContext;
using CACES.DAL.Repositorios.Pacientes;
using CACES.DAL.Repositorios.Usuario;
using Microsoft.EntityFrameworkCore;
using CACES.DAL.Repositorios.HistorialMedicos;
using CACES.DAL.Repositorios.Medicos;
using CACES.BLL.Servicios.Medicos;
using CACES.BLL.Servicios.Auth;

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
// Registrar servicios y repositorios (añadir esta línea)
builder.Services.AddScoped<IUsuarioService, UsuarioServicio>();
builder.Services.AddTransient<IEmailServicio, EmailServicio>();
// Servicios
builder.Services.AddAutoMapper(cfg => { }, typeof(MapeoClases)); // Directamente desde la documentación

builder.Services.AddScoped<IHistorialMedicoRepositorio, HistorialMedicoRepositorio>();
builder.Services.AddScoped<IPacienteServicio, PacienteServicio>();

builder.Services.AddScoped<IMedicoRepositorio, MedicoRepositorio>();
builder.Services.AddScoped<IMedicoServicio, MedicoServicio>();

builder.Services.AddControllersWithViews();
builder.Services.AddSession();

builder.Services.AddScoped<IAuthServicio, AuthServicio>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.MapRazorPages();


app.Run();
