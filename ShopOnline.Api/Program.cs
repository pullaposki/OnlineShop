using Microsoft.EntityFrameworkCore;
using ShopOnline.Api.Data;

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
