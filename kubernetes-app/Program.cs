using System.Numerics; // Necesario para BigInteger

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
    return Results.Ok(podName);
})
.WithName("test");

app.MapGet("/fibonacci", () =>
{
    int random = new Random().Next(1, 51); // Genera un número aleatorio entre 1 y 1000
    // Simular un trabajo intensivo de CPU calculando un número grande de Fibonacci

    int n = random; // Puedes ajustar este valor para controlar la intensidad del cálculo
    BigInteger fibonacciResult = CalculateFibonacci(n);

    string podName = Environment.GetEnvironmentVariable("HOSTNAME") ?? "unknown-pod";
    return Results.Ok($"{podName} - Fibonacci({n}) = {fibonacciResult}");
})
.WithName("fibonacci");

// Función para calcular el número de Fibonacci de forma recursiva (intensivo en CPU)
static BigInteger CalculateFibonacci(int n)
{
    if (n <= 1)
        return n;
    return CalculateFibonacci(n - 1) + CalculateFibonacci(n - 2);
}

app.Run("http://*:80");