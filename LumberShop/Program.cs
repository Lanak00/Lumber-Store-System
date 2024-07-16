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

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IClientService, ClientService>();

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
