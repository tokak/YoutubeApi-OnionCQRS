﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using YoutubeApi.Application.Features.Auth.Command.Register;

namespace YoutubeApi.WepApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator mediator;

        public AuthController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterCommandRequest request)
        {
            await mediator.Send(request);
            return StatusCode(StatusCodes.Status201Created);
        }
    }
}
