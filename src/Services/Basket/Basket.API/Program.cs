var builder = WebApplication.CreateBuilder(args);

// adicionar servicos ao container

var app = builder.Build();

//configura o pipeline para requisiçaõ HTT
app.MapGet("/", () => "Hello World!");

app.Run();
