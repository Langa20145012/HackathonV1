using Hackath_Application_API.Interfaces;
using Hackath_Application_API.Services;
using Hackathon_Application_Database.DatabaseContext;
using Microsoft.EntityFrameworkCore;
//using Ocelot.DependencyInjection;
//using Ocelot.Middleware;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("HackathonConn")));

// Add Redis Cache
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("RedisConnection");
    options.InstanceName = "MatterService_";
});

// Add HttpClient
builder.Services.AddHttpClient<INotificationService, NotificationService>(client => { client.Timeout = TimeSpan.FromSeconds(30); });
builder.Services.AddHttpClient<IDocumentService, Hackath_Application_API.Services.DocumentService>(client => {     client.Timeout = TimeSpan.FromSeconds(30); });
builder.Services.AddHttpClient<IMatterService, MatterService>(client => { client.Timeout = TimeSpan.FromSeconds(30); });

// Register services
builder.Services.AddScoped<IMatterService, MatterService>();
builder.Services.AddScoped<IEmailService, EmailService>(); 
builder.Services.AddScoped<INotificationService, NotificationService>();

// Add Ocelot
//builder.Services.AddOcelot(builder.Configuration);
// Add CORS
builder.Services.AddCors(options => 
{   
    options.AddPolicy("CorsPolicy",         
    builder => builder             
            .AllowAnyOrigin()            
            .AllowAnyMethod()            
            .AllowAnyHeader()); 
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.UseCors("CorsPolicy");

// Use Ocelot middleware
//await app.UseOcelot();

app.Run();
