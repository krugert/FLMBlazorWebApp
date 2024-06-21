using FLMBlazorWebApp.Repositories;

namespace FLMBlazorWebApp.Interface
{
    public interface IReportService
    {
        public Task<IEnumerable<Report>> GetProductsByBranch();
    }
}
