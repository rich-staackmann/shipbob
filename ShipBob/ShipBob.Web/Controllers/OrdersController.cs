using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShipBob.Data.Models;
using ShipBob.Web.DTO;
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

        [HttpGet("user/{userId}", Name = "GetUserOrders")]
        public IActionResult GetUserOrders(int userId)
        {
            var user = _userOrderContext.Users
                .Include(u => u.Orders)
                .Single(x => x.UserId == userId);

            return Ok(user.Orders);
        }

        [HttpPost]
        public IActionResult Create([FromBody]OrderDTO orderDTO)
        {
            var user = _userOrderContext.Users
                .Where(x => x.UserId == orderDTO.UserId)
                .ToList()
                .FirstOrDefault();
            if(user != null)
            {
                var order = new Order(orderDTO.TrackingId, orderDTO.Name, orderDTO.Street,orderDTO.City,orderDTO.State,orderDTO.ZipCode,orderDTO.UserId);
                user.AddOrder(order, _userOrderContext);
                return CreatedAtRoute("GetUserOrders", new { userId = order.UserId }, order);
            }

            return new ObjectResult("The order's user was not found")
            {
                StatusCode = 412
            };
        }

        [HttpPut("{orderId}")]
        public IActionResult Update(int orderId, [FromBody]OrderDTO orderDTO)
        {
            try
            {
                var user = _userOrderContext.Users
                    .Include(u => u.Orders)
                    .Single(x => x.UserId == orderDTO.UserId);
                var order = user.Orders.Where(o => o.OrderId == orderId).FirstOrDefault();

                if (order == null)
                {
                    return NotFound();
                }

                order.UpdateOrder(orderDTO.TrackingId, orderDTO.Name, orderDTO.Street, orderDTO.City, orderDTO.State, orderDTO.ZipCode, _userOrderContext);

                return Ok();
            }
            catch(InvalidOperationException ex)
            {
                return NotFound();
            }
            catch(Exception ex)
            {
                return new ObjectResult("Error")
                {
                    StatusCode = 500
                };
            }
        }
    }
}
