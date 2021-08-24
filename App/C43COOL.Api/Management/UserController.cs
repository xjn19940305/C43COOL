using C43COOL.Models.User.Management;
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
    public class UserController : ControllerBase
    {
        private readonly IUserManagementService service;

        public UserController(
            IUserManagementService service
            )
        {
            this.service = service;
        }

        /// <summary>
        /// 用户列表查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Query([FromQuery] UserListQueryModel request)
        {
            return Ok(await service.Query(request));
        }
        /// <summary>
        /// 用户添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserCreateModel model)
        {
            await service.Create(model);
            return Ok();
        }
        /// <summary>
        /// 用户修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Modify([FromBody] UserModifyModel model)
        {
            await service.Modify(model);
            return Ok();
        }
        /// <summary>
        /// 用户删除
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] string UserId)
        {
            await service.Delete(UserId);
            return Ok();
        }
        /// <summary>
        /// 批量启用禁用用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Enabled([FromBody] UserEnabledModel model)
        {
            await service.Enabled(model);
            return Ok();
        }
        /// <summary>
        /// 根据用户ID获得详情
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string UserId)
        {
            return Ok(await service.Get(UserId));
        }
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SignInWithPassword([FromBody] LoginSignInWithPasswordModel model)
        {
            return Ok(await service.SignInWithPassword(model));
        }
        /// <summary>
        /// 根据Token获取用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserInfo()
        {
            return Ok(await service.GetUserInfo());
        }
    }
}
