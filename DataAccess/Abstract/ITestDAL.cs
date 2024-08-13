using Core.DataAccess;
using Core.Helpers.PaginationHelper.EntityFramework;
using Entities.Concrete;

namespace DataAccess.Abstract
{
    public interface ITestDAL : IRepositoryBase<Test>
    {
        Task<PagedList<Test>> GetAllByPage(int page, int itemsPerPage);
    }
}
