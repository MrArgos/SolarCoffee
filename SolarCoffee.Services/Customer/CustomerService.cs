using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SolarCoffee.Data;

namespace SolarCoffee.Services.Customer
{
    public class CustomerService : ICustomerService
    {
        private readonly ILogger<CustomerService> _logger;
        private readonly SolarDbContext _db;

        public CustomerService(ILogger<CustomerService> logger, SolarDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        /// <summary>
        /// Returns a list of Customers from the database
        /// </summary>
        /// <returns>List Customer</returns>
        public List<Data.Models.Customer> GetAllCustomers()
        {
            return _db.Customers
                .Include(c => c.PrimaryAddress)
                .OrderBy(c => c.LastName)
                .ToList();
        }

        /// <summary>
        /// Adds a new Customer record
        /// </summary>
        /// <param name="customer">Customer instance</param>
        /// <returns>ServiceResponse Customer</returns>
        public ServiceResponse<Data.Models.Customer> CreateCustomer(Data.Models.Customer customer)
        {
            try
            {
                _db.Add(customer);
                _db.SaveChanges();
                return new ServiceResponse<Data.Models.Customer>
                {
                    Data = customer,
                    IsSuccess = true,
                    Message = "Customer inserted in database",
                    Time = DateTime.UtcNow
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace);
                return new ServiceResponse<Data.Models.Customer>
                {
                    Data = null,
                    IsSuccess = false,
                    Message = "Failed inserting Customer in database",
                    Time = DateTime.UtcNow
                };
            }
        }

        /// <summary>
        /// Deletes a Customer record
        /// </summary>
        /// <param name="id">Id of Customer to delete</param>
        /// <returns>ServiceResponse bool</returns>
        public ServiceResponse<bool> DeleteCustomer(int id)
        {
            try
            {
                var customer = _db.Customers.Find(id);
                if (customer == null)
                {
                    return new ServiceResponse<bool>
                    {
                        Data = false,
                        IsSuccess = false,
                        Message = "Failed inserting Customer in database",
                        Time = DateTime.UtcNow
                    };
                }

                _db.Remove(customer);
                _db.SaveChanges();
                return new ServiceResponse<bool>
                {
                    Data = true,
                    IsSuccess = true,
                    Message = "Failed inserting Customer in database",
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
                    Message = "Failed inserting Customer in database",
                    Time = DateTime.UtcNow
                };
            }
        }

        /// <summary>
        /// Gets a Customer record by Primary Key
        /// </summary>
        /// <param name="id"> Primary Key of Customer</param>
        /// <returns>Customer</returns>
        public Data.Models.Customer GetCustomerById(int id)
        {
            return _db.Customers.Find(id);
        }
    }
}