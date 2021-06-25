using System.Collections.Generic;
using SolarCoffee.Data.Models;

namespace SolarCoffee.Services.Customer
{
    public interface ICustomerService
    {
        List<Data.Models.Customer> GetAllCustomers();
        ServiceResponse<Data.Models.Customer> CreateCustomer(Data.Models.Customer customer);
        ServiceResponse<bool> DeleteCustomer(int id);
        Data.Models.Customer GetCustomerById(int id);
    }
}