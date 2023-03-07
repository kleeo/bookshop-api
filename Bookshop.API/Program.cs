using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Bookshop.Data;
using Bookshop.Services.Interfaces;
using Bookshop.Services;
using System.Text.Json.Serialization;

var bookshopUIAccess = "Bookshop UI Access";
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<BookshopDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("BookshopContext") ?? throw new InvalidOperationException("Connection string 'BookshopContext' not found.")));

// Add services to the container.
builder.Services.AddScoped<IBookingsService, BookingsService>();
builder.Services.AddScoped<IProductsService, ProductsService>();
builder.Services.AddScoped<IStoreService, StoreService>();
builder.Services.AddScoped<IUsersService, UsersService>();


builder.Services.AddControllers().AddJsonOptions(opt =>
{
	opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
}); 

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
	options.AddPolicy(name: bookshopUIAccess,
					  policy =>
					  {
						  policy.AllowAnyOrigin()
						  .AllowAnyHeader()
						  .AllowAnyMethod();
					  });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseCors(bookshopUIAccess);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
