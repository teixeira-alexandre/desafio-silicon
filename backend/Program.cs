using Microsoft.EntityFrameworkCore;
using ProjetoBanco.Data;
using ProjetoBanco.Services;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("libera_front", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://127.0.0.1:5173")
              .AllowAnyHeader().AllowAnyMethod();
    });
});

// MySQL
builder.Services.AddDbContext<BancoContext>(options =>
{
    var cs = builder.Configuration.GetConnectionString("mysql");
    options.UseMySql(cs, ServerVersion.AutoDetect(cs));
});

builder.Services.AddScoped<ContaService>();

builder.Services
  .AddControllers()
  .AddJsonOptions(o =>
  {
      o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
      o.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
  });

builder.Services.AddSwaggerGen(); // Inicializa o Swagger

var app = builder.Build();

Console.WriteLine("Pre Swagger");
app.UseSwagger();
app.UseSwaggerUI();
Console.WriteLine("Pos Swagger");
app.UseCors("libera_front");
app.MapControllers();

app.Run();


