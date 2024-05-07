using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyMicroservice.Discount.Services;
using MyMicroService.Shared.ControllerBases;
using MyMicroService.Shared.Services;

namespace MyMicroservice.Discount.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountController : CustomBaseController
    {
        private readonly IDiscountService _discountService;
        private readonly ISharerdIdentityService _sharerdIdentityService;

        public DiscountController(IDiscountService discountService, ISharerdIdentityService sharerdIdentityService)
        {
            _discountService = discountService;
            _sharerdIdentityService = sharerdIdentityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return CreateActioNResultInstance(await _discountService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var discount =await _discountService.GetById(id);
            return CreateActioNResultInstance(discount);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]/{code}")]
        public async Task<IActionResult> GetByCode(string code)
        {
            var userId = _sharerdIdentityService.GetUserId;
            var discount=await _discountService.GetByCodeAndUserId(code, userId);
            return CreateActioNResultInstance(discount);
        }

        [HttpPost]
        public async Task<IActionResult> Save(Models.Discount discount)
        {
            return CreateActioNResultInstance(await _discountService.Save(discount));
        }

        [HttpPut]
        public async Task<IActionResult> Update(Models.Discount discount)
        {
            return CreateActioNResultInstance(await _discountService.Update(discount));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return CreateActioNResultInstance(await _discountService.Delete(id));
        }

    }
}
