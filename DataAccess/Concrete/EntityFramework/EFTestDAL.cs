using Core.DataAccess.EntityFramework;
using Core.Helpers.PaginationHelper.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete.EntityFramework
{
    public class EFTestDAL : EFRepositoryBase<Test, AppDbContext>, ITestDAL
    {
        public async Task<PagedList<Test>> GetAllByPage(int page, int itemsPerPage)
        {
            await using var context = new AppDbContext();

            var query = context.Set<Test>().AsNoTracking().ToList();

            var response = query
                .Skip((page - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .ToList();

            int count = query.Count();
            return new PagedList<Test>(response, count, page, itemsPerPage);
        }
    }
}
