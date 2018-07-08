using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace ShipBob.Data.Models
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public IList<Order> Orders { get; private set; }

        private User() { }

        public User(string firstName, string lastName, int userId = 0)
        {
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            Orders = new List<Order>();
        }

        public void AddOrder(Order order, UserOrderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context),
                    "You must provide a context");
            }
            if(this.Orders == null)
            {
                this.Orders = new List<Order>();
            }
            this.Orders.Add(order);
            context.SaveChanges();
        }

    }
}
