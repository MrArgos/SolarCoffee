using SolarCoffee.Data.Models;
using SolarCoffee.Web.Controllers;

namespace SolarCoffee.Web.Serialization
{
    public static class InventoryMapper
    {
        public static ProductInventoryModel SerializeInventoryModel(ProductInventory inventory)
        {
            return new ProductInventoryModel
            {
                Id = inventory.Id,
                QuantityOnHand = inventory.QuantityOnHand,
                IdealQuantity = inventory.IdealQuantity,
                CreatedOn = inventory.CreatedOn,
                UpdatedOn = inventory.UpdatedOn,
                Product = ProductMapper.SerializeProductModel(inventory.Product)
            };
        }
        
        public static ProductInventory SerializeInventoryModel(ProductInventoryModel inventory)
        {
            return new ProductInventory
            {
                Id = inventory.Id,
                QuantityOnHand = inventory.QuantityOnHand,
                IdealQuantity = inventory.IdealQuantity,
                CreatedOn = inventory.CreatedOn,
                UpdatedOn = inventory.UpdatedOn,
                Product = ProductMapper.SerializeProductModel(inventory.Product)
            };
        }
    }
}