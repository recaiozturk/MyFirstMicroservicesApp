using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyMicroservice.Services.Basket.Dtos;
using MyMicroservice.Services.Basket.Services;
using MyMicroService.Shared.ControllerBases;
using MyMicroService.Shared.Services;

namespace MyMicroservice.Services.Basket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketsController : CustomBaseController
    {
        private readonly IBasketService _basketService;
        private readonly ISharerdIdentityService _sharerdIdentityService;

        public BasketsController(IBasketService basketService, ISharerdIdentityService sharerdIdentityService)
        {
            _basketService = basketService;
            _sharerdIdentityService = sharerdIdentityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBasket()
        {
            return CreateActioNResultInstance(await _basketService.GetBasket(_sharerdIdentityService.GetUserId));
        }

        [HttpPost]
        public async Task<IActionResult> SaveOrUpdateBasket(BasketDto basketDto)
        {
            var response = await _basketService.SaveOrUpdate(basketDto);
            return CreateActioNResultInstance(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBasket()
        {
            return CreateActioNResultInstance(await _basketService.Delete(_sharerdIdentityService.GetUserId));
        }
    }
}
