using FLMBlazorWebApp.Interface;
using System.Data.SqlClient;
using System.Data;
using Model = FLMBlazorWebApp.Repositories;
using Dapper;

namespace FLMBlazorWebApp.Service
{
    public class BranchProductService : IBranchProductService
    {
        List<Model.Branch> _branches = new List<Model.Branch>();
        Model.Branch _branch = new Model.Branch();
        List<Model.Product> _products = new List<Model.Product>();
        Model.Product _product = new Model.Product();
        Model.BranchProduct _branchProduct = new Model.BranchProduct();
        List<Model.BranchProduct> _branchProducts = new List<Model.BranchProduct>();
        int oldId { get; set; }

        public IConfiguration _Configuration { get; }
        public string _connectionString = "";

        public BranchProductService(IConfiguration configuration)
        {
            _Configuration = configuration;
            _connectionString = _Configuration.GetConnectionString("FLMDbConnection");
        }

        public List<Model.Branch> GetBranches()
        {
            return new List<Model.Branch>();
        }

        public List<Model.Product> GetProducts()
        {
            return new List<Model.Product>();
        }

        public List<Model.BranchProduct> GetBranchProducts()
        {
            _branchProducts = new List<Model.BranchProduct>();
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                var branchProducts = connection.Query<Model.BranchProduct>("SELECT * FROM BranchProduct").ToList();

                if (branchProducts != null && branchProducts.Count() > 0)
                {
                    _branchProducts = branchProducts;
                }
            }
            return _branchProducts;
        }

        public List<Model.Product> GetProductsByBranchId(int branchId)
        {
            _products = new List<Model.Product>();
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                var products = connection.Query<Model.Product>("SELECT p.Name, p.WeightedItem, p.SuggestedSellingPrice FROM BranchProduct bp INNER JOIN Product p ON bp.ProductId = p.Id INNER JOIN Branch b ON bp.BranchId = b.Id AND b.Id = @BranchId", new { branchId }).ToList();

                if (products != null && products.Count() > 0)
                {
                    _products = products;
                }
            }
            return _products;
        }

        public Model.BranchProduct SaveOrUpdate(Model.BranchProduct branchProduct)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                _branchProduct = new Model.BranchProduct()
                {
                    BranchId = branchProduct.BranchId,
                    ProductId = branchProduct.ProductId
                };

                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                int operationType = Convert.ToInt32(oldId == 0 ? Model.OperationType.Insert : Model.OperationType.Update);

                IEnumerable<Model.BranchProduct> branchProducts;

                if (operationType == (int)Model.OperationType.Insert)
                {
                    branchProducts = connection.Query<Model.BranchProduct>("INSERT INTO BranchProduct(BranchId, ProductId) VALUES(@BranchId, @ProductId)", new { @BranchId = branchProduct.BranchId, @ProductId = branchProduct.ProductId });
                }
                else
                {
                    branchProducts = connection.Query<Model.BranchProduct>("UPDATE BranchProduct SET BranchId = @BranchId, ProductId = @ProductIds WHERE BranchProductId = @Id", new { @BranchId = branchProduct.BranchId, @ProductId = branchProduct.ProductId, @Id = oldId });
                }

                if (branchProducts != null && branchProducts.Count() > 0)
                {
                    _branchProduct = branchProducts.FirstOrDefault();
                }
            }
            return _branchProduct;
        }
    }
}
