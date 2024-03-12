using Application.Interfaces;
using Application.Services;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Interfaces;
using Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ICustomerService, CustomerService>();
builder.Services.AddSingleton<ICustomerRepository, CustomerRepository>();

builder.Services.AddDbContext<CustomerContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("sql");
    options.UseSqlServer(connectionString);
}, ServiceLifetime.Singleton);

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
