using Core.Utilities.Results.Abstract;
using Entities.DTOs.RoleDTOs;

namespace Business.Abstract
{
    public interface IRoleService
    {
        Task<IResult> CreateRoleAsync(string roleName);
        Task<IResult> DeleteRoleAsync(string id);
        Task<IResult> UpdateRoleAsync(string roleId, string roleName);
        IDataResult<List<GetRoleDTO>> GetAllRoles();
    }
}
