using MediatR;
using YoutubeApi.Application.Features.Products.Rules;
using YoutubeApi.Application.Interfaces.UnitOfWorks;
using YoutubeApi.Domain.Entities;

namespace YoutubeApi.Application.Features.Products.Command.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommandRequest,Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ProductRules _productRules;

        public CreateProductCommandHandler(IUnitOfWork unitOfWork,ProductRules productRules)
        {
            _unitOfWork = unitOfWork;
            _productRules = productRules;
        }
        public async Task<Unit> Handle(CreateProductCommandRequest request,CancellationToken cancellationToken)
        {
            IList<Product> products = await _unitOfWork.GetReadRepository<Product>().GetAllAsync();

            //if (products.Any(x => x.Title == request.Title))
            //    throw new Exception("Aynı başlıktan ürün olamaz");

            await _productRules.ProductTitleMustNotBeSame(products,request.Title);           

            Product product = new()
            {
                Title = request.Title,
                Description = request.Description,
                BrandId = request.BrandId,
                Price = request.Price,
                Discount = request.Discount
            };
            await _unitOfWork.GetWriteRepository<Product>().AddAsync(product);
            if (await _unitOfWork.SaveAsync() > 0)
            {
                foreach (var categoryId in request.CategoryIds)
                    await _unitOfWork.GetWriteRepository<ProductCategory>().AddAsync(new()
                    {
                        ProductId = product.Id,
                        CategoryId = categoryId
                    });

                await _unitOfWork.SaveAsync();
            }

            return Unit.Value;
        }
    }
}
