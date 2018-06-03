using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Learn.Quartz.NET.ConsoleApp
{
    public class HelloJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            // add comments
            await Console.Out.WriteLineAsync("Greetings from HelloJob!");
        }
    }
}
