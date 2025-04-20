using Microsoft.EntityFrameworkCore;
using VendingMachineApp.Application.Services;
using VendingMachineApp.Core.Interfaces;
using VendingMachineApp.Hubs;
using VendingMachineApp.Infrastructure.Persistence;
using VendingMachineApp.Infrastructure.Repositories;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IBrandRepository, BrandRepository>();
builder.Services.AddScoped<IDrinkRepository, DrinkRepository>();
builder.Services.AddScoped<ICoinRepository, CoinRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

builder.Services.AddScoped<BrandService>();
builder.Services.AddScoped<DrinkService>();
builder.Services.AddScoped<DrinkValidateService>();
builder.Services.AddScoped<CoinService>();
builder.Services.AddScoped<OrderService>();

builder.Services.AddDbContext<InventoryContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddDbContext<OrderContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFront", policy =>
    {
        policy
        .AllowAnyOrigin()    
            .AllowAnyMethod()    
            .AllowAnyHeader();
    });
});

builder.Services.AddSignalR();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var inventory = scope.ServiceProvider;
    DbInitializer.Initialize(inventory);
    scope.ServiceProvider.GetRequiredService<OrderContext>().Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowFront");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHub<VendingMachineHub>("/vendingMachineHub");

app.Run();
