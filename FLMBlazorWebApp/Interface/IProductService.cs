using FLMBlazorWebApp.Repositories;

namespace FLMBlazorWebApp.Interface
{
    public interface IProductService
    {
        List<Product> GetProducts();
        Product GetById(int productId);
        string Delete(int productId);
        Product SaveOrUpdate(Product product);
    }
}
