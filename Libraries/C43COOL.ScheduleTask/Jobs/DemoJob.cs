using C43COOL.Domain;
using C43COOL.Enum.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C43COOL.ScheduleTask.Jobs
{
    public class DemoJob : IJob
    {
        private readonly IServiceProvider _provider;
        private readonly ILogger<DemoJob> logger;

        public DemoJob(
            IServiceProvider provider,
            ILogger<DemoJob> logger)
        {
            this._provider = provider;
            this.logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            using (var scope = _provider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<C43DbContext>();
                var JsonStr = context.JobDetail.JobDataMap.GetString("JSON");
                var result = new
                {
                    JobId = string.Empty
                };
                var JsonData = JsonConvert.DeserializeAnonymousType(JsonStr, result);
                //拿到任务
                var job = await dbContext.JobSchedule.FirstOrDefaultAsync(x => x.Id == JsonData.JobId);
                //将任务变为执行中
                job.JobStatu = JobStatus.Running;
                await dbContext.SaveChangesAsync();
                try
                {
                    //执行逻辑
                    while (true)
                    {
                        logger.LogInformation("我在测试 ");
                        await Task.Delay(1500);
                    }
                }
                catch (Exception ex)
                {
                    job.ErrorMessage = ex.Message;
                    logger.LogInformation($"{DateTime.UtcNow} {ex.Message}");
                }
                finally
                {
                    //任务完成状态改变
                    job.JobStatu = JobStatus.Complete;
                    await dbContext.SaveChangesAsync();
                    logger.LogInformation($"{DateTime.UtcNow} 本次任务ID:{job.Id} 任务完成!");
                }
            }
        }
    }
}
