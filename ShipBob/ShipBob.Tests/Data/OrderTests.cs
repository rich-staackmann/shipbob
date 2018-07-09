using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using NSubstitute;
using Microsoft.EntityFrameworkCore;
using ShipBob.Data.Models;
using System.Linq;

namespace ShipBob.Tests.Data
{
    public class OrderTests
    {
        [Fact]
        public void UpdateOrder_Should_UpdateAndSaveToContext()
        {
            var options = new DbContextOptionsBuilder<UserOrderContext>()
               .UseInMemoryDatabase(databaseName: "order_test")
               .Options;
            var testUser = new User("test", "mc testy face", 1);
            var testOrder = new Order("1234", "aaa", "bbb", "ccc", "IL", "60004", 1);
            using (var context = new UserOrderContext(options))
            {
                context.Users.Add(testUser);
                testUser.AddOrder(testOrder, context);
                context.SaveChanges();
            }

            using (var context = new UserOrderContext(options))
            {
                testOrder.UpdateOrder("5555", "aaa", "bbb", "ccc", "IL", "60004", context);
            }

            using (var context = new UserOrderContext(options))
            {
                var user = context.Users.Include(u => u.Orders).Single(u => u.UserId == 1);
                Assert.Equal(1, user.Orders.Count);
                Assert.Equal("5555", user.Orders.First().TrackingId);
            }
        }
    }
}
