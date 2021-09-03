using C43COOL.Models.Article.Management;
using C43COOL.Service.Interface.Management;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace C43COOL.Api.Management
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "manager")]
    [Authorize]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleMangementService service;

        public ArticleController(
            IArticleMangementService service
            )
        {
            this.service = service;
        }
        /// <summary>
        /// 获取文章分页列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Query([FromQuery] ArticleRequest request)
        {
            var data = await service.Query(request);
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
            var data = await service.Get(Id);
            return Ok(data);
        }

        /// <summary>
        /// 新增文章
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ArticleCreateModel dto)
        {
            await service.Create(dto);
            return Ok();
        }
        /// <summary>
        /// 更新文章
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ArticleModifyModel dto)
        {
            await service.Update(dto);
            return Ok();
        }
        /// <summary>
        /// 删除文章
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] string[] ids)
        {
            await service.Delete(ids);
            return Ok();
        }
    }
}
