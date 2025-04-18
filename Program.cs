using Microsoft.EntityFrameworkCore;
using System;
using VendingMachineApp.Data;
using VendingMachineApp.Hubs;
using VendingMachineApp.Interfaces;
using VendingMachineApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IDrinkService, DrinkService>();
builder.Services.AddScoped<IBrandService, BrandService>();
builder.Services.AddScoped<ICoinService,CoinService>();
builder.Services.AddScoped<IDrinkValidateService, DrinkValidateService>();

builder.Services.AddDbContext<VendingMachineContext>(options =>

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
    var services = scope.ServiceProvider;
    DbInitializer.Initialize(services);
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
