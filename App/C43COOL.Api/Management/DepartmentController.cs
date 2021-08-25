using C43COOL.Models.Department.Management;
using C43COOL.Service.Interface.Management;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace C43COOL.Api.Management
{
    [ApiController]
    [Route("api/mgt/[controller]/[action]")]
    [ApiExplorerSettings(GroupName = "manager")]
    [Authorize]
    public class DepartmentController:ControllerBase
    {
        private readonly IDepartmentManagementService service;

        public DepartmentController(
            IDepartmentManagementService service
            )
        {
            this.service = service;
        }
        /// <summary>
        /// 部门列表查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Query([FromQuery] DepartmentQueryModel request)
        {
            return Ok(await service.Query(request));
        }
        /// <summary>
        /// 新增部门
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] DepartmentCreateModel model)
        {
            await service.Create(model);
            return Ok();
        }
        /// <summary>
        /// 修改部门
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Modify([FromBody] DepartmentModifyModel model)
        {
            await service.Modify(model);
            return Ok();
        }
        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] string[] ids)
        {
            await service.Delete(ids);
            return Ok();
        }

        /// <summary>
        /// 根据ID获取详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string Id)
        {
            return Ok(await service.Get(Id));
        }
    }
}
