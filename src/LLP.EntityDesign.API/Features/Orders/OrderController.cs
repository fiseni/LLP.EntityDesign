using LLP.EntityDesign.API.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LLP.EntityDesign.API.Features.Orders
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService orderService;

        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            var result = await orderService.GetAsync(id, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<OrderDto>> AddAsync(OrderCreateDto orderCreateDto, CancellationToken cancellationToken)
        {
            if (orderCreateDto is null)
            {
                return BadRequest();
            }

            var result = await orderService.AddAsync(orderCreateDto, cancellationToken);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<OrderDto>> UpdateAsync(int id, OrderUpdateDto orderUpdateDto, CancellationToken cancellationToken)
        {
            if (orderUpdateDto?.Id != id)
            {
                return BadRequest();
            }

            var result = await orderService.UpdateAsync(orderUpdateDto, cancellationToken);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            await orderService.DeleteAsync(id, cancellationToken);
            return Ok();
        }
    }
}
