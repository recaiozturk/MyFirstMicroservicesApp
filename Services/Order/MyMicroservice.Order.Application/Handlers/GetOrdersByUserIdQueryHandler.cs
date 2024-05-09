
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyMicroservice.Order.Application.DTOs;
using MyMicroservice.Order.Application.Mapping;
using MyMicroservice.Order.Application.Queries;
using MyMicroservice.Order.Infrastructure;
using MyMicroservice.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMicroservice.Order.Application.Handlers
{
    class GetOrdersByUserIdQueryHandler : IRequestHandler<GetOrdersByUserIdQuery, Response<List<OrderDto>>>
    {
        private readonly OrderDbContext _context;

        public GetOrdersByUserIdQueryHandler(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<Response<List<OrderDto>>> Handle(GetOrdersByUserIdQuery request, CancellationToken cancellationToken)
        {
            var orders=await _context.Orders.Include(x=>x.OrderItems).Where(x=>x.BuyerId==request.UserId).ToListAsync();
            if (!orders.Any())
            {
                return Response<List<OrderDto>>.Success(new List<OrderDto>(), 200);
            }
            var ordersDto = ObjectMapper.Mapper.Map<List<OrderDto>>(orders);
            return Response<List<OrderDto>>.Success(ordersDto, 200);
        }
    }
}
