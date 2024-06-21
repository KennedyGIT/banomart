using AutoMapper;
using Banomart.Services.ShoppingCartAPI;
using Banomart.Services.ShoppingCartAPI.Data;
using Banomart.Services.ShoppingCartAPI.Extensions;
using Banomart.Services.ShoppingCartAPI.Service;
using Banomart.Services.ShoppingCartAPI.Service.IService;
using Banomart.Services.ShoppingCartAPI.Utility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<DatabaseContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


IMapper mapper = MappingProfiles.RegisterMappings().CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddHttpClient("Product", p => p.BaseAddress = new Uri(builder.Configuration["ServiceUrls:ProductAPI"])).AddHttpMessageHandler<AuthenticationHttpClientHandler>(); 

builder.Services.AddHttpClient("Coupon", c => c.BaseAddress = new Uri(builder.Configuration["ServiceUrls:CouponAPI"])).AddHttpMessageHandler<AuthenticationHttpClientHandler>();

builder.Services.AddScoped<IProductService, ProducService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<AuthenticationHttpClientHandler>();
builder.Services.AddScoped<ICouponService, CouponService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o =>
{
    o.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme, securityScheme: new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter the bearer Authorization string as follows: 'Bearer {Generated Auth Token}'",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    o.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                }
            }, new string[]{ }
        }
    });
}

);

builder.AddApplicationAuthentication();

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

ApplyPendingMigrations();

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
