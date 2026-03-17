using BookingModule.Handlers;
using BookingModule.Infrastructure;
using BookingModule.Repositories;
using BookingModule.Services;
using BookingModule.Subscribers;
using BookingNya.Endpoints;
using InventoryModule.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NotificationModule.Infrastructure;
using PaymentModule.Infrastructure;
using Scalar.AspNetCore;
using DotNetCore.CAP;
using InventoryModule.Handlers;
using InventoryModule.Repositories;
using InventoryModule.Services;
using PaymentModule.Repositories;
using PaymentModule.Services;
using PaymentModule.Subscribers;
using Savorboard.CAP.InMemoryMessageQueue;

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
builder.Services.AddScoped<IBookingService,BookingService>();

builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(BookingRequestHandler).Assembly);
});

builder.Services.AddCap(x =>
{
   
    x.UseInMemoryMessageQueue();
    x.UseInMemoryStorage();

});
builder.Services.AddTransient<ReserveRoomSubscriber>();
builder.Services.AddTransient<SagaStatusSubscriber>();
builder.Services.AddTransient<RoomReservedSubscriber>();
builder.Services.AddTransient<PaymentProcessSubscriber>();

builder.Services.AddTransient<PaymentProcessedSubscriber>();
builder.Services.AddScoped<IReserveRoomService,ReserveRoomService>();
builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<Mock>();
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

