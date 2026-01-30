var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

// AutoMapper
builder.Services.AddAutoMapper(cfg => cfg.LicenseKey = "<License Key Here>", typeof(EntitiesMapperProfile).Assembly);
builder.Services.AddDbContext<AssetDbContext>(opt =>
{
    // Default: local SQLite file for easy reviewer experience
    var cs = builder.Configuration.GetConnectionString("Default")
             ?? "Data Source=assetprices.db";

    opt.UseSqlite(cs);
});

builder.Services.AddTransient<ApiExceptionMiddleware>();

builder.Services.AddScoped<IAssetService, AssetService>();
builder.Services.AddScoped<IAssetPriceService, AssetPriceService>();
var app = builder.Build();

// DB init (simple for coding exercise). For production, prefer migrations in CI/CD.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AssetDbContext>();
    await db.Database.EnsureCreatedAsync();
    await DbSeeder.SeedAsync(db);
}


// Use custom API exception middleware
app.UseMiddleware<ApiExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "PlatformOne Assets API v1");
        c.RoutePrefix = "swagger"; // ensures it maps to /swagger/index.html
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
