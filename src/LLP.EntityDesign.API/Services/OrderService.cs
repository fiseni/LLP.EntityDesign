using LLP.EntityDesign.API.Contracts;
using LLP.EntityDesign.API.Data;
using LLP.EntityDesign.API.Data.Orders;
using LLP.EntityDesign.API.Data.Orders.Seeds;
using LLP.EntityDesign.API.Features.Orders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LLP.EntityDesign.API.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext dbContext;
        private readonly IDateTime dateTimeService;
        private readonly IDocumentNoGenerator documentNoGenerator;

        public OrderService(AppDbContext dbContext,
                            IDateTime dateTimeService,
                            IDocumentNoGenerator documentNoGenerator)
        {
            this.dbContext = dbContext;
            this.dateTimeService = dateTimeService;
            this.documentNoGenerator = documentNoGenerator;
        }

        public async Task<List<OrderDto>> GetAsync(CancellationToken cancellationToken)
        {
            var orders = await dbContext.Orders.Include(x => x.Items).ToListAsync(cancellationToken);

            var ordersDto = orders.Select(x => MapOrderToOrderDto(x));

            return ordersDto.ToList();
        }

        public async Task<OrderDto> GetAsync(int orderId, CancellationToken cancellationToken)
        {
            var order = await dbContext.Orders.Where(x=>x.Id == orderId).Include(x => x.Items).SingleOrDefaultAsync(cancellationToken);

            _ = order ?? throw new KeyNotFoundException($"The order with Id: {orderId} is not found!");

            var orderDto = MapOrderToOrderDto(order);

            return orderDto;
        }

        public async Task<OrderDto> AddAsync(OrderCreateDto orderCreateDto, CancellationToken cancellationToken)
        {
            _ = orderCreateDto ?? throw new ArgumentNullException(nameof(orderCreateDto));

            var orderNo = await documentNoGenerator.GetNewOrderNo();

            var customer = new Customer(orderCreateDto.FirstName, orderCreateDto.LastName, orderCreateDto.Email);
            var address = new Address(orderCreateDto.Street, orderCreateDto.City, orderCreateDto.PostalCode, orderCreateDto.Country);
            var order = new Order(dateTimeService, orderNo, customer, address);

            foreach (var item in orderCreateDto.Items)
            {
                order.AddItem(new OrderItem(item.Name, item.Price));
            }

            await dbContext.Orders.AddAsync(order);

            await dbContext.SaveChangesAsync(cancellationToken);

            return MapOrderToOrderDto(order);
        }

        public async Task<OrderDto> UpdateAsync(OrderUpdateDto orderUpdateDto, CancellationToken cancellationToken)
        {
            _ = orderUpdateDto ?? throw new ArgumentNullException(nameof(orderUpdateDto));

            var order = await dbContext.Orders.Where(x => x.Id == orderUpdateDto.Id).Include(x => x.Items).SingleOrDefaultAsync(cancellationToken);

            _ = order ?? throw new KeyNotFoundException($"The order with Id: {orderUpdateDto.Id} is not found!");

            var address = new Address(orderUpdateDto.Street, orderUpdateDto.City, orderUpdateDto.PostalCode, orderUpdateDto.Country);
            
            order.UpdateAddress(address);

            await dbContext.SaveChangesAsync(cancellationToken);

            return MapOrderToOrderDto(order);
        }

        public async Task DeleteAsync(int orderId, CancellationToken cancellationToken)
        {
            var order = await dbContext.Orders.FindAsync(orderId);

            _ = order ?? throw new KeyNotFoundException($"The order with Id: {orderId} is not found!");

            dbContext.Orders.Remove(order);

            await dbContext.SaveChangesAsync(cancellationToken);
        }

        private static OrderDto MapOrderToOrderDto(Order order)
        {
            return new OrderDto
            {
                Id = order.Id,
                OrderNo = order.OrderNo,
                DateCreated = order.DateCreated,
                DateCompleted = order.DateCompleted,
                FirstName = order.Customer.FirstName,
                LastName = order.Customer.LastName,
                Email = order.Customer.Email,
                Street = order.Address.Street,
                City = order.Address.City,
                PostalCode = order.Address.PostalCode,
                Country = order.Address.Country,
                GrandTotal = order.GrandTotal,
                GrandTotalNormalized = order.GrandTotalNormalized,
                Items = order.Items.Select(orderItem => new OrderItemDto
                {
                    Id = orderItem.Id,
                    Name = orderItem.Name,
                    Price = orderItem.Price
                }).ToList()
            };
        }
    }
}
