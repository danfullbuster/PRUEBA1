using AgriculturalCrm.Api.Data;
using AgriculturalCrm.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "CRM Agrícola — Clientes y parcelas",
        Version = "v1",
        Description = "PoC: registro de clientes y fincas (parcelas)."
    });
});

builder.Services.AddScoped<IClientesService, ClientesService>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(connectionString))
{
    builder.Services.AddDbContext<CrmDbContext>(options =>
        options.UseInMemoryDatabase("CrmInMemory"));
}
else
{
    builder.Services.AddDbContext<CrmDbContext>(options =>
        options.UseSqlServer(connectionString));
}

var corsOrigins = builder.Configuration.GetSection("Cors:Origins").Get<string[]>()
    ?? ["http://localhost:5173"];
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.WithOrigins(corsOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CRM Agrícola v1"));
}

app.UseCors();
app.UseHttpsRedirection();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CrmDbContext>();
    if (db.Database.IsRelational())
        await db.Database.MigrateAsync();
}

await app.RunAsync();
