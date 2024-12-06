
namespace Basket.API.Basket.DeleteBasket;

//public record DeleteBasketRequest(string UserName);

public record DekleteBasketresponse(bool IsSuccess);
public class DeleteBasketRequest : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/basket/{userName}", async (string userName, ISender sender) =>
        {
            var result = await sender.Send(new DeleteBasketCommand(userName));

            var response = result.Adapt<DekleteBasketresponse>();

            return Results.Ok(response);
        })
        .WithName("DeleteProduuct")
        .Produces<DekleteBasketresponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Delete a basket")
        .WithDescription("Delete a basket by user name");
    }
}

