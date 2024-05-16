using Microsoft.EntityFrameworkCore;
using LumberStoreSystem.DataAccess;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<LumberStoreSystemDbContext>();

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
