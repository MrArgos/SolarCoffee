using SolarCoffee.Data.Models;
using SolarCoffee.Web.Controllers;

namespace SolarCoffee.Web.Serialization
{
    public static class InventoryMapper
    {
        public static ProductInventoryModel SerializeInventoryModel(ProductInventory inventory)
        {
            var inventoryModel = new ProductInventoryModel
            {
                Id = inventory.Id,
                QuantityOnHand = inventory.QuantityOnHand,
                IdealQuantity = inventory.IdealQuantity,
                CreatedOn = inventory.CreatedOn,
                UpdatedOn = inventory.UpdatedOn,
                ProductModel = ProductMapper.SerializeProductModel(inventory.Product)
            };
            return inventoryModel;
        }
        
        public static ProductInventory SerializeInventoryModel(ProductInventoryModel inventoryModel)
        {
            var inventory = new ProductInventory
            {
                Id = inventoryModel.Id,
                QuantityOnHand = inventoryModel.QuantityOnHand,
                IdealQuantity = inventoryModel.IdealQuantity,
                CreatedOn = inventoryModel.CreatedOn,
                UpdatedOn = inventoryModel.UpdatedOn,
                Product = ProductMapper.SerializeProductModel(inventoryModel.ProductModel)
            };
            return inventory;
        }
    }
}