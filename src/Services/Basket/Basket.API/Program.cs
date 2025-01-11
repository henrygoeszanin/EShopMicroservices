using BuildingBlocks.Exceptions.Handler;

var builder = WebApplication.CreateBuilder(args);

// adicionar servicos ao container
var assembly = typeof(Program).Assembly;
builder.Services.AddCarter();
builder.Services.AddMediatR(config => 
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetConnectionString("Database")!);
    options.Schema.For<ShoppingCart>().Identity(x => x.UserName);
}).UseLightweightSessions();

builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.AddScoped<IBasketRepository, CachedBasketRepository>();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis")!;
});

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();

//configura o pipeline para requisiçaõ HTP request pipeline
app.MapCarter();

app.UseExceptionHandler(options =>{ });

await Task.Run(app.Run);
