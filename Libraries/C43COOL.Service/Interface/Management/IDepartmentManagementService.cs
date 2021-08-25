using C43COOL.Models.Department.Management;
using C43COOL.Models.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C43COOL.Service.Interface.Management
{
    public interface IDepartmentManagementService
    {
        /// <summary>
        /// 部门查询接口
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PagedViewModel> Query(DepartmentQueryModel request);
        /// <summary>
        /// 新增部门
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task Create(DepartmentCreateModel model);
        /// <summary>
        /// 修改部门
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task Modify(DepartmentModifyModel model);
        /// <summary>
        /// 批量删除部门
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task Delete(string[] ids);

        /// <summary>
        /// 根据ID获取详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<DepartmentInfoModel> Get(string Id);
    }
}
