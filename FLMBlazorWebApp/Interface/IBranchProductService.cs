using Model = FLMBlazorWebApp.Repositories;
using System.Reflection;

namespace FLMBlazorWebApp.Interface
{
    public interface IBranchProductService
    {
        List<Model.Branch> GetBranches();
        List<Model.Product> GetProducts();
        Model.BranchProduct SaveOrUpdate(Model.BranchProduct branchProduct);
        List<Model.BranchProduct> GetBranchProducts();
        List<Model.Product> GetProductsByBranchId(int branchId);

        /*
        BranchProduct GetById(int branchProductId);
        string Delete(int branchProductId);
        BranchProduct SaveOrUpdate(BranchProduct branchProduct);
        */
    }
}
