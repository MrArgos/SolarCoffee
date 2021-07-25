using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SolarCoffee.Data;
using SolarCoffee.Data.Models;

namespace SolarCoffee.Services.Inventory
{
    public class InventoryService : IInventoryService
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
                .OrderBy(pi => pi.Product.Name)
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
                inventory.UpdatedOn = DateTime.UtcNow;

                try
                {
                    CreateSnapshot(inventory);
                }
                catch (Exception e)
                {
                    _logger.LogError("Error Creating Snapshot");
                    _logger.LogError(e.StackTrace);
                }

                _db.SaveChanges();

                return new ServiceResponse<ProductInventory>
                {
                    Data = inventory,
                    IsSuccess = true,
                    Message = $"Product '{inventory.Product.Name}' inventory updated",
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
                    Message = "Failed to update Product inventory",
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
                .FirstOrDefault(pi => pi.Product.Id == productId);
        }


        /// <summary>
        /// Creates Snapshot of inventory at current time
        /// </summary>
        /// <param name="inventory">Inventory to snapshot</param>
        public void CreateSnapshot(ProductInventory inventory)
        {
            var now = DateTime.UtcNow;

            var snapshot = new ProductInventorySnapshot
            {
                SnapshotTime = now,
                Product = inventory.Product,
                QuantityOnHand = inventory.QuantityOnHand
                
            };
            _db.Add(snapshot);

            /* _db.SaveChanges();  //No need to save changes, because SaveChanges is
                                  // called right after CreateSnapshot in UpdateUnitsAvailable
            */
        }

        /// <summary>
        /// Snapshot history for the previous 6 hours
        /// </summary>
        /// <returns>List of ProductInventorySnapshot</returns>
        public List<ProductInventorySnapshot> GetSnapshotHistory()
        {
            var earliest = DateTime.UtcNow - TimeSpan.FromHours(6);

            return _db.ProductInventorySnapshots
                .Include(snap => snap.Product)
                .Where(snap => snap.SnapshotTime > earliest
                               && !snap.Product.IsArchived)
                .ToList();
        }
    }
}