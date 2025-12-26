using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using StudentApiCore.Data;

var builder = WebApplication.CreateBuilder(args);

// -------------------- SERVICES --------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DB
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// CORS (Angular)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins(
            "http://localhost:4200",
            "http://localhost:8080"
        )
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

var app = builder.Build();

// -------------------- MIDDLEWARE --------------------

// Swagger
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Student API v1");
    options.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();

// 🔥 CSP + NONCE Middleware (BEFORE Static Files)
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/" ||
        context.Request.Path.Value?.EndsWith("index.html") == true)
    {
        var bytes = RandomNumberGenerator.GetBytes(16);
        var nonce = Convert.ToBase64String(bytes);

        var csp = new StringBuilder()
            .Append("default-src 'self'; ")
            .Append($"style-src 'self' 'nonce-{nonce}'; ")
            .Append($"script-src 'self' 'nonce-{nonce}'; ")
            .Append("img-src 'self' data: https:; ")
            .Append("font-src 'self' https:; ")
            .Append("connect-src 'self' https:; ")
            .Append("object-src 'none'; ")
            .Append("base-uri 'self'; ")
            .Append("frame-ancestors 'none';")
            .ToString();

        context.Response.Headers["Content-Security-Policy"] = csp;
        context.Response.Headers["Referrer-Policy"] = "strict-origin-when-cross-origin";

        var env = app.Environment;
        var webRoot = env.WebRootPath ?? Path.Combine(env.ContentRootPath, "wwwroot");
        var indexPath = Path.Combine(webRoot, "index.html");

        if (File.Exists(indexPath))
        {
            var html = await File.ReadAllTextAsync(indexPath);
            html = html.Replace("%CSP_NONCE%", nonce);

            context.Response.ContentType = "text/html; charset=utf-8";
            await context.Response.WriteAsync(html, Encoding.UTF8);
            return;
        }
    }

    await next();
});

// Static files (Angular build)
app.UseStaticFiles();

// CORS
app.UseCors("AllowAngular");

app.UseAuthorization();
app.MapControllers();

app.Run();
