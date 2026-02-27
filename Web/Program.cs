using BookingModule.Infrastructure;
using BookingNya.Endpoints;
using InventoryModule.Infrastructure;
using Microsoft.EntityFrameworkCore;
using NotificationModule.Infrastructure;
using PaymentModule.Infrastructure;
using Scalar.AspNetCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<BookingDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("BookingDBConnection"),
        b => b.MigrationsAssembly(typeof(BookingDbContext).Assembly.FullName)));
builder.Services.AddDbContext<InventoryDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("BookingDBConnection"),
        b => b.MigrationsAssembly(typeof(InventoryDbContext).Assembly.FullName)));

builder.Services.AddDbContext<NotificationDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("BookingDBConnection"),
        b => b.MigrationsAssembly(typeof(NotificationDbContext).Assembly.FullName)));

builder.Services.AddDbContext<PaymentDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("BookingDBConnection"),
        b => b.MigrationsAssembly(typeof(PaymentDbContext).Assembly.FullName)));
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference("/api-docs");
}

app.UseHttpsRedirection();
app.MapSagaEndpont();


app.Run();

