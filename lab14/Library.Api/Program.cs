using Library.Application.Mapping;
using Library.Application.Repositories;
using Library.Application.Services;
using Library.Infrastructure.Data;
using Library.Infrastructure.Repositories;
using Library.Api.Middleware;
using Library.Api.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options => 
{
    options.Filters.Add<ActionLoggingFilter>();
});
builder.Services.AddMemoryCache();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "LibraryApi",
            ValidAudience = "LibraryApiClients",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey_12345678901234567890"))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("FullLibraryAccess", policy => policy.RequireClaim("LibraryAccess", "Full"));
});

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
    c.SwaggerDoc("v1", new() { Title = "Library API - Практична 17", Version = "v1" });
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Ensure DB is created for demo purposes
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<LibraryContext>();
    db.Database.EnsureCreated();
}

app.UseGlobalExceptionHandler();
app.UseRequestLogging();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Library API v1");
    c.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
