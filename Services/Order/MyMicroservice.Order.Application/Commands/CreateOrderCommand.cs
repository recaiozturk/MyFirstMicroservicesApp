using MediatR;
using MyMicroservice.Order.Application.DTOs;
using MyMicroservice.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMicroservice.Order.Application.Commands
{
    public class CreateOrderCommand:IRequest<Response<CreatedOrderDto>>
    {
        public string BuyerId { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }

        public AddressDto Address  { get; set; }
    }
}
