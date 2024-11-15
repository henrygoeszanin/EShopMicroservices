namespace Catalog.API.Products.GetProducts;

// Define um record para a requisição de produtos, com parâmetros opcionais para número da página e tamanho da página (padrão é 10)
public record GetProductsRequest(int? PageNumber, int? PageSize = 10);

// Define um record para a resposta da requisição de produtos, contendo uma lista de produtos
public record GetProductsResponse(IEnumerable<Product> Products);

// Define a classe GetProductsEndpoint que implementa a interface ICarterModule
public class GetProductsEndpoint : ICarterModule
{
    // Método para adicionar rotas ao aplicativo
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        // Mapeia a rota GET "/products" para um manipulador assíncrono
        app.MapGet("/products", async ([AsParameters] GetProductsRequest request, ISender sender) =>
        {
            // Adapta a requisição para um objeto GetProductsQuery
            var query = request.Adapt<GetProductsQuery>();

            // Envia a query usando o sender e aguarda o resultado
            var result = await sender.Send(query);

            // Adapta o resultado para um objeto GetProductsResponse
            var response = result.Adapt<GetProductsResponse>();

            // Retorna a resposta com status 200 OK
            return Results.Ok(response);
        })
        .WithName("GetProducts") // Define o nome da rota
        .Produces<GetProductsResponse>(StatusCodes.Status200OK) // Define o tipo de resposta produzida e o status 200 OK
        .WithSummary("Get Products") // Adiciona um resumo à rota
        .WithDescription("Get Products"); // Adiciona uma descrição à rota
    }
}