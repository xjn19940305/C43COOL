using C43COOL.Redis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace C43COOL.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(GroupName = "front")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> logger;
        private readonly RedisCache cache;

        public TestController(
            ILogger<TestController> logger,
            RedisCache cache
            )
        {
            this.logger = logger;
            this.cache = cache;
        }
        #region 分布式锁
        /// <summary>
        /// 分布式加锁
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> TestRedisLock()
        {
            var aa = await cache.LockAsync("aa", "aaaaa", TimeSpan.FromMinutes(1));
            if (aa)
                logger.LogInformation("分布式锁创建成功");
            else
                logger.LogInformation("锁已被占用");
            return Ok();
        }
        /// <summary>
        /// 分布式解锁
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> TestRedisUnLock()
        {
            var aa = await cache.UnLockASync("aa", "aaaaa");
            if (aa)
                logger.LogInformation("分布式锁解除成功");
            else
                logger.LogInformation("分布式锁解除失败");
            return Ok();
        }
        #endregion
        #region 消息订阅
        /// <summary>
        /// 往指定通道发送消息
        /// </summary>
        /// <param name="ChannelName"></param>
        /// <param name="Content"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Pub([FromQuery] string ChannelName, [FromQuery] string Content)
        {
            await cache.PublishAsync(ChannelName, Content);
            return Ok();
        }
        /// <summary>
        /// 订阅对应通道的内容
        /// </summary>
        /// <param name="ChannelName"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> SubscribeAsync([FromQuery] string ChannelName)
        {
            await cache.SubscribeAsync(ChannelName, (channel, content) =>
            {
                logger.LogInformation($"通道名称:{channel} 内容:{content}");
            });
            return Ok();
        }
        /// <summary>
        /// 取消订阅对应通道的内容
        /// </summary>
        /// <param name="ChannelName"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> UnsubscribeAsync([FromQuery] string ChannelName)
        {
            await cache.UnSubscribeAsync(ChannelName);
            return Ok();
        }
        #endregion
    }
}
