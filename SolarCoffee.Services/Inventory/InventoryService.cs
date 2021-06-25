using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SolarCoffee.Data;
using SolarCoffee.Data.Models;

namespace SolarCoffee.Services.Inventory
{
    class InventoryService : IInventoryService
    {
        private readonly ILogger<InventoryService> _logger;
        private readonly SolarDbContext _db;

        public InventoryService(ILogger<InventoryService> logger, SolarDbContext db)
        {
            _logger = logger;
            _db = db;
        }
        
        /// <summary>
        /// Returns all current inventory from the database
        /// </summary>
        /// <returns></returns>
        public List<ProductInventory> GetCurrentInventory()
        {
            return _db.ProductInventories
                .Include(pi => pi.Product)
                .Where(pi => !pi.Product.IsArchived)
                .ToList();
        }

        /// <summary>
        /// Updates number of units available of the provided id.
        /// Adjusts QuantityOnHand by adjustment value
        /// </summary>
        /// <param name="id">productId</param>
        /// <param name="adjustment">number of units added / removed from inventory</param>
        /// <returns>ServiceResponse ProductInventory</returns>
        public ServiceResponse<ProductInventory> UpdateUnitsAvailable(int id, int adjustment)
        {
            try
            {
                var inventory = _db.ProductInventories
                    .Include(pi => pi.Product)
                    .First(pi => pi.Product.Id == id);
                
                inventory.QuantityOnHand += adjustment;

                _db.SaveChanges();
                
                try
                {
                    CreateSnapshot(inventory.Id);
                }
                catch (Exception e)
                {
                    _logger.LogError("Error Creating Snapshot");
                    _logger.LogError(e.StackTrace);
                }

                return new ServiceResponse<ProductInventory>
                {
                    Data = inventory,
                    IsSuccess = true,
                    Message = $"Product {id} inventory updated",
                    Time = DateTime.UtcNow
                };
            }
            catch (Exception e)
            {
                _logger.LogError("Error Updating Units");
                _logger.LogError(e.StackTrace);
                return new ServiceResponse<ProductInventory>
                {
                    Data = null,
                    IsSuccess = false,
                    Message = $"Failed to update Product {id} inventory",
                    Time = DateTime.UtcNow
                };
            }
        }

        /// <summary>
        /// Returns ProductInventory by Primary Key
        /// </summary>
        /// <param name="productId">Primary Key</param>
        /// <returns>ProductInventory</returns>
        public ProductInventory GetByProductId(int productId)
        {
            return _db.ProductInventories
                .Include(pi => pi.Product)
                .FirstOrDefault(pi =>  pi.Product.Id == productId);
        }

        /// <summary>
        /// Creates Snapshot of inventory at current time
        /// </summary>
        /// <param name="productInventoryId">Primary Key of ProductInventory to snapshot</param>
        /// <returns>ServiceResponse bool</returns>
        public ServiceResponse<bool> CreateSnapshot(int productInventoryId)
        {
            try
            {
                var productInventory = _db.ProductInventories
                    .Include(pi => pi.Product)
                    .First(pi => pi.Id == productInventoryId);

                var snapshot = new ProductInventorySnapshot
                {
                    Product = productInventory.Product,
                    QuantityOnHand = productInventory.QuantityOnHand,
                    SnapshotTime = DateTime.UtcNow
                };
                _db.Add(snapshot);
                _db.SaveChanges();
                
                return new ServiceResponse<bool>
                {
                    Data = true,
                    IsSuccess = true,
                    Message = $"Snapshot created for inventory {productInventoryId}",
                    Time = DateTime.UtcNow
                };
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to create Snapshot for inventory");
                _logger.LogError(e.StackTrace);
                return new ServiceResponse<bool>
                {
                    Data = false,
                    IsSuccess = false,
                    Message = $"Failed to create Snapshot for inventory {productInventoryId}",
                    Time = DateTime.UtcNow
                };
            }
        }

        
        public List<ProductInventorySnapshot> GetSnapshotHistory()
        {
            throw new System.NotImplementedException();
        }
    }
}