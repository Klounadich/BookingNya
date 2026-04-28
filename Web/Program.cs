using System.Text;
using AuthModule.Commands;
using AuthModule.Handlers;
using AuthModule.Infrastructure;
using AuthModule.Repositories;
using AuthModule.Services;
using BookingModule.Commands;
using BookingModule.Handlers;
using BookingModule.Infrastructure;
using BookingModule.Repositories;
using BookingModule.Services;
using BookingModule.Subscribers;
using BookingNya.Endpoints;
using BookingNya.Validators;
using InventoryModule.Infrastructure;
using Microsoft.EntityFrameworkCore;
using NotificationModule.Infrastructure;
using PaymentModule.Infrastructure;
using Scalar.AspNetCore;
using FluentValidation;
using InventoryModule.Handlers;
using InventoryModule.Repositories;
using InventoryModule.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using NotificationModule.Repositories;
using NotificationModule.Services;
using NotificationModule.SMTP.Models;
using NotificationModule.SMTP.Services;
using NotificationModule.Subscribers;
using Npgsql;
using PaymentModule.Repositories;
using PaymentModule.Services;
using PaymentModule.Subscribers;
using Savorboard.CAP.InMemoryMessageQueue;
using Shared.Other;

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

builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseNpgsql(dataSource, b => b.MigrationsAssembly(typeof(AuthDbContext).Assembly.FullName)));

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
builder.Services.AddScoped<IValidator<BookingRequestCommand>, BookingRequestValidator>();
builder.Services.AddScoped<IValidator<RegisterRequestCommand>, RegisterRequestValidator>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<ISagaOrchestrator, SagaOrchestrator>();


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
builder.Services.AddTransient<IMailService, MailService>();
//Singletones:
builder.Services.AddSingleton<IRequestTracker, RequestTracker>();
builder.Services.AddSingleton<IEmailTemplateLoader, EmailTemplateLoader>();
//Others:
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(BookingRequestHandler).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(AuthHandler).Assembly);
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

// JWT Settings -------------------------------------------------------------------
builder.Services.Configure<JWTService.AuthSettings>(
    builder.Configuration.GetSection("jwt")
);
builder.Services.AddScoped<JWTService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(i =>
    {
        i.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwt:SecretKey"]))
        };
        i.Events= new JwtBearerEvents { 
            OnMessageReceived = context =>
            {
                context.Token = context.Request.Cookies["auth_token"];
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder().AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme).RequireAuthenticatedUser().Build();
});


//-----------------------------------------------------------------------------
builder.Services.Configure<MailSettings>(
    builder.Configuration.GetSection("MailSettings")
);

var app = builder.Build();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapHub<Shared.SignalR.SagaProcessHub>("/saga-process-hub");
app.UseStaticFiles();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference("/api-docs");
}
app.UseHttpsRedirection();
app.MapSagaEndpont();
app.MapBookingEndpoint();
app.MapAuthEndpoint();


app.Run();

