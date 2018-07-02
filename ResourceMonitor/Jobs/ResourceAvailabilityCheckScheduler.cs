namespace ResourceMonitor.Jobs
{
    using Quartz;
    using Quartz.Impl;

    public class ResourceAvailabilityCheckScheduler
    {
        public static async void Start()
        {
            var scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            await scheduler.Start();

            var job = new JobDetailImpl("1Job", null, typeof(ResourceAvailabilityCheckJob));

            var trigger = TriggerBuilder.Create()
                                        .WithIdentity("trigger1", "group1")
                                        .StartNow()
                                        .WithSimpleSchedule(
                                            builder => builder
                                                .WithIntervalInMinutes(1)
                                                .RepeatForever())
                                        .Build();                             

            await scheduler.ScheduleJob(job, trigger);        
        }
    }
}