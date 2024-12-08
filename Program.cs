using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using System.Threading.RateLimiting;
using TSNO.BackgroundServices;
using TSNO.Data;
using TSNO.Services.Expiration;

var builder = WebApplication.CreateBuilder(args);
 
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IExpirationService, ExpirationService>();
builder.Services.AddHostedService<ExpirationBackgroundService>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection") ??
        Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null) // Retry for transient errors
    )
);

string clientUrl = builder.Configuration.GetValue<string>("CLIENT_URL")!;

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
        policy.WithOrigins(clientUrl)
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials());
});

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("FixedPolicy", policy =>
    {
        policy.PermitLimit = 15;
        policy.Window = TimeSpan.FromMinutes(1);
        policy.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        policy.QueueLimit = 2;
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();

    try
    {
        Console.WriteLine("Checking database connectivity...");
        if (!context.Database.CanConnect())
        {
            Console.WriteLine("Database not found. Applying migrations...");
            context.Database.Migrate();
            Console.WriteLine("Database created and migrations applied successfully.");
        }
        else
        {
            Console.WriteLine("Database exists. Checking for pending migrations...");
            var pendingMigrations = context.Database.GetPendingMigrations();
            if (pendingMigrations.Any())
            {
                Console.WriteLine("Applying pending migrations...");
                context.Database.Migrate();
                Console.WriteLine("Migrations applied successfully.");
            }
            else
            {
                Console.WriteLine("No pending migrations found.");
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error during database initialization: {ex.Message}");

 
        if (ex.Message.Contains("already exists"))
        {
            Console.WriteLine("Database already exists. Skipping creation.");
        }
        else
        {
            throw;
        }
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHttpsRedirection();
}

app.UseCors("AllowSpecificOrigin");
app.UseRateLimiter();
app.UseAuthorization();
app.MapControllers();
app.Run();
