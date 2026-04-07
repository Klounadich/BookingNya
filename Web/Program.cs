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
using NotificationModule.Repositories;
using NotificationModule.Services;
using NotificationModule.Subscribers;
using Npgsql;
using PaymentModule.Repositories;
using PaymentModule.Services;
using PaymentModule.Subscribers;
using Savorboard.CAP.InMemoryMessageQueue;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();
// DB CONTEXT--------------------------------------------------------------
var dataSourceBuilder = new NpgsqlDataSourceBuilder(builder.Configuration.GetConnectionString("BookingDBConnection"));
dataSourceBuilder.EnableDynamicJson();
var dataSource = dataSourceBuilder.Build();

builder.Services.AddDbContext<BookingDbContext>(options =>
    options.UseNpgsql(dataSource, b => b.MigrationsAssembly(typeof(BookingDbContext).Assembly.FullName)));

builder.Services.AddDbContext<InventoryDbContext>(options =>
    options.UseNpgsql(dataSource, b => b.MigrationsAssembly(typeof(InventoryDbContext).Assembly.FullName)));

builder.Services.AddDbContext<NotificationDbContext>(options =>
    options.UseNpgsql(dataSource, b => b.MigrationsAssembly(typeof(NotificationDbContext).Assembly.FullName)));

builder.Services.AddDbContext<PaymentDbContext>(options =>
    options.UseNpgsql(dataSource, b => b.MigrationsAssembly(typeof(PaymentDbContext).Assembly.FullName)));
//--------------------------------------------------------------

//SERVICES ------------------------------------------------------------------
//scopeds:
builder.Services.AddScoped<IBookingService,BookingService>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IReserveRoomService,ReserveRoomService>();
builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<Mock>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
//trasients:
builder.Services.AddTransient<ReserveRoomSubscriber>();
builder.Services.AddTransient<SagaStatusSubscriber>();
builder.Services.AddTransient<RoomReservedSubscriber>();
builder.Services.AddTransient<PaymentProcessSubscriber>();
builder.Services.AddTransient<PaymentProcessedSubscriber>();
builder.Services.AddTransient<ConfirmationSubscriber>();
builder.Services.AddTransient<NotificationSentSubscriber>();
builder.Services.AddTransient<NotificationConfirmSubscriber>();
builder.Services.AddTransient<FreeRoomsSubscriber>();
builder.Services.AddTransient<CheckRoomSubscriber>();
//Singletones:
builder.Services.AddSingleton<IRequestTracker, RequestTracker>();
//Others:
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(BookingRequestHandler).Assembly);
});
builder.Services.AddCap(x =>
{
   
    x.UseInMemoryMessageQueue();
    x.UseInMemoryStorage();

});
builder.Services.AddSignalR();
// ------------------------------------------------------------------------------------
// CORS Settings --------------------------------------------------------------------------------
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        
        policy.WithOrigins("http://localhost:5255") 
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); 
    });
});
//  --------------------------------------------------------------------------------
var app = builder.Build();
app.UseCors();
app.MapHub<Shared.SignalR.SagaProcessHub>("/saga-process-hub");
app.UseStaticFiles();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference("/api-docs");
}
app.UseHttpsRedirection();
app.MapSagaEndpont();
app.MapBookingEndpont();


app.Run();

