using TestCaseB.Utility;

namespace TestCaseB.Services.Interfaces
{
    public interface IUriService
    {
        public Uri GetPageUri(PaginationFilter filter, string route);
    }
}
