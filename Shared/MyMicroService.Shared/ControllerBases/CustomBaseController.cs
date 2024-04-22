using Microsoft.AspNetCore.Mvc;
using MyMicroservice.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyMicroService.Shared.ControllerBases
{
    public class CustomBaseController:ControllerBase
    {
        public IActionResult CreateActioNResultInstance<T>(Response<T> response)
        {
            return new ObjectResult(response)
            {
                StatusCode = response.StatusCode
            };
        }
    }
}
