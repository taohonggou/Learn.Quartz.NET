using System;
using System.Collections.Specialized;
using System.IO;
using System.Threading.Tasks;
using log4net;
using log4net.Config;
using Quartz;
using Quartz.Impl;
using Quartz.Logging;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", ConfigFileExtension = "config", Watch = true)]
namespace Learn.Quartz.NET.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //LogProvider.SetCurrentLogProvider(new ConsoleLogProvider());
            // test modify
            // continue test pull request
            #region 这种方式也可以配置log4net
            //var repository = LogManager.CreateRepository("ConsoleApp");
            //XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));
            //var logger= LogManager.GetLogger("ConsoleApp", typeof(Program)); 
            #endregion

            var logger = LogManager.GetLogger(typeof(Program));
            logger.Info("test");
            logger.Error("error");

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

                await scheduler.Start();


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
