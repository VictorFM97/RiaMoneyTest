using API.Middleware;
using Application.Interfaces;
using Application.Services;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Interfaces;
using Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<GlobalRequestHandlingMiddleware>();

builder.Services.AddDbContext<CustomerContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("sql");
    options.UseSqlServer(connectionString);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CustomerContext>();

    var migrations = db.Database.GetPendingMigrations();

    if (migrations.Any())
    {
        db.Database.Migrate();
    }
}

app.UseMiddleware<GlobalRequestHandlingMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
