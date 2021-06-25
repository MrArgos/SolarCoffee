using System.Collections.Generic;
using SolarCoffee.Data.Models;

namespace SolarCoffee.Services.Order
{
    public interface IOrderService
    {
        List<SalesOrder> GetOrders();
        ServiceResponse<SalesOrder> GetOrderById(int id);
        ServiceResponse<bool> GenerateInvoiceForOrder(SalesOrder order);
        ServiceResponse<bool> MakeFulfilled(int id);
    }
}