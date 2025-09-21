using MediatR;
using Microsoft.EntityFrameworkCore;
using YoutubeApi.Application.DTOs;
using YoutubeApi.Application.Interfaces.AutoMapper;
using YoutubeApi.Application.Interfaces.UnitOfWorks;
using YoutubeApi.Domain.Entities;

namespace YoutubeApi.Application.Features.Products.Queries.GetAllProducts
{
    //aradaki işlemleri burda yapıyoruz requesti alıyoruz işlem gerçekleşiyor respons ile kullanıcıya geri dönderiyoruz işlem başarılı oldugunda 200 de dönebilirsin ikinci parametre olmadan 
    //ikinci paremetre eklersen paremetreyi dönersin
    public class GetAllProductsHandler : IRequestHandler<GetAllProductsQueryRequest, IList<GetAllProductsQueryResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllProductsHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IList<GetAllProductsQueryResponse>> Handle(GetAllProductsQueryRequest request, CancellationToken cancellationToken)
        {
            // Veritabanındaki Product tablosunu temsil eden repository'yi kullanarak  tüm ürünleri  liste olarak çekiyoruz
            var products = await _unitOfWork.GetReadRepository<Product>().GetAllAsync(include:x=>x.Include(b=>b.Brand));

            _mapper.Map<BrandDto,Brand>(new Brand());
            // Dışarıya göndereceğimiz cevap listesi için boş bir liste oluşturuyoruz
            //List<GetAllProductsQueryResponse> response = new();

            // Veritabanından gelen her bir ürün için dönüyoruz
            //foreach (var product in products)
            //    response.Add(new GetAllProductsQueryResponse
            //    {
            //        Title = product.Title,
            //        Description = product.Description,
            //        Discount = product.Discount,
            //        Price = product.Price - (product.Price * product.Discount / 100),
            //    });
            var map = _mapper.Map<GetAllProductsQueryResponse, Product>(products);
            foreach (var item in map)
                item.Price -= (item.Price * item.Discount / 100);

            // response listesini döndürüyoruz
            throw new Exception("Hata mesajı");
        }
    }
}
