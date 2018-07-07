using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShipBob.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShipBob.Web.Controllers
{
    [Route("api/[controller]")]
    public class OrdersController : Controller
    {
        private readonly UserOrderContext _userOrderContext;

        public OrdersController(UserOrderContext userOrderContext)
        {
            _userOrderContext = userOrderContext;
        }

        [HttpGet("{id}", Name = "GetOrder")]
        public IActionResult Get(int id)
        {
            var order = _userOrderContext.Orders
                .Where(o => o.OrderId == id )
                .ToList();
          
            return Ok(order);
        }

        [HttpGet("user/{userId}")]
        public IActionResult GetUserOrders(int userId)
        {
            var order = _userOrderContext.Orders
                .Where(o => o.UserId == userId)
                .ToList();

            return Ok(order);
        }

        [HttpPost]
        public IActionResult Post([FromBody]Order order)
        {
            _userOrderContext.Add(order);
            _userOrderContext.SaveChanges();

            return CreatedAtRoute("GetOrder", new { id = order.OrderId }, order);
        }

        [HttpPut("{orderId}")]
        public IActionResult Update(int orderId, [FromBody]Order updatedOrder)
        {
            var order = _userOrderContext.Orders.Find(orderId);
            if (order == null)
            {
                return NotFound();
            }

            order.TrackingId = updatedOrder.TrackingId;
            order.Name = updatedOrder.Name;
            order.Street = updatedOrder.Street;
            order.City = updatedOrder.City;
            order.State = updatedOrder.State;
            order.ZipCode = updatedOrder.ZipCode;

            _userOrderContext.Orders.Update(order);
            _userOrderContext.SaveChanges();
            return Ok();
        }
    }
}
