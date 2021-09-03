using C43COOL.Models.File;
using C43COOL.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace C43COOL.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileService fileservice;

        public FileController(
            IFileService fileservice
            )
        {
            this.fileservice = fileservice;
        }
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="name">文件名称</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateFile([FromQuery] string name)
        {
            var request = new CreateFileRequest()
            {
                Name = name,
                FileStream = Request.Body,
                FileType = Request.ContentType
            };
            var result = await fileservice.CreateFileAsync(request);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        /// <summary>
        /// 获取文件信息
        /// </summary>
        /// <param name="id">文件Id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFile([FromRoute] string id)
        {
            var result = await fileservice.GetFile(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        /// <summary>
        /// 获取文件内容
        /// </summary>
        /// <param name="id">文件Id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFileContent([FromRoute] string id)
        {
            var result = await fileservice.GetFile(id);
            if (result != null)
            {
                var stream = await fileservice.OpenAsRead(result.Path);
                if (stream != null)
                {
                    return File(stream, result.Type ?? "", true);
                }
            }
            return NotFound();
        }
    }
}
