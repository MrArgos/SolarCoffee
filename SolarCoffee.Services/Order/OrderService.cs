using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using SolarCoffee.Data;
using SolarCoffee.Data.Models;

namespace SolarCoffee.Services.Order
{
    class OrderService : IOrderService
    {
        private readonly ILogger<OrderService> _logger;
        private readonly SolarDbContext _db;

        public OrderService(ILogger<OrderService> logger, SolarDbContext db)
        {
            _logger = logger;
            _db = db;
        }
        
        public List<SalesOrder> GetOrders()
        {
            throw new System.NotImplementedException();
        }

        public ServiceResponse<SalesOrder> GetOrderById(int id)
        {
            throw new System.NotImplementedException();
        }

        public ServiceResponse<bool> GenerateInvoiceForOrder(SalesOrder order)
        {
            throw new System.NotImplementedException();
        }

        public ServiceResponse<bool> MakeFulfilled(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}