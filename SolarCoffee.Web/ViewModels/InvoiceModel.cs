using System;
using System.Collections.Generic;
using SolarCoffee.Data.Models;

namespace SolarCoffee.Web.ViewModels
{
    /// <summary>
    /// View model for open SalesOrders
    /// </summary>
    public class InvoiceModel
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int CustomerId { get; set; }
        public List<SalesOrderItemModel> LineItems { get; set; }
    }
}