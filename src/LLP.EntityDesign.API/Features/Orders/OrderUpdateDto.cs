using System;
using System.Collections.Generic;

namespace LLP.EntityDesign.API.Features.Orders
{
    public class OrderUpdateDto
    {
        public int Id { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
    }
}
