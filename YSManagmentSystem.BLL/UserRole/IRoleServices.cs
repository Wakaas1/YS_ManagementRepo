using YSManagmentSystem.Domain.User;

namespace YSManagmentSystem.BLL.UserRole
{
    public interface IRoleServices
    {
        int AddRole(AppRole model);
        int AddUserRole(int userId, int roleId);
        List<RoleEdit> GetAllRole(int uId);
        IEnumerable<UserDetail> GetAllUsers(UserDetail model);
        void RemoveRole(int userId, int roleId);
    }
}