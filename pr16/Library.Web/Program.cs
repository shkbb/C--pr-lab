using Library.Application.Mapping;
using Library.Application.Repositories;
using Library.Application.Services;
using Library.Infrastructure.Data;
using Library.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Configure infrastructure
builder.Services.AddDbContext<LibraryContext>(options =>
    options.UseSqlServer(
        "Server=localhost,1433;Database=LibraryDB_pr15;User Id=sa;" +
        "Password=Your_Strong_Password_123!;TrustServerCertificate=True;"));

builder.Services.AddScoped<IBookRepository, BookRepository>();

// Configure application
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddAutoMapper(typeof(BookMappingProfile));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<LibraryContext>();
    db.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
