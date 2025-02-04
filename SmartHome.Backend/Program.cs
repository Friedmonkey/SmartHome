using FastEndpoints.Security;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextFactory<SmartHomeDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration["SmartHomeDb"]);
});

builder.Services.AddAuthentication("Bearer");

builder.Services
    .AddAuthenticationJwtBearer(x =>
    {
        x.SigningKey = builder.Configuration["JwtSigningKey"];
    })
    .AddAuthorization()
    .AddFastEndpoints()
    .SwaggerDocument();


builder.Services.AddHttpContextAccessor();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseDefaultExceptionHandler(useGenericReason: true);
app.UseFastEndpoints(fe => 
{
    fe.Security.RoleClaimType = "Role";
});

app.UseSwaggerGen();

app.Run();

public partial class Program { }