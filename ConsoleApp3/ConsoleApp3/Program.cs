using com.lightstreamer.client;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            Console.WriteLine("Hello World! " + args[0]);

            var ls1 = new MyConnector("https://push.lightstreamer.com", "DEMO");
            var ls2 = new MyConnector("https://push.lightstreamer.com", "WELCOME");
            var ls3 = new MyConnector("http://push.lightstreamer.com", "MARKETDEPTH");

            if (args[0].StartsWith("yes"))
            {
                Environment.SetEnvironmentVariable("io.netty.allocator.maxOrder", "1");
                Environment.SetEnvironmentVariable("io.netty.allocator.type", "unpooled");

                System.Console.WriteLine("Tuned DotNetty.");
            }

            while (true)
            {
                var connectTasks = new List<Task>();
                var subTasks = new List<Task>();

                connectTasks.Add(ls1.Connect(new CancellationTokenSource(7000).Token));
                connectTasks.Add(ls2.Connect(new CancellationTokenSource(7000).Token));
                connectTasks.Add(ls3.Connect(new CancellationTokenSource(7000).Token));

                var allTasksAwaiter = Task.WhenAll(connectTasks);

                try
                {
                    allTasksAwaiter.Wait();
                }
                catch (Exception e)
                {
                System.Console.WriteLine("ERRORE ********** " + e.Message);
                    
                }

                subTasks.Add(ls1.Subscribe("MERGE", "QUOTE_ADAPTER", new string[3] { "item1", "item2", "item16" }, new string[3] { "last_price", "time", "stock_name" }, new CancellationTokenSource(5000).Token));
                subTasks.Add(ls3.Subscribe("COMMAND", null, new string[1] { "AXY_COMP_BUYSIDE" }, new string[3] { "command", "key", "qty" }, new CancellationTokenSource(5000).Token));

                var allSubTasksAwaiter = Task.WhenAll(subTasks);

                try
                {
                    allSubTasksAwaiter.Wait();
                }
                catch (Exception e)
                {
                    System.Console.WriteLine("ERRORE ********** " + e.Message);

                }

                ls1.disconnect();
                ls2.disconnect();
                ls3.disconnect();

                Thread.Sleep(30000);

                System.Console.WriteLine("Memory usage :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::" +  GC.GetTotalMemory(true));

                Thread.Sleep(30000);
            }
        }
    }
}
