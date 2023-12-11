using Microsoft.EntityFrameworkCore;
using ShopOnline.Api.Data;
using ShopOnline.Api.Repositories.Contracts;
using ShopOnline.Api.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// this line of code is adding a DbContext to the dependency injection container,
// specifying that SQL Server should be used as the database provider,
// and providing the connection string for connecting to the database
builder.Services.AddDbContextPool<ShopOnlineDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("ShopOnlineConnection")));


// Here’s a breakdown:
// builder.Services: This accesses the Services property of the builder object,
// which is an IServiceCollection. This is essentially a registry of all the services available
// in your application.

// .AddScoped<IProductRepository, ProductRepository>():
// This is registering the ProductRepository class as the implementation
// of the IProductRepository interface.

// The AddScoped method means that a new instance of ProductRepository
// will be created once per client request. This is in contrast to AddSingleton,
// which would create a single instance for the entire application, or AddTransient,
// which would create a new instance every time one is requested.
builder.Services.AddScoped<IProductRepository, ProductRepository>();

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

app.Run();
