using Microsoft.EntityFrameworkCore;
using LumberStoreSystem.DataAccess;
using Microsoft.AspNetCore.Connections;
using LumberStoreSystem.DataAccess.Repository;
using LumberStoreSystem.BussinessLogic.Services;
using Microsoft.Extensions.Configuration;
using LumberStoreSystem.BussinessLogic.Interfaces;
using LumberStoreSystem.DataAccess.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<LumberStoreSystemDbContext>(options =>
{
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 27))
    );
});

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<IDimensionsRepository, DimensionsRepository>();
builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<ICuttingListItemRepository, CuttingListItemRepository>();

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IDimensionsService, DimensionsService>();
builder.Services.AddScoped<IOrderItemService, OrderItemService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ICuttingListItemService, CuttingListItemService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var contextFactory = new LumberStoreSystemContextFactory();
string connectionString = "server=localhost;database=lumberstoresystem;uid=root;pwd=root;Old Guids=true";
var context = contextFactory.CreateDbContext(new string[] { connectionString });


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
