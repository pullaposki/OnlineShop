using Microsoft.EntityFrameworkCore;
using ShopOnline.Api.Data;
using ShopOnline.Api.Repositories.Contracts;
using ShopOnline.Api.Repositories;
using Microsoft.Net.Http.Headers;

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

// In summary, this line of code configures the application to allow cross-origin requests
// from “https://localhost:7232” and “http://localhost:7232”, using any HTTP method, and allowing the ‘Content-Type’ header.
// This is typically done to allow a web application running at one origin to access select resources from a server at a different origin.
app.UseCors(policy => policy.WithOrigins("https://localhost:7232", "http://localhost:7232").
	AllowAnyMethod().WithHeaders(HeaderNames.ContentType));

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
