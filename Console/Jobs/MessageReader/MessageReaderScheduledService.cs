using Microsoft.Extensions.Hosting;
using Quartz;

namespace Console.Jobs.MessageReader
{
    public class MessageReaderScheduledService : BackgroundService
    {
        private readonly Lazy<Task<IScheduler>> _scheduler;

        public MessageReaderScheduledService(ISchedulerFactory factory) => _scheduler =
            new Lazy<Task<IScheduler>>(() => factory.GetScheduler());

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var scheduler = await _scheduler.Value;

            var job = JobBuilder.Create<MessageReaderJob>().WithIdentity("MessageReader.Job").Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity("MessageReader.Trigger")
                .StartNow()
                .WithSimpleSchedule(
                    s => s
                    .WithIntervalInMinutes(1)
                        .RepeatForever())
                .Build();

            await scheduler.ScheduleJob(job, trigger);

            await scheduler.Start(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            var scheduler = await _scheduler.Value;
            await scheduler.Shutdown(cancellationToken);
            System.Console.WriteLine("A scheduler has been stopped.");
        }
    }
}
