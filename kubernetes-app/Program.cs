var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/test", async () =>
{
    Random random = new Random();
    int delay = random.Next(0, 1001); // Genera un retraso aleatorio entre 0 y 1000 ms
    await Task.Delay(delay);
    
    string podName = Environment.GetEnvironmentVariable("HOSTNAME") ?? "unknown-pod";
    return Results.Ok($"Ok from pod: {podName} - {Guid.NewGuid()}");})
.WithName("test");

app.Run("http://*:80");