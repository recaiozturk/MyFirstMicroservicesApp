using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyMicroservice.Order.Application.Commands;
using MyMicroservice.Order.Application.Queries;
using MyMicroService.Shared.ControllerBases;
using MyMicroService.Shared.Services;

namespace MyMicroservice.Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : CustomBaseController
    {
        private readonly IMediator _mediator;
        private readonly ISharerdIdentityService _sharedIdenttityService;

        public OrdersController(ISharerdIdentityService sharedIdenttityService, IMediator mediator)
        {
            _sharedIdenttityService = sharedIdenttityService;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var response = await _mediator.Send(new GetOrdersByUserIdQuery { UserId = _sharedIdenttityService.GetUserId });
            return CreateActioNResultInstance(response);
        }

        [HttpPost]
        public async Task<IActionResult> SaveOrder(CreateOrderCommand createOrderCommand)
        {
            var response = await _mediator.Send(createOrderCommand);
            return CreateActioNResultInstance(response);
        }
    }
}
