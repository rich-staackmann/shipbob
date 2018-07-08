using Microsoft.AspNetCore.Mvc;
using ShipBob.Data.Models;
using ShipBob.Web.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShipBob.Web.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly UserOrderContext _userOrderContext;

        public UsersController(UserOrderContext userOrderContext)
        {
            _userOrderContext = userOrderContext;
        }

        [HttpGet(Name = "GetUsers")]
        public IActionResult Get()
        {
            var users = _userOrderContext.Users.ToList();

            return Ok(users);
        }

        [HttpPost]
        public IActionResult Post([FromBody]UserDTO userDTO)
        {
            var user = new User(userDTO.FirstName, userDTO.LastName);

            _userOrderContext.Add(user);
            _userOrderContext.SaveChanges();

            return CreatedAtRoute("GetUsers", new { id = user.UserId }, user);
        }
    }
}
