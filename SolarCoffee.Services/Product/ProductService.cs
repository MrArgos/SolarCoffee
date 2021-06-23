using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SolarCoffee.Data;
using SolarCoffee.Data.Models;

namespace SolarCoffee.Services.Product
{
    public class ProductService : IProductService
    {
        private readonly SolarDbContext _db;

        public ProductService(SolarDbContext dbContext)
        {
            _db = dbContext;
        }
        
        /// <summary>
        /// Retrieves all Products from database
        /// </summary>
        /// <returns>List of Products</returns>
        public List<Data.Models.Product> GetAllProducts()
        {
            return _db.Products.ToList();
        }

        /// <summary>
        /// Retrieves a Product by primary key
        /// </summary>
        /// <param name="id"> Primary key to search in database</param>
        /// <returns>Product with given id</returns>
        public Data.Models.Product GetProductById(int id)
        {
            return _db.Products.Find(id);
        }

        /// <summary>
        /// Adds a new product to the database
        /// </summary>
        /// <param name="product"> product object - Data.Models.Product</param>
        /// <returns>ServiceResponse{Product, Time, Message, IsSuccess}</returns>
        public ServiceResponse<Data.Models.Product> CreateProduct(Data.Models.Product product)
        {
            try
            {
                _db.Products.Add(product);
     
                var newInventory = new ProductInventory
                {
                    Product = product,
                    QuantityOnHand = 0,
                    IdealQuantity = 10
                };
                _db.ProductInventories.Add(newInventory);
                 
                _db.SaveChanges();
                
                return new ServiceResponse<Data.Models.Product>
                {
                    Data = product,
                    Time = DateTime.UtcNow,
                    Message = "Saved new Product",
                    IsSuccess = true
                };
            }
            catch (Exception e)
            {
                return new ServiceResponse<Data.Models.Product>
                {
                    Data = product,
                    Time = DateTime.UtcNow,
                    Message = e.StackTrace,
                    IsSuccess = false
                };
            }
        }
 
        /// <summary>
        /// Archives a Product by setting boolean IsArchived to true
        /// </summary>
        /// <param name="id">Id of product to be Archived</param>
        /// <returns>ServiceResponse{Product, Time, Message, IsSuccess}</returns>
        /// <exception cref="NotImplementedException"></exception>
        public ServiceResponse<Data.Models.Product> ArchiveProduct(int id)
        {
            try
            {
                var productToArchive = _db.Products.Find(id);
                productToArchive.IsArchived = true;
                _db.SaveChanges();
                
                return new ServiceResponse<Data.Models.Product>
                {
                    Data = productToArchive,
                    Time = DateTime.UtcNow,
                    Message = "Archived Product",
                    IsSuccess = true
                };
            }
            catch (Exception e)
            {
                return new ServiceResponse<Data.Models.Product>
                {
                    Data = null,
                    Time = DateTime.UtcNow,
                    Message = e.StackTrace,
                    IsSuccess = false
                };
            }
        }
    }
}