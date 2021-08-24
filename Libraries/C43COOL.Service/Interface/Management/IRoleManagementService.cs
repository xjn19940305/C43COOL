using C43COOL.Models.Paging;
using C43COOL.Models.Role.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C43COOL.Service.Interface.Management
{
    public interface IRoleManagementService
    {
        /// <summary>
        /// 查询角色接口
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PagedViewModel> Query(RoleQueryRequest request);
        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task Create(RoleCreateModel model);
        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task Modify(RoleModifyModel model);
        /// <summary>
        /// 批量删除角色
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task Delete(string[] ids);
        /// <summary>
        /// 给角色授权模块
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task AuthorizeRole(RoleMenuAuthorizeModel model);
        /// <summary>
        /// 根据角色ID获得已授权的模块
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        Task<string[]> GetRoleModules(string RoleId);
        /// <summary>
        /// 根据ID获取角色详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<RoleInfoModel> Get(string Id);
    }
}
