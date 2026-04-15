using Library.Application.Mapping;
using Library.Application.Repositories;
using Library.Application.Services;
using Library.Infrastructure.Data;
using Library.Infrastructure.Repositories;
using Library.Api.Middleware;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure infrastructure components (DbContext, Repositories)
builder.Services.AddDbContext<LibraryContext>(options =>
    options.UseSqlServer(
        "Server=localhost,1433;Database=LibraryDB_lab12;User Id=sa;" +
        "Password=Your_Strong_Password_123!;TrustServerCertificate=True;"));

builder.Services.AddScoped<IBookRepository, BookRepository>();

// Configure application components (Services, AutoMapper)
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddAutoMapper(typeof(BookMappingProfile));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Library API - Лабораторна 12 (Layered)", Version = "v1" });
});

var app = builder.Build();

// Ensure DB is created for demo purposes
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<LibraryContext>();
    db.Database.EnsureCreated();
}

app.UseRequestLogging();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Library API v1");
    c.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
