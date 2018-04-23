using System;
using System.Collections.Specialized;
using System.Threading.Tasks;

using Quartz;
using Quartz.Impl;
using Quartz.Logging;

namespace Learn.Quartz.NET.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            LogProvider.SetCurrentLogProvider(new ConsoleLogProvider());

            RunProgram().GetAwaiter().GetResult();

            Console.WriteLine("Press any key to close the application");
            Console.ReadKey();
        }

        private static async Task RunProgram()
        {
            try
            {
                

                NameValueCollection props = new NameValueCollection
                {
                    { "quartz.serializer.type", "binary" }
                };
                StdSchedulerFactory factory = new StdSchedulerFactory(props);
                IScheduler scheduler = await factory.GetScheduler();

                await scheduler.Start();

                IJobDetail job = JobBuilder.Create<HelloJob>()
                    .WithIdentity("job1", "group1").Build();

                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity("trigger1", "group1")
                    .StartNow()
                    .WithSimpleSchedule(x =>
                        x.WithIntervalInSeconds(2)
                        .RepeatForever())
                    .Build();

                await scheduler.ScheduleJob(job, trigger);




                //await Task.Delay(TimeSpan.FromSeconds(30));

                //await scheduler.Shutdown();
            }
            catch (Exception ex)
            {
                await Console.Error.WriteLineAsync(ex.ToString());

            }
        }
    }
}
