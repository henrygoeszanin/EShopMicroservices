using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Adiciona os serviços a serem usados
var assembly = typeof(Program).Assembly;

// Configura o MediatR com comportamentos de validação e logging
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

// Adiciona validadores do assembly
builder.Services.AddValidatorsFromAssembly(assembly);

// Adiciona o Carter, um micro-framework para endpoints
builder.Services.AddCarter();

// Configura o Marten, uma biblioteca de armazenamento de dados
builder.Services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();

if(builder.Environment.IsDevelopment())
{
    builder.Services.InitializeMartenWith<CatalogInitialData>();
}

// Adiciona um manipulador de exceções personalizado
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

// Adiciona Health Checks
builder.Services.AddHealthChecks()
    // Adiciona um Health Check para o banco de dados usando AspNetCore.HealthChecks
    .AddNpgSql(builder.Configuration.GetConnectionString("Database")!);

// Constrói o aplicativo
var app = builder.Build();

// Configura a pipeline das requisições HTTP
app.MapCarter();

// Configura o manipulador de exceções
app.UseExceptionHandler(options => {});

// Configura o Health Check para usar a UI da biblioteca HealthChecks.UI
app.UseHealthChecks("/health",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

// Executa o aplicativo
await Task.Run(app.Run);