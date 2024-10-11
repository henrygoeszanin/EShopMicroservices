namespace Catalog.API.Products.CreateProduct;

// Record que define a estrutura da requisição para criar um produto
public record CreateProductRequest(string Name, List<string> Category, string Description, string ImageFile, decimal Price);

// Record que define a estrutura da resposta após a criação do produto
public record CreateProductResponse(Guid Id);

public class CreateProductEndpoint : ICarterModule
{
    // Método para adicionar rotas ao endpoint
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        // Define uma rota HTTP POST para criar um novo produto
        app.MapPost("/products", async (CreateProductRequest request, ISender sender) =>
        {
            // Adapta a requisição (request) para o comando CreateProductCommand
            var command = request.Adapt<CreateProductCommand>();

            // Envia o comando usando o ISender (MediatR ou similar) e aguarda o resultado
            var result = await sender.Send(command);

            // Adapta o resultado do comando para a resposta CreateProductResponse
            var response = result.Adapt<CreateProductResponse>();

            // Retorna o status HTTP 201 Created com a URL do novo produto e o conteúdo da resposta
            return Results.Created($"/products/{response.Id}", response);

        })
            // Define o nome da rota para facilitar a referência em outros lugares
            .WithName("CreateProduct")

            // Indica que a rota pode retornar um CreateProductResponse com status 201 Created
            .Produces<CreateProductResponse>(StatusCodes.Status201Created)

            // Indica que a rota também pode retornar um problema (400 Bad Request)
            .ProducesProblem(StatusCodes.Status400BadRequest)

            // Adiciona um resumo para descrever brevemente o que o endpoint faz
            .WithSummary("Create Product")

            // Adiciona uma descrição mais detalhada sobre o que o endpoint faz
            .WithDescription("Create Product");
    }
}
