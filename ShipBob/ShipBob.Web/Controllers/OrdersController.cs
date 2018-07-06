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

        [HttpPost]
        public IActionResult Post([FromBody]Order order)
        {
            _userOrderContext.Add(order);
            _userOrderContext.SaveChanges();

            return CreatedAtRoute("GetOrder", new { id = order.OrderId }, order);
        }
    }
}
