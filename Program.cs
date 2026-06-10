using MongoDB.Driver;
using appsy.src.service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.WebHost.UseUrls("http://localhost:5000");

// MongoDB
var connectionString = builder.Configuration["MongoDb:ConnectionString"];
var databaseName = builder.Configuration["MongoDb:DatabaseName"];

builder.Services.AddSingleton<IMongoClient>(
    new MongoClient(connectionString)
);

builder.Services.AddSingleton<IMongoDatabase>(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase(databaseName);
});

builder.Services.AddScoped<AuthService>();

//  app
var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

// Endpoint de prueba
app.MapGet("/test-db", async (IMongoDatabase database) =>
{
    var collections = await database.ListCollectionNames().ToListAsync();
    return Results.Ok(collections);
});

app.Run();