using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SolarCoffee.Services.Customer;
using SolarCoffee.Web.Serialization;
using SolarCoffee.Web.ViewModels;

namespace SolarCoffee.Web.Controllers
{
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomerService _customerService;

        public CustomerController(ILogger<CustomerController> logger, ICustomerService customerService)
        {
            _logger = logger;
            _customerService = customerService;
        }

        [HttpPost("/api/customer")]
        public ActionResult CreateCustomer([FromBody] CustomerModel customerModel)
        {
            _logger.LogInformation("Creating a new Customer");
            customerModel.CreatedOn = DateTime.UtcNow;
            customerModel.UpdatedOn = customerModel.CreatedOn;
            customerModel.PrimaryAddress.CreatedOn = DateTime.UtcNow;
            customerModel.PrimaryAddress.UpdatedOn = customerModel.PrimaryAddress.CreatedOn;
            var customerData = CustomerMapper.SerializeCustomer(customerModel);
            var newCustomer = _customerService.CreateCustomer(customerData);
            return Ok(newCustomer);
        }

        [HttpGet("/api/customer")]
        public ActionResult GetAllCustomers()
        {
            _logger.LogInformation("Getting all customers");
            var customers = _customerService.GetAllCustomers();
            var customerModels = customers
                .Select(CustomerMapper.SerializeCustomer)
                .OrderByDescending(customer => customer.CreatedOn)
                .ToList();
            
            return Ok(customerModels);
        }

        [HttpGet("/api/customer/{id:int}")]
        public ActionResult GetCustomerById(int id)
        {
            _logger.LogInformation("Getting customer with id: {Id}",id);
            var customer = _customerService.GetCustomerById(id);
            if (customer == null)
            {
                return NoContent();
            }
            var customerModel = CustomerMapper.SerializeCustomer(customer);
            return Ok(customerModel);
        }

        [HttpDelete("/api/customer/{id:int}")]
        public ActionResult DeleteCustomer(int id)
        {
            _logger.LogInformation("Deleting Customer with id: {Id}",id);
            var response = _customerService.DeleteCustomer(id);
            return Ok(response);
        }
    }
}