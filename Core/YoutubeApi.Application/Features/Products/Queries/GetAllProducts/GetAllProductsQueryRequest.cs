using MediatR;
using YoutubeApi.Application.Interfaces.RedisCache;

namespace YoutubeApi.Application.Features.Products.Queries.GetAllProducts
{
    //kullanıcını aldıgımız veriler veya istekler
    public class GetAllProductsQueryRequest : IRequest<IList<GetAllProductsQueryResponse>>, ICacheableQuery
    {
        public string CacheKey => "GetAllProducts";

        public double CacheTime => 60;
    }
}
