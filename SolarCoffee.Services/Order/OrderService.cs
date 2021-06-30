using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SolarCoffee.Data;
using SolarCoffee.Data.Models;
using SolarCoffee.Services.Inventory;
using SolarCoffee.Services.Product;

namespace SolarCoffee.Services.Order
{
    public class OrderService : IOrderService
    {
        private readonly ILogger<OrderService> _logger;
        private readonly SolarDbContext _db;
        private readonly IProductService _productService;
        private readonly IInventoryService _inventoryService;

        public OrderService(
            ILogger<OrderService> logger,
            SolarDbContext db,
            IProductService productService,
            IInventoryService inventoryService)
        {
            _logger = logger;
            _db = db;
            _productService = productService;
            _inventoryService = inventoryService;
        }

        /// <summary>
        /// Returns list of SalesOrder
        /// </summary>
        /// <returns>List SalesOrder</returns>
        public List<SalesOrder> GetOrders()
        {
            return _db.SalesOrders
                .Include(order => order.Customer)
                    .ThenInclude(customer => customer.PrimaryAddress)
                .Include(order => order.SalesOrderItems)
                    .ThenInclude(item => item.Product)
                .ToList();
        }

        /// <summary>
        /// Retrieves SalesOrder by Id
        /// </summary>
        /// <param name="id">Sales Order Primary Key</param>
        /// <returns>ServiceResponse SalesOrder</returns>
        public ServiceResponse<SalesOrder> GetOrderById(int id)
        {
            try
            {
                var salesOrder = _db.SalesOrders
                    .Include(order => order.Customer)
                        .ThenInclude(customer => customer.PrimaryAddress)
                    .Include(order => order.SalesOrderItems)
                        .ThenInclude(item => item.Product)
                    .First(x => x.Id == id);

                return new ServiceResponse<SalesOrder>
                {
                    Data = salesOrder,
                    IsSuccess = true,
                    Message = "Order retrieved successfully",
                    Time = DateTime.UtcNow
                };
            }
            catch (Exception e)
            {
                _logger.LogError("Error retrieving Order");
                _logger.LogError(e.StackTrace);

                return new ServiceResponse<SalesOrder>
                {
                    Data = null,
                    IsSuccess = true,
                    Message = "Order retrieved successfully",
                    Time = DateTime.UtcNow
                };
            }
        }

        /// <summary>
        /// Creates an open SalesOrder
        /// </summary>
        /// <param name="order"></param>
        /// <returns>ServiceResponse bool</returns>
        public ServiceResponse<bool> GenerateOpenOrder(SalesOrder order)
        {
            foreach (var item in order.SalesOrderItems)
            {
                item.Product = _productService.GetProductById(item.Product.Id);
                _inventoryService.UpdateUnitsAvailable(item.Product.Id, -item.Quantity);
            }

            try
            {
                _db.SalesOrders.Add(order);
                _db.SaveChanges();
                return new ServiceResponse<bool>
                {
                    Data = true,
                    IsSuccess = true,
                    Message = "yes",
                    Time = DateTime.UtcNow
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace);
                return new ServiceResponse<bool>
                {
                    Data = false,
                    IsSuccess = false,
                    Message = "no",
                    Time = DateTime.UtcNow
                };
            }
        }

        /// <summary>
        /// Fulfils a SalesOrder
        /// </summary>
        /// <param name="id">Primary Key of SalesOrder to fulfill</param>
        /// <returns>ServiceResponse bool</returns>
        public ServiceResponse<bool> MakeFulfilled(int id)
        {
            var now = DateTime.UtcNow;
            try
            {
                var order = _db.SalesOrders.Find(id);

                order.IsPaid = true;
                order.UpdatedOn = now;
                _db.SalesOrders.Update(order);
                _db.SaveChanges();

                return new ServiceResponse<bool>
                {
                    Data = true,
                    IsSuccess = true,
                    Message = "SalesOrder is now fulfilled",
                    Time = now
                };
            }
            catch (Exception e)
            {
                _logger.LogError("Error fulfilling SalesOrder");
                _logger.LogError(e.StackTrace);
                return new ServiceResponse<bool>
                {
                    Data = false,
                    IsSuccess = true,
                    Message = "Error fulfilling SalesOrder",
                    Time = now
                };
            }
        }
    }
}