var builder = WebApplication.CreateBuilder(args);

// Registrar Controllers
builder.Services.AddControllers();
builder.WebHost.UseUrls("http://localhost:5000");

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

// Mapear Controllers
app.MapControllers();
app.Run();