using MediatR;
using YoutubeApi.Application.Interfaces.UnitOfWorks;
using YoutubeApi.Domain.Entities;

namespace YoutubeApi.Application.Features.Products.Queries.GetAllProducts
{
    //aradaki işlemleri burda yapıyoruz requesti alıyoruz işlem gerçekleşiyor respons ile kullanıcıya geri dönderiyoruz işlem başarılı oldugunda 200 de dönebilirsin ikinci parametre olmadan 
    //ikinci paremetre eklersen paremetreyi dönersin
    public class GetAllProductsHandler : IRequestHandler<GetAllProductsQueryRequest, IList<GetAllProductsQueryResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllProductsHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IList<GetAllProductsQueryResponse>> Handle(GetAllProductsQueryRequest request, CancellationToken cancellationToken)
        {
            // Veritabanındaki Product tablosunu temsil eden repository'yi kullanarak  tüm ürünleri  liste olarak çekiyoruz
            var products = await _unitOfWork.GetReadRepository<Product>().GetAllAsync();

            // Dışarıya göndereceğimiz cevap listesi için boş bir liste oluşturuyoruz
            List<GetAllProductsQueryResponse> response = new();

            // Veritabanından gelen her bir ürün için dönüyoruz
            foreach (var product in products)
                response.Add(new GetAllProductsQueryResponse
                {
                    Title = product.Title,
                    Description = product.Description,
                    Discount = product.Discount,
                    Price = product.Price - (product.Price * product.Discount / 100),
                });

            // response listesini döndürüyoruz
            return response;
        }
    }
}
