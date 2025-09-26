using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YoutubeApi.Application.Features.Products.Command.CreateProduct;
using YoutubeApi.Application.Features.Products.Command.DeleteProduct;
using YoutubeApi.Application.Features.Products.Command.UpdateProduct;
using YoutubeApi.Application.Features.Products.Queries.GetAllProducts;

namespace YoutubeApi.WepApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllProducts()
        {
            var response = await _mediator.Send(new GetAllProductsQueryRequest());
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductCommandRequest request)
        {
            await _mediator.Send(request);
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> UpdateProduct(UpdateProductCommandRequest request)
        {
            await _mediator.Send(request);
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> DeleteProduct(DeleteProductCommandRequest request)
        {
            await _mediator.Send(request);
            return Ok();
        }
    }
}
