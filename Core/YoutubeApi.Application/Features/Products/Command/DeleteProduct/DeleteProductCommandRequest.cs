using MediatR;

namespace YoutubeApi.Application.Features.Products.Command.DeleteProduct
{
    public class DeleteProductCommandRequest:IRequest
    {
        public int Id { get; set; }
    }
}
