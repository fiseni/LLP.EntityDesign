using LLP.EntityDesign.API.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LLP.EntityDesign.API.Data.Orders
{
    public class Order
    {
        public int Id { get; }
        public string OrderNo { get; }
        public DateTime DateCreated { get; }
        public DateTime? DateCompleted { get; private set; }
        public Customer Customer { get; }
        public Address Address { get; private set; }
        public decimal GrandTotal { get; private set; }


        private readonly List<OrderItem> _items = new List<OrderItem>();
        public IEnumerable<OrderItem> Items => _items.AsEnumerable();


        // Just to demostrate calculated properties.
        public string GrandTotalNormalized => this.GrandTotal.ToString("n2");


        private Order(int id, string orderNo, DateTime dateCreated)
        {
            this.Id = id;
            this.OrderNo = orderNo;
            this.DateCreated = dateCreated;
        }

        public Order(IDateTime dateTimeService, string orderNo, Customer customer, Address address)
        {
            _ = dateTimeService ?? throw new ArgumentNullException(nameof(dateTimeService));
            _ = customer ?? throw new ArgumentNullException(nameof(customer));
            if (string.IsNullOrEmpty(orderNo)) throw new ArgumentNullException(nameof(orderNo));

            //this.Id = if we decide to use DB generated value no action required.

            this.OrderNo = orderNo;
            this.DateCreated = dateTimeService.Now;
            this.Customer = customer;

            UpdateAddress(address);
        }

        /// It's important not to update the Address if there are no changes.
        /// If updated, since it's a new object, EF tracker will mark it as New.
        /// This can have implications on "Auditing" logic, if you have implemented one. 
        public void UpdateAddress(Address address)
        {
            _ = address ?? throw new ArgumentNullException(nameof(address));

            if (this.Address is null || !this.Address.Equals(address))
            {
                this.Address = address;
            }
        }

        public OrderItem AddItem(OrderItem orderItem)
        {
            _ = orderItem ?? throw new ArgumentNullException(nameof(orderItem));

            _items.Add(orderItem);

            CalculateGrandTotal();

            return orderItem;
        }

        public void DeleteItem(int orderItemId)
        {
            var orderItem = Items.FirstOrDefault(x => x.Id == orderItemId);
            _ = orderItem ?? throw new KeyNotFoundException($"The order item with Id: {orderItemId} is not found!");

            _items.Remove(orderItem);

            CalculateGrandTotal();
        }

        public void Complete(IDateTime dateTimeService)
        {
            this.DateCompleted = dateTimeService.Now;
        }

        private void CalculateGrandTotal()
        {
            this.GrandTotal = this.Items.Sum(x => x.Price);
        }
    }
}
