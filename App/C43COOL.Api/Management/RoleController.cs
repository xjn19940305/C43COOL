using C43COOL.Models.Role.Management;
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
    public class RoleController : ControllerBase
    {
        private readonly IRoleManagementService service;

        public RoleController(
            IRoleManagementService service
            )
        {
            this.service = service;
        }

        /// <summary>
        /// 角色列表查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Query([FromQuery] RoleQueryRequest request)
        {
            return Ok(await service.Query(request));
        }
        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RoleCreateModel model)
        {
            await service.Create(model);
            return Ok();
        }
        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Modify([FromBody] RoleModifyModel model)
        {
            await service.Modify(model);
            return Ok();
        }
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] string[] ids)
        {
            await service.Delete(ids);
            return Ok();
        }
        /// <summary>
        /// 根据角色ID拿到角色对应的模块权限
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetRoleMenus([FromQuery] string RoleId)
        {
            return Ok(await service.GetRoleModules(RoleId));
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
        /// <summary>
        /// 给角色授权对应模块权限
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> AuthorizeRoleMenu([FromBody] RoleMenuAuthorizeModel model)
        {
            await service.AuthorizeRole(model);
            return Ok();
        }
    }
}
