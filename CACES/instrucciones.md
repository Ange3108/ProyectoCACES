# Buenas Prácticas de Programación en ASP.NET 10

## 📋 Tabla de Contenidos
1. [Estructura del Proyecto](#estructura-del-proyecto)
2. [Convenciones de Código](#convenciones-de-código)
3. [Async/Await](#asyncawait)
4. [Dependency Injection](#dependency-injection)
5. [Entity Framework](#entity-framework)
6. [Seguridad](#seguridad)
7. [Validación](#validación)
8. [Logging](#logging)
9. [Testing](#testing)
10. [Performance](#performance)

---

## Estructura del Proyecto

### Arquitectura en Capas Recomendada

```
ProyectoCACES/
├── CACES/                      # Proyecto Principal (Presentación - Razor Pages)
│   ├── Pages/                  # Razor Pages
│   ├── wwwroot/               # Archivos estáticos
│   ├── appsettings.json       # Configuración
│   └── Program.cs             # Configuración principal
├── CACES.DAL/                 # Data Access Layer
│   ├── Entidades/             # Modelos de BD
│   ├── Contexto/              # DbContext
│   └── Repositorios/          # Implementación de repositorios
├── CACES.BLL/                 # Business Logic Layer
│   ├── Servicios/             # Lógica de negocio
│   └── DTOs/                  # Data Transfer Objects
└── CACES.Test/                # Pruebas unitarias
```

---

## Convenciones de Código

### 1. Nomenclatura

```csharp
// ✅ CORRECTO
public class MedicoRepositorio { }
public interface IMedicoServicio { }
public async Task<Medico> ObtenerMedicoAsync(int id) { }
private const string CLAVE_CACHE = "medicos";
private readonly ILogger<MedicoServicio> _logger;

// ❌ INCORRECTO
public class medicoRepositorio { }
public interface MedicoServicio { }
public Task obtener_medico(int id) { }
private const string cache_key = "medicos";
```

### 2. Propiedades y Campos

```csharp
// ✅ CORRECTO
public class PacienteServicio
{
    private readonly IPacienteRepositorio _repositorio;
    private readonly IMapper _mapper;

    public string Nombre { get; set; }
    public int Edad { get; private set; }
    public DateTime FechaRegistro { get; } = DateTime.Now;
}

// ❌ INCORRECTO
public class PacienteServicio
{
    public IPacienteRepositorio repositorio;
    public string nombre;
    private int edad;
}
```

### 3. Uso de var

```csharp
// ✅ CORRECTO - var es claro
var pacientes = await _repositorio.ObtenerTodosAsync();
var fecha = DateTime.Now;

// ⚠️ EVITAR
var x = ObtenerValor();  // ¿Qué tipo es x?
var resultado = new List<Medico> { }; // Tipo es obvio, pero es innecesario
```

---

## Async/Await

### 1. Siempre usar async en métodos I/O

```csharp
// ✅ CORRECTO
public async Task<List<Medico>> ObtenerMedicosAsync()
{
    return await _context.Medicos.ToListAsync();
}

public async Task<ActionResult> CrearMedicoAsync(Medico medico)
{
    await _repositorio.AgregarAsync(medico);
    await _repositorio.GuardarCambiosAsync();
    return CreatedAtAction(nameof(ObtenerMedicoAsync), new { id = medico.IdMedico }, medico);
}

// ❌ INCORRECTO
public List<Medico> ObtenerMedicos()
{
    return _context.Medicos.ToList(); // Bloqueante
}

// ❌ INCORRECTO - Async over Sync
public async Task<List<Medico>> ObtenerMedicosAsync()
{
    return _context.Medicos.ToList(); // Sin await, sin async
}
```

### 2. Naming Convention

```csharp
// ✅ Todos los métodos async deben terminar en "Async"
public async Task<Medico> ObtenerPorIdAsync(int id)
public async Task<bool> ExisteAsync(int id)
public async Task GuardarAsync()
```

### 3. ConfigureAwait en librerías

```csharp
// ✅ CORRECTO en librerías (DAL, BLL)
public async Task<Medico> ObtenerAsync(int id)
{
    return await _context.Medicos
        .FirstOrDefaultAsync(m => m.IdMedico == id)
        .ConfigureAwait(false);
}

// ℹ️ En Controllers/Pages no es necesario ConfigureAwait
```

---

## Dependency Injection

### 1. Configuración en Program.cs

```csharp
// ✅ CORRECTO - ASP.NET 10
var builder = WebApplicationBuilder.CreateBuilder(args);

// Servicios
builder.Services.AddScoped<IMedicoRepositorio, MedicoRepositorio>();
builder.Services.AddScoped<IMedicoServicio, MedicoServicio>();
builder.Services.AddScoped<IPacienteRepositorio, PacienteRepositorio>();
builder.Services.AddScoped<IPacienteServicio, PacienteServicio>();

// DbContext
builder.Services.AddDbContext<CACESContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Logging
builder.Services.AddLogging(config =>
{
    config.AddConsole();
    config.AddDebug();
    config.AddFile("logs/caces-{Date}.txt");
});

var app = builder.Build();
```

### 2. Inyección en Servicios

```csharp
// ✅ CORRECTO
public class MedicoServicio : IMedicoServicio
{
    private readonly IMedicoRepositorio _repositorio;
    private readonly IMapper _mapper;
    private readonly ILogger<MedicoServicio> _logger;

    public MedicoServicio(
        IMedicoRepositorio repositorio,
        IMapper mapper,
        ILogger<MedicoServicio> logger)
    {
        _repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
}

// ❌ INCORRECTO
public class MedicoServicio : IMedicoServicio
{
    private IMedicoRepositorio _repositorio = new MedicoRepositorio(); // Acoplamiento
}
```

---

## Entity Framework

### 1. Configuración de DbContext

```csharp
// ✅ CORRECTO
public class CACESContext : DbContext
{
    public CACESContext(DbContextOptions<CACESContext> options)
        : base(options) { }

    public DbSet<Medico> Medicos { get; set; }
    public DbSet<Paciente> Pacientes { get; set; }
    public DbSet<HistorialMedico> HistorialesMedicos { get; set; }
    public DbSet<Cita> Citas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuraciones de entidades
        modelBuilder.Entity<Medico>()
            .HasKey(m => m.IdMedico);

        modelBuilder.Entity<Medico>()
            .HasOne(m => m.Usuario)
            .WithMany(u => u.Medicos)
            .HasForeignKey(m => m.IdUsuario)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
```

### 2. Queries Optimizadas

```csharp
// ✅ CORRECTO - Usar Select para traer solo campos necesarios
public async Task<List<MedicoDTO>> ObtenerMedicosPorEspecialidadAsync(int idEspecialidad)
{
    return await _context.Medicos
        .AsNoTracking() // Solo lectura
        .Where(m => m.IdEspecialidad == idEspecialidad && m.FechaDeRegistro != null)
        .Select(m => new MedicoDTO
        {
            IdMedico = m.IdMedico,
            Nombres = m.Usuario.Nombres,
            Especialidad = m.Especialidad.Nombre,
            Experiencia = m.Experiencia
        })
        .ToListAsync();
}

// ❌ INCORRECTO - Trae todas las columnas y relaciones
public async Task<List<Medico>> ObtenerMedicosPorEspecialidad(int idEspecialidad)
{
    return await _context.Medicos
        .Where(m => m.IdEspecialidad == idEspecialidad)
        .Include(m => m.Usuario)
        .Include(m => m.Especialidad)
        .Include(m => m.Citas) // N+1 problem
        .ToListAsync();
}

// ❌ INCORRECTO - Uso de Lazy Loading
var medicos = await _context.Medicos.ToListAsync();
foreach (var medico in medicos)
{
    var usuario = medico.Usuario; // Query adicional por cada médico
}
```

### 3. Migrations

```bash
# Crear migration
dotnet ef migrations add NombreMigration

# Actualizar BD
dotnet ef database update

# Revertir migration
dotnet ef database update NombreMigrationAnterior

# Ver pending migrations
dotnet ef migrations list
```

---

## Seguridad

### 1. Contraseñas

```csharp
// ✅ CORRECTO - Usar Identity o BCrypt
using System.Security.Cryptography;

public class UsuarioServicio
{
    public string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }

    public bool VerifyPassword(string password, string hash)
    {
        var hashOfInput = HashPassword(password);
        return hashOfInput.Equals(hash);
    }
}

// ℹ️ Mejor: Usar ASP.NET Identity
```

### 2. Validación de Entrada

```csharp
// ✅ CORRECTO - Validar siempre entrada del usuario
[HttpPost]
public async Task<ActionResult> CrearMedico([FromBody] CrearMedicoDTO dto)
{
    if (!ModelState.IsValid)
        return BadRequest(ModelState);

    if (string.IsNullOrWhiteSpace(dto.Nombres))
        return BadRequest("El nombre no puede estar vacío");

    if (await _medico_Servicio.ExisteAsync(dto.DUI))
        return BadRequest("El médico ya está registrado");

    var medico = await _medicoServicio.CrearAsync(dto);
    return CreatedAtAction(nameof(ObtenerMedico), new { id = medico.IdMedico }, medico);
}

// ❌ INCORRECTO
[HttpPost]
public async Task<ActionResult> CrearMedico([FromBody] Medico medico)
{
    var resultado = await _context.Medicos.AddAsync(medico);
    await _context.SaveChangesAsync();
    return Ok(resultado);
}
```

### 3. SQL Injection Prevention

```csharp
// ✅ CORRECTO - Usar parametrizados
var medicos = await _context.Medicos
    .Where(m => m.DUI == dui)  // Parametrizado
    .ToListAsync();

// ❌ INCORRECTO - String Interpolation en SQL
var medicos = await _context.Medicos
    .FromSqlInterpolated($"SELECT * FROM Medicos WHERE DUI = {dui}")
    .ToListAsync();
```

### 4. HTTPS y Headers de Seguridad

```csharp
// En Program.cs
app.UseHsts(); // HSTS header
app.UseHttpsRedirection();

// En appsettings.json
{
  "Kestrel": {
    "Endpoints": {
      "Http": {
        "Url": "http://localhost:5000"
      },
      "Https": {
        "Url": "https://localhost:5001",
        "Certificate": {
          "Path": "path/to/cert.pfx",
          "Password": "password"
        }
      }
    }
  }
}
```

---

## Validación

### 1. Data Annotations

```csharp
// ✅ CORRECTO
public class MedicoDTO
{
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(100, MinimumLength = 2, 
        ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres")]
    public string Nombres { get; set; }

    [Required]
    [EmailAddress(ErrorMessage = "Correo inválido")]
    public string CorreoElectronico { get; set; }

    [Range(0, 60, ErrorMessage = "La experiencia debe estar entre 0 y 60 años")]
    public int Experiencia { get; set; }

    [RegularExpression(@"^\d{8}$", ErrorMessage = "El DUI debe tener 8 dígitos")]
    public string DUI { get; set; }

    [Phone(ErrorMessage = "Teléfono inválido")]
    public string Telefono { get; set; }
}
```

### 2. Validación Personalizada

```csharp
// ✅ CORRECTO
public class ValidarDUIAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is not string dui)
            return ValidationResult.Success;

        if (dui.Length != 8 || !dui.All(char.IsDigit))
            return new ValidationResult("DUI inválido");

        return ValidationResult.Success;
    }
}

public class MedicoDTO
{
    [ValidarDUI]
    public string DUI { get; set; }
}
```

---

## Logging

### 1. Configuración Básica

```csharp
// En Program.cs
builder.Services.AddLogging(config =>
{
    config.ClearProviders();
    config.AddConsole();
    config.AddDebug();
    config.AddFile("logs/caces-{Date}.txt");
    config.SetMinimumLevel(LogLevel.Information);
});

// ℹ️ En Producción: usar Serilog, Application Insights, ELK, etc.
```

### 2. Uso de Logging

```csharp
// ✅ CORRECTO
public class MedicoServicio
{
    private readonly ILogger<MedicoServicio> _logger;

    public async Task<Medico> ObtenerAsync(int id)
    {
        _logger.LogInformation("Obteniendo médico con ID: {IdMedico}", id);

        try
        {
            var medico = await _repositorio.ObtenerAsync(id);
            if (medico == null)
            {
                _logger.LogWarning("Médico no encontrado. ID: {IdMedico}", id);
                return null;
            }

            _logger.LogInformation("Médico obtenido exitosamente. ID: {IdMedico}", id);
            return medico;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener médico. ID: {IdMedico}", id);
            throw;
        }
    }
}

// ❌ INCORRECTO
public async Task<Medico> Obtener(int id)
{
    Console.WriteLine($"Obteniendo médico {id}"); // No se ve en producción
    return await _repositorio.ObtenerAsync(id);
}
```

---

## Testing

### 1. Estructura de Tests

```csharp
// ✅ CORRECTO
[TestClass]
public class MedicoServicioTests
{
    private Mock<IMedicoRepositorio> _mockRepositorio;
    private MedicoServicio _servicio;

    [TestInitialize]
    public void Setup()
    {
        _mockRepositorio = new Mock<IMedicoRepositorio>();
        _servicio = new MedicoServicio(_mockRepositorio.Object);
    }

    [TestMethod]
    public async Task ObtenerAsync_ConIdValido_RetornaMedico()
    {
        // Arrange
        var idEsperado = 1;
        var medicoEsperado = new Medico { IdMedico = idEsperado, Nombres = "Juan" };
        _mockRepositorio.Setup(r => r.ObtenerAsync(idEsperado))
            .ReturnsAsync(medicoEsperado);

        // Act
        var resultado = await _servicio.ObtenerAsync(idEsperado);

        // Assert
        Assert.IsNotNull(resultado);
        Assert.AreEqual(idEsperado, resultado.IdMedico);
        _mockRepositorio.Verify(r => r.ObtenerAsync(idEsperado), Times.Once);
    }

    [TestMethod]
    public async Task ObtenerAsync_ConIdInvalido_RetornaNull()
    {
        // Arrange
        _mockRepositorio.Setup(r => r.ObtenerAsync(It.IsAny<int>()))
            .ReturnsAsync((Medico)null);

        // Act
        var resultado = await _servicio.ObtenerAsync(-1);

        // Assert
        Assert.IsNull(resultado);
    }
}
```

### 2. Pruebas de Integración

```csharp
// ✅ CORRECTO
[TestClass]
public class MedicoIntegrationTests : IAsyncLifetime
{
    private WebApplicationFactory<Program> _factory;
    private HttpClient _client;

    public async Task InitializeAsync()
    {
        _factory = new WebApplicationFactory<Program>();
        _client = _factory.CreateClient();
        // Seed datos de prueba
        await SeedDatosAsync();
    }

    public async Task DisposeAsync()
    {
        _client?.Dispose();
        _factory?.Dispose();
    }

    [TestMethod]
    public async Task GetMedico_ConIdValido_Retorna200()
    {
        // Act
        var response = await _client.GetAsync("/api/medicos/1");

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().NotBeNullOrEmpty();
    }
}
```

---

## Performance

### 1. Caching

```csharp
// ✅ CORRECTO
public class MedicoServicio
{
    private readonly IMemoryCache _cache;
    private const string CACHE_KEY_MEDICOS = "medicos_todos";
    private const int CACHE_DURACION_MINUTOS = 30;

    public async Task<List<Medico>> ObtenerTodosAsync()
    {
        if (_cache.TryGetValue(CACHE_KEY_MEDICOS, out List<Medico> medicos))
        {
            return medicos;
        }

        medicos = await _repositorio.ObtenerTodosAsync();

        var cacheOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(CACHE_DURACION_MINUTOS));

        _cache.Set(CACHE_KEY_MEDICOS, medicos, cacheOptions);
        return medicos;
    }

    public async Task<Medico> CrearAsync(Medico medico)
    {
        var resultado = await _repositorio.AgregarAsync(medico);
        _cache.Remove(CACHE_KEY_MEDICOS); // Invalidar cache
        return resultado;
    }
}
```

### 2. Paginación

```csharp
// ✅ CORRECTO
public async Task<PagedResult<MedicoDTO>> ObtenerPaginadoAsync(int pagina = 1, int tamano = 10)
{
    var total = await _context.Medicos.CountAsync();

    var medicos = await _context.Medicos
        .AsNoTracking()
        .OrderBy(m => m.IdMedico)
        .Skip((pagina - 1) * tamano)
        .Take(tamano)
        .Select(m => new MedicoDTO { /* ... */ })
        .ToListAsync();

    return new PagedResult<MedicoDTO>
    {
        Datos = medicos,
        Total = total,
        Pagina = pagina,
        Tamano = tamano,
        TotalPaginas = (int)Math.Ceiling(total / (double)tamano)
    };
}

// En Razor Page
public class IndexModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public int Pagina { get; set; } = 1;

    public PagedResult<MedicoDTO> Resultado { get; set; }

    public async Task OnGetAsync()
    {
        Resultado = await _servicio.ObtenerPaginadoAsync(Pagina, 10);
    }
}
```

### 3. DTOs vs Entidades

```csharp
// ✅ CORRECTO - Usar DTOs para APIs
public class MedicoDTO
{
    public int IdMedico { get; set; }
    public string Nombres { get; set; }
    public string Especialidad { get; set; }
    // Solo campos necesarios
}

// ❌ INCORRECTO - Exponer entidad completa
[HttpGet("{id}")]
public async Task<ActionResult<Medico>> Get(int id)
{
    return await _context.Medicos.FindAsync(id);
    // Expone contraseñas, datos sensibles, etc.
}
```

---

## Patrones Recomendados

### 1. Repository Pattern

```csharp
// ✅ CORRECTO
public interface IMedicoRepositorio
{
    Task<Medico> ObtenerPorIdAsync(int id);
    Task<List<Medico>> ObtenerTodosAsync();
    Task AgregarAsync(Medico medico);
    Task ActualizarAsync(Medico medico);
    Task EliminarAsync(int id);
}

public class MedicoRepositorio : IMedicoRepositorio
{
    private readonly CACESContext _context;

    public MedicoRepositorio(CACESContext context)
    {
        _context = context;
    }

    public async Task<Medico> ObtenerPorIdAsync(int id)
    {
        return await _context.Medicos
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.IdMedico == id);
    }

    public async Task AgregarAsync(Medico medico)
    {
        await _context.Medicos.AddAsync(medico);
        await _context.SaveChangesAsync();
    }
}
```

### 2. Unit of Work Pattern

```csharp
// ✅ CORRECTO
public interface IUnitOfWork : IDisposable
{
    IMedicoRepositorio Medicos { get; }
    IPacienteRepositorio Pacientes { get; }
    IHistorialRepositorio Historiables { get; }
    Task<int> SaveChangesAsync();
}

public class UnitOfWork : IUnitOfWork
{
    private readonly CACESContext _context;
    private IMedicoRepositorio _medicoRepositorio;
    private IPacienteRepositorio _pacienteRepositorio;

    public IMedicoRepositorio Medicos 
        => _medicoRepositorio ??= new MedicoRepositorio(_context);

    public IPacienteRepositorio Pacientes 
        => _pacienteRepositorio ??= new PacienteRepositorio(_context);

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}
```

---

## Ejemplo Completo: Crear Médico

```csharp
// 1. DTO (Entrada)
public class CrearMedicoDTO
{
    [Required]
    [StringLength(100)]
    public string Nombres { get; set; }

    [Required]
    [EmailAddress]
    public string CorreoElectronico { get; set; }

    [Required]
    [Range(0, 60)]
    public int Experiencia { get; set; }
}

// 2. Servicio
public interface IMedicoServicio
{
    Task<MedicoDTO> CrearAsync(CrearMedicoDTO dto);
}

public class MedicoServicio : IMedicoServicio
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<MedicoServicio> _logger;

    public MedicoServicio(IUnitOfWork unitOfWork, IMapper mapper, ILogger<MedicoServicio> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<MedicoDTO> CrearAsync(CrearMedicoDTO dto)
    {
        _logger.LogInformation("Creando nuevo médico: {Nombres}", dto.Nombres);

        var medico = _mapper.Map<Medico>(dto);
        medico.FechaDeRegistro = DateTime.Now;

        await _unitOfWork.Medicos.AgregarAsync(medico);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Médico creado exitosamente. ID: {IdMedico}", medico.IdMedico);

        return _mapper.Map<MedicoDTO>(medico);
    }
}

// 3. Página Razor
public class CrearModel : PageModel
{
    private readonly IMedicoServicio _servicio;

    [BindProperty]
    public CrearMedicoDTO Medico { get; set; }

    public CrearModel(IMedicoServicio servicio)
    {
        _servicio = servicio;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        try
        {
            await _servicio.CrearAsync(Medico);
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, "Error al crear el médico");
            return Page();
        }
    }
}
```

---

## Checklist Final

- [ ] Código sigue convenciones de nombres
- [ ] Métodos I/O son async
- [ ] Se usa Dependency Injection
- [ ] Validación en DTOs
- [ ] Logging en puntos críticos
- [ ] Queries optimizadas en EF
- [ ] Manejo de errores adecuado
- [ ] Tests unitarios escritos
- [ ] SQL Injection prevenido
- [ ] Contraseñas hasheadas
- [ ] Datos sensibles no expuestos
- [ ] Performance considerado
- [ ] Documentación código importante

---

**Última actualización:** 2024 | **ASP.NET:** 10 | **C#:** 14