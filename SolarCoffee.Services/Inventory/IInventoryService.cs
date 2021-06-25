using System.Collections.Generic;
using SolarCoffee.Data.Models;

namespace SolarCoffee.Services.Inventory
{
    public interface IInventoryService
    {
        List<ProductInventory> GetCurrentInventory();
        ServiceResponse<ProductInventory> UpdateUnitsAvailable(int id, int adjustment);
        ProductInventory GetByProductId(int productId);
        ServiceResponse<bool> CreateSnapshot(int productInventoryId);
        List<ProductInventorySnapshot> GetSnapshotHistory();
    }
}