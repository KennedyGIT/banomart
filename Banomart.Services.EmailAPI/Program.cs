using Banomart.Services.EmailAPI.Data;
using Banomart.Services.EmailAPI.Extensions;
using Banomart.Services.EmailAPI.Messaging;
using Banomart.Services.EmailAPI.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DatabaseContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var optionBuilder = new DbContextOptionsBuilder<DatabaseContext>();
optionBuilder.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
builder.Services.AddSingleton(new EmailService(optionBuilder.Options));

builder.Services.AddSingleton<IServiceBusConsumer, ServiceBusConsumer>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

ApplyPendingMigrations();

app.UseAzureServiceBusConsumer();

app.Run();

void ApplyPendingMigrations()
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

        if (db.Database.GetPendingMigrations().Count() > 0)
        {
            db.Database.Migrate();
        }
    }
}
