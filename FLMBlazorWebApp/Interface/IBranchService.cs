using FLMBlazorWebApp.Repositories;

namespace FLMBlazorWebApp.Interface
{
    public interface IBranchService
    {
        List<Branch> GetBranches();
        Branch GetById(int branchId);
        string Delete(int branchId);
        Branch SaveOrUpdate(Branch branch);
    }
}
