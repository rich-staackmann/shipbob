using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using NSubstitute;
using Microsoft.EntityFrameworkCore;
using ShipBob.Data.Models;
using System.Linq;
using ShipBob.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using ShipBob.Web.DTO;

namespace ShipBob.Tests.Web
{
    public class OrderControllerTests
    {
        [Fact]
        public void GetUserOrders_Should_ReturnOrdersForUserId()
        {
            var options = new DbContextOptionsBuilder<UserOrderContext>()
              .UseInMemoryDatabase(databaseName: "order_controller_tests1")
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
                var ordersController = new OrdersController(context);
                var ordersResult = ordersController.GetUserOrders(testUser.UserId) as OkObjectResult;
                var orders = (List<Order>)ordersResult.Value;
                Assert.Single(orders);
                Assert.Equal(testOrder.TrackingId, orders.First().TrackingId);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public void Create_Should_ReturnOrderLocation()
        {
            var options = new DbContextOptionsBuilder<UserOrderContext>()
              .UseInMemoryDatabase(databaseName: "order_controller_tests2")
              .Options;
            var testUser = new User("test", "mc testy face", 1);
            var testOrder = new OrderDTO()
            {
                TrackingId = "1234",
                Name = "nnnn",
                Street = "sssss",
                City = "cccc",
                State = "ssss",
                ZipCode = "zzzzz",
                UserId = 1
            };
            using (var context = new UserOrderContext(options))
            {
                context.Users.Add(testUser);
                context.SaveChanges();
            }

            using (var context = new UserOrderContext(options))
            {
                var ordersController = new OrdersController(context);
                var ordersResult = ordersController.Create(testOrder) as CreatedAtRouteResult;
                var order = (Order)ordersResult.Value;
                Assert.Equal(201, ordersResult.StatusCode);
                Assert.Equal("GetUserOrders", ordersResult.RouteName);
                Assert.Equal(testOrder.TrackingId, order.TrackingId);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public void Create_Should_Return412IfORderInvalid()
        {
            var options = new DbContextOptionsBuilder<UserOrderContext>()
              .UseInMemoryDatabase(databaseName: "order_controller_tests3")
              .Options;
            var testUser = new User("test", "mc testy face", 1);
            var testOrder = new OrderDTO()
            {
                TrackingId = "1234",
                Name = "nnnn",
                Street = "sssss",
                City = "cccc",
                State = "ssss",
                ZipCode = "zzzzz",
                UserId = 2
            };
            using (var context = new UserOrderContext(options))
            {
                context.Users.Add(testUser);
                context.SaveChanges();
            }

            using (var context = new UserOrderContext(options))
            {
                var ordersController = new OrdersController(context);
                var ordersResult = ordersController.Create(testOrder) as ObjectResult;
                
                Assert.Equal(412, ordersResult.StatusCode);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public void Update_Should_Return404IfORderInvalid()
        {
            var options = new DbContextOptionsBuilder<UserOrderContext>()
              .UseInMemoryDatabase(databaseName: "order_controller_tests4")
              .Options;
            var testUser = new User("test", "mc testy face", 1);
            var testOrder = new Order("1234", "aaa", "bbb", "ccc", "IL", "60004", 1);
            var testOrderDTO = new OrderDTO()
            {
                TrackingId = "1234",
                Name = "nnnn",
                Street = "sssss",
                City = "cccc",
                State = "ssss",
                ZipCode = "zzzzz",
                UserId = 1
            };
            using (var context = new UserOrderContext(options))
            {
                context.Users.Add(testUser);
                testUser.AddOrder(testOrder, context);
                context.SaveChanges();
            }

            using (var context = new UserOrderContext(options))
            {
                var ordersController = new OrdersController(context);
                var ordersResult = ordersController.Update(2, testOrderDTO) as NotFoundResult;

                Assert.Equal(404, ordersResult.StatusCode);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public void Update_Should_ReturnOk()
        {
            var options = new DbContextOptionsBuilder<UserOrderContext>()
              .UseInMemoryDatabase(databaseName: "order_controller_tests5")
              .Options;
            var testUser = new User("test", "mc testy face", 2);
            var testOrder = new Order("1234", "aaa", "bbb", "ccc", "IL", "60004", 2);
            var testOrderDTO = new OrderDTO()
            {
                TrackingId = "1234",
                Name = "nnnn",
                Street = "sssss",
                City = "cccc",
                State = "ssss",
                ZipCode = "zzzzz",
                UserId = 2
            };
            using (var context = new UserOrderContext(options))
            {
                context.Users.Add(testUser);
                testUser.AddOrder(testOrder, context);
                context.SaveChanges();
            }

            using (var context = new UserOrderContext(options))
            {
                //ef core in memory provider doesnt reset its increments for auto generated ids
                //so you cant rely on the first object added having an id of 1
                var user = context.Users.Include(u => u.Orders).Single(u => u.UserId == 2);
                var orderId = user.Orders.First().OrderId;
                var ordersController = new OrdersController(context);
                var ordersResult = ordersController.Update(orderId, testOrderDTO) as OkResult;

                Assert.Equal(200, ordersResult.StatusCode);

                context.Database.EnsureDeleted();
            }
        }
    }
}
