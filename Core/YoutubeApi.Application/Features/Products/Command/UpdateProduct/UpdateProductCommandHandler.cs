using MediatR;
using YoutubeApi.Application.Interfaces.AutoMapper;
using YoutubeApi.Application.Interfaces.UnitOfWorks;
using YoutubeApi.Domain.Entities;

namespace YoutubeApi.Application.Features.Products.Command.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommandRequest,Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UpdateProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Unit> Handle(UpdateProductCommandRequest request, CancellationToken cancellationToken)
        {
            // İlgili ürünü veritabanından getir (silinmemiş olan)
            var product = await _unitOfWork.GetReadRepository<Product>().GetAsync(x => x.Id == request.Id && !x.IsDeleted);

            // Gelen request verilerini ürün nesnesine eşleştir (maple)
            var map = _mapper.Map<Product, UpdateProductCommandRequest>(request);

            // Ürüne ait mevcut kategorileri getir
            var productCategories = await _unitOfWork.GetReadRepository<ProductCategory>().GetAllAsync(x => x.ProductId == product.Id);

            // Mevcut kategori ilişkilerini sil
            await _unitOfWork.GetWriteRepository<ProductCategory>().HardDeleteRangeAsync(productCategories);

            // Yeni kategorileri ekle
            foreach (var categoryId in request.CategoryIds)
                await _unitOfWork.GetWriteRepository<ProductCategory>().AddAsync(new ProductCategory() { CategoryId = categoryId, ProductId = product.Id });

            // Ürün bilgisini güncelle
            await _unitOfWork.GetWriteRepository<Product>().UpdateAsync(map );

            // Tüm değişiklikleri kaydet
            await _unitOfWork.SaveAsync();

            return Unit.Value;
        }

    }
}
