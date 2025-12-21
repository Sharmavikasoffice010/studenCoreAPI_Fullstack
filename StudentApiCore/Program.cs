using Microsoft.EntityFrameworkCore;
using StudentApiCore.Data;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DB
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// 🔥 CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

// Swagger (IIS / Prod)
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Student API v1");
    options.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();

// ✅ THIS LINE WAS MISSING
app.UseCors("AllowAngular");

app.UseAuthorization();
app.MapControllers();
app.Run();
