using BuildingBlocks.Exceptions.Handler;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Adicionar serviços ao container
var assembly = typeof(Program).Assembly;

// Adiciona Carter para simplificar a criação de endpoints
builder.Services.AddCarter();

// Adiciona MediatR para mediar a comunicação entre componentes
builder.Services.AddMediatR(config => 
{
    // Registra serviços do assembly atual
    config.RegisterServicesFromAssembly(assembly);
    // Adiciona comportamento de validação
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    // Adiciona comportamento de logging
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

// Configura Marten como o provedor de banco de dados
builder.Services.AddMarten(options =>
{
    // Define a string de conexão do banco de dados
    options.Connection(builder.Configuration.GetConnectionString("Database")!);
    // Define a identidade do schema para ShoppingCart
    options.Schema.For<ShoppingCart>().Identity(x => x.UserName);
}).UseLightweightSessions();

// Adiciona repositórios ao container de injeção de dependência
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();

// Configura o cache Redis
builder.Services.AddStackExchangeRedisCache(options =>
{
    // Define a string de conexão do Redis
    options.Configuration = builder.Configuration.GetConnectionString("Redis")!;
});

// Adiciona o manipulador de exceções customizado
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddHealthChecks()
    .AddRedis(builder.Configuration.GetConnectionString("Redis")!)
    .AddNpgSql(builder.Configuration.GetConnectionString("Database")!);

var app = builder.Build();

// Configura o pipeline para requisições HTTP
app.MapCarter();

// Configura health checks
app.UseHealthChecks("/health",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

// Configura o middleware de tratamento de exceções
app.UseExceptionHandler(options =>{ });

// Inicia a aplicação
await Task.Run(app.Run);