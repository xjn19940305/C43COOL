using C43COOL.Models.Modules.Management;
using C43COOL.Models.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C43COOL.Service.Interface.Management
{
    public interface IModulesManagementService
    {
        /// <summary>
        /// 模块查询接口
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PagedViewModel> Query(ModulesQueryModel request);
        /// <summary>
        /// 新增模块
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task Create(ModulesCreateModel model);
        /// <summary>
        /// 修改模块
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task Modify(ModulesModifyModel model);
        /// <summary>
        /// 批量删除模块
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task Delete(string[] ids);

        /// <summary>
        /// 根据ID获取详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<ModulesInfoModel> Get(string Id);
    }
}
