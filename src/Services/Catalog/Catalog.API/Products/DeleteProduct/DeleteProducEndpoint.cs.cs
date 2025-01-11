namespace Catalog.API.Products.DeleteProduct;

public record DeleteProductRequest(Guid Id) : IRequest<DeleteProductResult>;
public record DeleteProductResponse(bool IsSuccess);

public class DeleteProducEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/products/{id}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new DeleteProductCommand(id));

            var response = result.Adapt<DeleteProductResponse>();

            return Results.Ok(response);
        })
        .WithName("DeleteProduct")
        .Produces<DeleteProductResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Deletes a product")
        .WithDescription("Deletes a product");
    }
}