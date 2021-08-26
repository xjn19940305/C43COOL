using C43COOL.Models.Modules.Management;
using C43COOL.Models.Paging;
using C43COOL.Models.User.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C43COOL.Service.Interface.Management
{
    public interface IUserManagementService
    {
        /// <summary>
        /// 获取用户中心列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PagedViewModel> Query(UserListQueryModel request);
        /// <summary>
        /// 批量禁用或者启用
        /// </summary>
        /// <returns></returns>
        Task Enabled(UserEnabledModel model);
        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task Create(UserCreateModel model);
        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task Modify(UserModifyModel model);
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        Task Delete(string UserId);
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<string> SignInWithPassword(LoginSignInWithPasswordModel model);
        /// <summary>
        /// 根据Token获取用户信息
        /// </summary>
        /// <returns></returns>
        Task<UserInfoModel> GetUserInfo();

        /// <summary>
        /// 根据ID获取用户详情
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        Task<UserInfoModel> Get(string UserId);
        /// <summary>
        /// 根据登录用户获取对应的菜单权限
        /// </summary>
        /// <returns></returns>
        Task<List<ModuleViewModel>> GetModules();

        /// <summary>
        /// 给用户绑定角色
        /// </summary>
        /// <param name="RoleIds"></param>
        /// <returns></returns>
        Task BindRole(UserBindModel model);
    }
}
