using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyMicroservice.Shared.Dtos;
using MyMicroService.Shared.ControllerBases;

namespace MyMicroservice.Services.FakePayment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FakePaymentController : CustomBaseController
    {
        [HttpPost]
        public IActionResult ReceivePayment()
        {
            return CreateActioNResultInstance(Response<NoContent>.Success(200));
        }
    }
}
