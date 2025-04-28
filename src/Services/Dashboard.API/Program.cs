using Dashboard.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

//builder.Services.AddSwaggerGen();

// Configure JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]!))
        };
    });

builder.Services.AddHttpContextAccessor(); // Needed to access current request

builder.Services.AddTransient<AuthHeaderHandler>(); // Register the handler

// Register HttpClient for microservices communication
builder.Services.AddHttpClient("TransactionService", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ServiceUrls:TransactionApi"]!);
}).AddHttpMessageHandler<AuthHeaderHandler>(); ;

builder.Services.AddHttpClient("CategoryService", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ServiceUrls:CategoryApi"]!);
}).AddHttpMessageHandler<AuthHeaderHandler>(); ;

// Register services
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddCors(o => o.AddPolicy("opencors", builder =>
{
    builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
}));


var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUi(options =>
    {
        options.DocumentPath = "/openapi/v1.json";
    });
}
// app.UseHttpsRedirection();
app.UseCors("opencors");

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();