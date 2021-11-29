using C43COOL.Models.Category.Management;
using C43COOL.Service.Interface.Management;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace C43COOL.Api.Management
{
    [Route("api/mgt/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "manager")]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryMangementService categoryService;

        public CategoryController(
            ICategoryMangementService categoryService
            )
        {
            this.categoryService = categoryService;
        }
        /// <summary>
        /// 获取树分类
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Query([FromQuery] CategoryRequest request)
        {
            var data = await categoryService.Query(request);
            return Ok(data);
        }
        /// <summary>
        /// 获取分类信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        public async Task<IActionResult> Get([FromRoute] string Id)
        {
            var data = await categoryService.Get(Id);
            return Ok(data);
        }
        /// <summary>
        /// 新增分类
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryCreateModel dto)
        {
            await categoryService.Create(dto);
            return Ok();
        }
        /// <summary>
        /// 更新分类
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] CategoryModifyModel dto)
        {
            await categoryService.Update(dto);
            return Ok();
        }
        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] string[] ids)
        {
            await categoryService.Delete(ids);
            return Ok();
        }
    }
}