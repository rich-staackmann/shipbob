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

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var users = await _userOrderContext.Users
                .Include(u => u.Orders)
                .ToArrayAsync();

            

            return Ok(users);
        }
    }
}
