using LLP.EntityDesign.API.Features.Orders;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LLP.EntityDesign.API.Contracts
{
    public interface IOrderService
    {
        Task<List<OrderDto>> GetAsync(CancellationToken cancellationToken);
        Task<OrderDto> GetAsync(int orderId, CancellationToken cancellationToken);
        Task<OrderDto> AddAsync(OrderCreateDto orderCreateDto, CancellationToken cancellationToken);
        Task<OrderDto> UpdateAsync(OrderUpdateDto orderUpdateDto, CancellationToken cancellationToken);
        Task DeleteAsync(int orderId, CancellationToken cancellationToken);
    }
}
