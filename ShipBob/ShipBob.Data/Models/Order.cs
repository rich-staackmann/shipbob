using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Linq;

namespace ShipBob.Data.Models
{
    public class Order
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; private set; }
        public string TrackingId { get; private set; }
        public string Name { get; private set; }
        public string Street { get; private set; }
        public string City { get; private set; }
        public string State { get; private  set; }
        public string ZipCode { get; private set; }
        public int UserId { get; private set; }

        private Order()
        { 
        }

        public Order(string trackingId, string name, string street, string city, string state, string zipCode, int userId)
        {
            TrackingId = trackingId;
            Name = name;
            Street = street;
            City = city;
            State = state;
            ZipCode = zipCode;
            UserId = userId;
        }

        public void UpdateOrder(string trackingId, string name, string street, string city, string state, string zipCode, UserOrderContext context)
        {
            var user = context.Users.Include(u => u.Orders).Single( u => u.UserId == this.UserId);

            if (user != null)
            {
                var order = user.Orders.Where(x => x.OrderId == this.OrderId).FirstOrDefault();
                if(order != null)
                {
                    order.TrackingId = trackingId;
                    order.Name = name;
                    order.Street = street;
                    order.City = city;
                    order.State = state;
                    order.ZipCode = zipCode;
                }
                

                context.Users.Update(user);
                context.SaveChanges();
            }
        }
    }
}
