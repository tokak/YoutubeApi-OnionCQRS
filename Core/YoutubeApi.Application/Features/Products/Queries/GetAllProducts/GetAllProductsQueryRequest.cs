using MediatR;

namespace YoutubeApi.Application.Features.Products.Queries.GetAllProducts
{
    //kullanıcını aldıgımız veriler veya istekler
    public class GetAllProductsQueryRequest : IRequest<IList<GetAllProductsQueryResponse>>
    {

    }
}
