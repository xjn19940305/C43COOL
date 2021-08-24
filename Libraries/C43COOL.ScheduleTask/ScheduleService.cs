using AutoMapper;
using C43COOL.Enum.Base;
using C43COOL.Models.JobModel;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace C43COOL.ScheduleTask
{
    public class ScheduleService
    {
        private readonly ISchedulerFactory schedulerFactory;
        private readonly IJobFactory jobFactory;
        private readonly IMapper mapper;
        private readonly ILogger<ScheduleService> logger;

        public ScheduleService(
            ISchedulerFactory schedulerFactory,
            IJobFactory jobFactory,
            IMapper mapper,
            ILogger<ScheduleService> logger
            )
        {
            this.schedulerFactory = schedulerFactory;
            this.jobFactory = jobFactory;
            this.mapper = mapper;
            this.logger = logger;
        }
        /// <summary>
        /// 开始执行任务
        /// </summary>
        /// <param name="Schedule"></param>
        /// <returns></returns>
        public async Task StartTask(JobScheduleDTO Job)
        {
            var t = Assembly.GetExecutingAssembly();
            Job.JobType = t.GetType($"{t.GetName().Name}.Jobs.{Job.AssemblyName}");
            var Scheduler = await schedulerFactory.GetScheduler();
            Scheduler.JobFactory = jobFactory;
            var jobData = new JobDataMap { new KeyValuePair<string, object>("JSON", Job?.Params ?? string.Empty) };
            await Scheduler.ScheduleJob(CreateJob(Job, jobData), CreateTrigger(Job));
            await Scheduler.Start();
        }
        /// <summary>
        /// 停止任务
        /// </summary>
        /// <param name="Schedule"></param>
        /// <returns></returns>
        public async Task StopTask(JobScheduleDTO Job)
        {
            var Scheduler = await schedulerFactory.GetScheduler();
            await Scheduler?.Shutdown();
            Job.JobStatu = JobStatus.Stopped;
        }
        /// <summary>
        /// 取消任务
        /// </summary>
        /// <param name="Schedule"></param>
        /// <returns></returns>
        public async Task Cancel(JobScheduleDTO Job)
        {
            var Scheduler = await schedulerFactory.GetScheduler();
            TriggerKey triggerKey = new TriggerKey(Job.Id);
            // 停止触发器
            await Scheduler.PauseTrigger(triggerKey);
            // 移除触发器
            await Scheduler.UnscheduleJob(triggerKey);
            // 删除任务
            await Scheduler.DeleteJob(new JobKey(Job.Id));
        }
     
        private IJobDetail CreateJob(JobScheduleDTO Job, JobDataMap data = null)
        {
            var jobType = Job.JobType;
            return JobBuilder
                .Create(jobType)
                .WithIdentity(Job.Id)
                .WithDescription(jobType.Name)
                .SetJobData(data)
                .Build();
        }

        private ITrigger CreateTrigger(JobScheduleDTO Job)
        {
            var trig = TriggerBuilder
                .Create()
                .WithIdentity($"{Job.JobName}.trigger");
            logger.LogInformation($"本地时间:{DateTime.Now}");
            logger.LogInformation($"本地UTC时间:{DateTime.UtcNow}");
            logger.LogInformation($"任务执行时间:{Job.StartNow.ToString()}");
            //判断是用 cron触发器还是simple
            //目前simple只支持立即执行以后还可以扩展
            if (Job.TriggerType == TriggerType.Cron)
                trig.WithCronSchedule(Job.CronExpression);
            else if (Job.TriggerType == TriggerType.Simple)
                trig.StartAt(new DateTimeOffset(Job.StartNow.Value, TimeSpan.Zero));
            return trig.Build();
        }
    }
}
