using System;
using System.Collections.Generic;

namespace LLP.EntityDesign.API.Features.Orders
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string OrderNo { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateCompleted { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public decimal GrandTotal { get; set; }
        public string GrandTotalNormalized { get; set; }

        public List<OrderItemDto> Items { get; set; }
    }
}
