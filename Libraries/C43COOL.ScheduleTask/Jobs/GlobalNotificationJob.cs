using AutoMapper;
using C43COOL.Domain;
using C43COOL.Enum.Base;
using C43COOL.Models.JobModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C43COOL.ScheduleTask.Jobs
{
    /// <summary>
    /// 全局调度任务器 抓取JobSchedule中的数据
    /// </summary>
    [DisallowConcurrentExecution]
    public class GlobalNotificationJob : IJob
    {
        private readonly ILogger<GlobalNotificationJob> logger;
        private readonly IServiceProvider _provider;
        private readonly ScheduleService scheduleService;
        private readonly IMapper mapper;

        public GlobalNotificationJob(
            ILogger<GlobalNotificationJob> logger,
            IServiceProvider provider,
            ScheduleService scheduleService,
            IMapper mapper
            )
        {
            this.logger = logger;
            this._provider = provider;
            this.scheduleService = scheduleService;
            this.mapper = mapper;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            using (var scope = _provider.CreateScope())
            {
                var DbContext = scope.ServiceProvider.GetService<C43DbContext>();
                var TaskList = await DbContext.JobSchedule.ToListAsync();
                logger.LogInformation($"{DateTime.UtcNow} 当前初始化队列任务:{TaskList.Count(x => x.JobStatu == JobStatus.Init)}");
                logger.LogInformation($"{DateTime.UtcNow} 等待执行队列任务:{TaskList.Count(x => x.JobStatu == JobStatus.Wait)}");
                logger.LogInformation($"{DateTime.UtcNow} 正在执行队列任务:{TaskList.Count(x => x.JobStatu == JobStatus.Running)}");
                logger.LogInformation($"{DateTime.UtcNow} 已完成队列任务:{TaskList.Count(x => x.JobStatu == JobStatus.Complete)}");
                var InitList = TaskList.Where(x => x.JobStatu == JobStatus.Init).ToList();
                try
                {
                    foreach (var x in InitList)
                    {
                        var model = mapper.Map<JobScheduleDTO>(x);
                        // 启动任务
                        await scheduleService.StartTask(model);
                        x.JobStatu = JobStatus.Wait;
                        await DbContext.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {
                    logger.LogInformation($"{DateTime.UtcNow} 全局调度发生错误!ex:{ex.Message}");
                }
            }

        }
    }
}
