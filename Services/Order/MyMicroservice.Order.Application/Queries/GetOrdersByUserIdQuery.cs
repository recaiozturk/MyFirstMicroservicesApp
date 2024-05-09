using MediatR;
using MyMicroservice.Order.Application.DTOs;
using MyMicroservice.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMicroservice.Order.Application.Queries
{
    class GetOrdersByUserIdQuery:IRequest<Response<List<OrderDto>>>
    {
        public string UserId { get; set; }
    }
}
