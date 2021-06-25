using SolarCoffee.Data.Models;
using SolarCoffee.Web.ViewModels;

namespace SolarCoffee.Web.Serialization
{
    public static class CustomerMapper
    {
        /// <summary>
        /// Serializes a Customer (data model) to CustomerModel (view model)
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public static CustomerModel SerializeCustomer(Customer customer)
        {
            return new CustomerModel
            {
                Id = customer.Id,
                CreatedOn = customer.CreatedOn,
                UpdatedOn = customer.UpdatedOn,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                PrimaryAddress = SerializeCustomerAddress(customer.PrimaryAddress)
            };
        }

        /// <summary>
        /// Serializes a CustomerModel (view model) to Customer (data model)
        /// </summary>
        /// <param name="customerModel"></param>
        /// <returns></returns>
        public static Customer SerializeCustomer(CustomerModel customerModel)
        {
            return new Customer
            {
                Id = customerModel.Id,
                CreatedOn = customerModel.CreatedOn,
                UpdatedOn = customerModel.UpdatedOn,
                FirstName = customerModel.FirstName,
                LastName = customerModel.LastName,
                PrimaryAddress = SerializeCustomerAddress(customerModel.PrimaryAddress)
            };
        }

        /// <summary>
        /// Serializes CustomerAddress (data model) to CustomerAddressModel (view model)
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static CustomerAddressModel SerializeCustomerAddress(CustomerAddress address)
        {
            return new CustomerAddressModel
            {
                Id = address.Id,
                AddressLine1 = address.AddressLine1,
                AddressLine2 = address.AddressLine2,
                City = address.City,
                Country = address.Country,
                PostalCode = address.PostalCode,
                CreatedOn = address.CreatedOn,
                UpdatedOn = address.UpdatedOn
            };
        }

        /// <summary>
        /// Serializes CustomerAddressModel (view model) to CustomerAddress (data model)
        /// </summary>
        /// <param name="addressModel"></param>
        /// <returns></returns>
        public static CustomerAddress SerializeCustomerAddress(CustomerAddressModel addressModel)
        {
            return new CustomerAddress
            {
                Id = addressModel.Id,
                AddressLine1 = addressModel.AddressLine1,
                AddressLine2 = addressModel.AddressLine2,
                City = addressModel.City,
                Country = addressModel.Country,
                PostalCode = addressModel.PostalCode,
                CreatedOn = addressModel.CreatedOn,
                UpdatedOn = addressModel.UpdatedOn
            };
        }
    }
}