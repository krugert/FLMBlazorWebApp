using Dapper;
using FLMBlazorWebApp.Components.Pages;
using FLMBlazorWebApp.Interface;
using Model = FLMBlazorWebApp.Repositories;
using Microsoft.AspNetCore.Components;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Remoting;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using FLMBlazorWebApp.Repositories;

namespace FLMBlazorWebApp.Service
{
    public class ProductService : ComponentBase, IProductService
    {
        List<Model.Product> _products = new List<Model.Product>();
        Model.Product _product = new Model.Product();
        int oldId { get; set; }

        public IConfiguration _Configuration { get; }
        public string _connectionString = "";

        public ProductService(IConfiguration configuration)
        {
            _Configuration = configuration;
            _connectionString = _Configuration.GetConnectionString("FLMDbConnection");
        }

        public List<Model.Product> GetProducts()
        {
            _products = new List<Model.Product>();
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                var products = connection.Query<Model.Product>("SELECT * FROM Product").ToList();

                if (products != null && products.Count() > 0)
                {
                    _products = products;
                }
            }
            return _products;
        }

        public string Delete(int productId)
        {
            string message = "Failed";

            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                _product = new Model.Product()
                {
                    Id = productId
                };

                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                var products = connection.Query<Model.Product>("DELETE FROM Product WHERE Id=@ProductId", new { productId });

                message = "Deleted";
            }
            return message;
        }

        public Model.Product GetById(int productId)
        {
            oldId = productId;
            _product = new Model.Product();
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                var products = connection.Query<Model.Product>("SELECT * FROM Product WHERE ID = " + productId);

                if (products != null && products.Count() > 0)
                {
                    _product = products.FirstOrDefault();
                }
            }
            return _product;
        }

        public Model.Product SaveOrUpdate(Model.Product product)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                _product = new Model.Product()
                {
                    Id = product.Id,
                    Name = product.Name,
                    WeightedItem = product.WeightedItem,
                    SuggestedSellingPrice = product.SuggestedSellingPrice
                };

                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                int operationType = Convert.ToInt32(oldId == 0 ? Model.OperationType.Insert : Model.OperationType.Update);

                IEnumerable<Model.Product> products;

                if (operationType == (int)Model.OperationType.Insert)
                {
                    products = connection.Query<Model.Product>("INSERT INTO Product(Id, Name, WeightedItem, SuggestedSellingPrice) VALUES(@ProductId, @Name, @WeightedItem, @SuggestedSellingPrice)", new { @ProductId = product.Id, @Name = product.Name, @WeightedItem = product.WeightedItem, @SuggestedSellingPrice = product.SuggestedSellingPrice });
                }
                else
                {
                    products = connection.Query<Model.Product>("UPDATE Product SET Id = @ProductId, Name = @Name, WeightedItem = @WeightedItem, SuggestedSellingPrice = @SuggestedSellingPrice WHERE Id = @Id", new { @ProductId = product.Id, @Name = product.Name, @WeightedItem = product.WeightedItem, @SuggestedSellingPrice = product.SuggestedSellingPrice, @Id = oldId });
                }

                if (products != null && products.Count() > 0)
                {
                    _product = products.FirstOrDefault();
                }
            }
            return _product;
        }

        private DynamicParameters SetParameters(Model.Product product, int operationType)
        {
            DynamicParameters obj = new DynamicParameters();
            obj.Add("@ProductId", product.Id);
            obj.Add("@Name", product.Name);
            obj.Add("@WeightedItem", product.WeightedItem);
            obj.Add("@SuggestedSellingPrice", product.SuggestedSellingPrice);
            obj.Add("@OperationType", operationType);

            return obj;
        }
    }
}
