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
            var products = await _unitOfWork.GetReadRepository<Product>().GetAllAsync(include: x => x.Include(b => b.Brand));

           var brand = _mapper.Map<BrandDto, Brand>(new Brand());
            var map = _mapper.Map<GetAllProductsQueryResponse, Product>(products);
            foreach (var item in map)
                item.Price -= (item.Price * item.Discount / 100);


            return map;
        }
    }
}
