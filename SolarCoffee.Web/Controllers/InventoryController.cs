using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SolarCoffee.Services.Inventory;
using SolarCoffee.Web.Serialization;

namespace SolarCoffee.Web.Controllers
{
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly ILogger<InventoryController> _logger;
        private readonly IInventoryService _inventoryService;

        public InventoryController(ILogger<InventoryController> logger, IInventoryService inventoryService)
        {
            _logger = logger;
            _inventoryService = inventoryService;
        }

        [HttpGet("/api/inventory")]
        public ActionResult GetCurrentInventory()
        {
            _logger.LogInformation("Getting all inventory");
            var inventory = _inventoryService.GetCurrentInventory()
                .Select(InventoryMapper.SerializeInventoryModel)
                .OrderBy(inv => inv.ProductModel.Name)
                .ToList();

            return Ok(inventory);
        }

        [HttpPatch("/api/inventory")]
        public ActionResult UpdateInventory([FromBody] ShipmentModel shipment)
        {
            _logger.LogInformation("Updating inventory for {ProductId} " +
                                   "- Adjustment: {Adjustment}",
                shipment.ProductId, shipment.Adjustment);

            var response = _inventoryService.UpdateUnitsAvailable(shipment.ProductId, shipment.Adjustment);
            
            return Ok(response);
        }
    }
}