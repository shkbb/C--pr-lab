using LibraryApi.Data;
using LibraryApi.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ── Services ────────────────────────────────────────────────────────────────
builder.Services.AddControllers();

// EF Core + SQL Server
builder.Services.AddDbContext<LibraryContext>(options =>
    options.UseSqlServer(
        "Server=localhost,1433;Database=LibraryDB_pr11;User Id=sa;" +
        "Password=Your_Strong_Password_123!;TrustServerCertificate=True;"));

// Business logic service via DI
builder.Services.AddScoped<IBookService, BookService>();

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Library API — Практична 11", Version = "v1" });
});

var app = builder.Build();

// ── Ensure DB is created ─────────────────────────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<LibraryContext>();
    db.Database.EnsureCreated();
}

// ── Middleware pipeline ──────────────────────────────────────────────────────
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Library API v1");
    c.RoutePrefix = string.Empty; // Swagger at root
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
