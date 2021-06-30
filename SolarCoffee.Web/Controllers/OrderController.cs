using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SolarCoffee.Services.Customer;
using SolarCoffee.Services.Order;
using SolarCoffee.Web.Serialization;
using SolarCoffee.Web.ViewModels;

namespace SolarCoffee.Web.Controllers
{
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderService _orderService;
        private readonly ICustomerService _customerService;

        public OrderController(
            ILogger<OrderController> logger, 
            IOrderService orderService, 
            ICustomerService customerService)
        {
            _logger = logger;
            _orderService = orderService;
            _customerService = customerService;
        }

        [HttpPost("/api/invoice")]
        public ActionResult GenerateNewOrder([FromBody] InvoiceModel invoice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            _logger.LogInformation("Generating Invoice");
            var order = OrderMapper.SerializeInvoiceToOrder(invoice);
            order.Customer = _customerService.GetCustomerById(invoice.CustomerId);
            var response = _orderService.GenerateOpenOrder(order);
            return Ok(response);
        }

        [HttpGet("/api/order")]
        public ActionResult GetAllOrders()
        {
            _logger.LogInformation("Getting all orders");
            var orders = _orderService.GetOrders();
            var orderModels = OrderMapper.SerializeOrdersToViewModels(orders);
            return Ok(orderModels);
        }

        [HttpPatch("/api/order/complete/{id:int}")]
        public ActionResult MarkingOrderAsPaid(int id)
        {
            _logger.LogInformation("Marking order {Id} as paid",id);
            var response = _orderService.MakeFulfilled(id);
            return Ok(response);
        }
    }
}