using MongoDB.Driver;
using appsy.src.service;
using appsy.src.repository;
using appsy.src.config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.WebHost.UseUrls("http://localhost:5000");

// --- JWT AUTHENTICATION CONFIGURATION ---
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!); 

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"JWT Authentication Failed: {context.Exception.Message}");
                return Task.CompletedTask;
            },
            OnChallenge = context =>
            {
                Console.WriteLine($"JWT Challenge Triggered: {context.Error}, {context.ErrorDescription}");
                return Task.CompletedTask;
            }
        };
    });

// --- MONGODB CONFIGURATION ---
var connectionString = builder.Configuration["MongoDb:ConnectionString"];
var databaseName = builder.Configuration["MongoDb:DatabaseName"];

// Fixed MongoDB setup (Adds safety to prevent nulls)
builder.Services.AddSingleton<IMongoClient>(sp => new MongoClient(connectionString));
builder.Services.AddScoped(sp => 
{
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase(databaseName!);
});

// --- SERVICES (The FIX for your current error) ---
// 1. Register the Repository
builder.Services.AddScoped<AuthRepository>();

// 2. Register JwtService (This was missing and causing your crash!)
builder.Services.AddScoped<JwtService>();

// 3. Register AuthService (Which relies on BOTH AuthRepository and JwtService)
builder.Services.AddScoped<AuthService>();

// --- CORS CONFIGURATION ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// --- JWT OPTIONS CONFIG ---
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

var app = builder.Build();

// --- MIDDLEWARE ORDER ---
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();

// app.UseHttpsRedirection();

app.MapControllers();

app.Run();