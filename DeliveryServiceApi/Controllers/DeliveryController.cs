using DeliveryServiceApi.Data;
using DeliveryServiceApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryServiceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeliveryController : ControllerBase
    {
        private readonly IOrderService orderService;
        private readonly AppDbContext appDbContext;

        public DeliveryController(IOrderService orderService, AppDbContext appDbContext) // dbcontext just for changing db in tests
        {
            if (orderService is null)
                throw new ArgumentNullException(nameof(IOrderService));

            this.orderService = orderService?? throw new ArgumentNullException(nameof(IOrderService));
            this.appDbContext = appDbContext?? throw new ArgumentNullException(nameof(AppDbContext));
        }

        [HttpGet("check-status")]
        public IActionResult CheckStatus() => Ok("active");

        [HttpPost("send-order")]
        public IActionResult SendOrder()
        {
            try
            {
                return orderService.DeliveryAvailable() ? Ok("Delivery is available for order") : NotFound();

            }
            catch (Exception)
            {

                return BadRequest("Failed to order");
            }
        }

        [HttpGet("get-orders-count")]
        public IActionResult GetOrdersCount() => Ok(appDbContext.Orders.Count());
    }
}
