using com.lightstreamer.client;
using NLog;
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
            Console.WriteLine("Start same three ls ... " + args[0]);

            var config = new NLog.Config.LoggingConfiguration();
            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = "Test.log" };
            var logconsole = new NLog.Targets.ConsoleTarget("logconsole");

            // config.AddRule(LogLevel.Debug, LogLevel.Fatal, logconsole);
            config.AddRule(LogLevel.Warn, LogLevel.Fatal, logconsole);
            config.AddRule(LogLevel.Info, LogLevel.Fatal, logfile);

            NLog.LogManager.Configuration = config;

            // InternalLoggerFactory.DefaultFactory.AddProvider(new ConsoleLoggerProvider((s, level) => true, false));

            LightstreamerClient.setLoggerProvider(new Log4NetLoggerProviderWrapper());

            var ls1 = new MyConnector("https://push.lightstreamer.com", "DEMO");
            var ls2 = new MyConnector("https://push.lightstreamer.com", "WELCOME");
            var ls3 = new MyConnector("http://push.lightstreamer.com", "MARKETDEPTH");
            var ls4 = new MyConnector("http://localhost:8080", "ROUNDTRIPDEMO");
            var ls5 = new MyConnector("http://localhost:8080", "WELCOME");

            if (args[0].StartsWith("yes"))
            {
                Environment.SetEnvironmentVariable("io.netty.allocator.maxOrder", "1");
                Environment.SetEnvironmentVariable("io.netty.allocator.type", "unpooled");

                System.Console.WriteLine("Tuned DotNetty.");
            }

            while (true)
            {
                //var ls1 = new MyConnector("https://push.lightstreamer.com", "DEMO");
                //var ls2 = new MyConnector("https://push.lightstreamer.com", "WELCOME");
                //var ls3 = new MyConnector("http://push.lightstreamer.com", "MARKETDEPTH");
                //var ls4 = new MyConnector("http://localhost:8080", "ROUNDTRIPDEMO");
                //var ls5 = new MyConnector("http://localhost:8080", "PORTFOLIODEMO");

                var connectTasks = new List<Task>();
                var subTasks = new List<Task>();

                connectTasks.Add(ls1.Connect(new CancellationTokenSource(7000).Token));
                connectTasks.Add(ls2.Connect(new CancellationTokenSource(7000).Token));
                connectTasks.Add(ls3.Connect(new CancellationTokenSource(7000).Token));
                connectTasks.Add(ls4.Connect(new CancellationTokenSource(7000).Token));
                connectTasks.Add(ls5.Connect(new CancellationTokenSource(7000).Token));

                var allTasksAwaiter = Task.WhenAll(connectTasks);

                try
                {
                    allTasksAwaiter.Wait();
                }
                catch (Exception e)
                {
                System.Console.WriteLine("ERRORE ********** " + e.Message);
                    
                }

                subTasks.Add(ls1.Subscribe("MERGE", "QUOTE_ADAPTER", new string[2] { "item2", "item16" }, new string[2] { "last_price", "time" }, new CancellationTokenSource(5000).Token));
                subTasks.Add(ls2.Subscribe("MERGE", "STOCKS", new string[2] { "item1", "item5" }, new string[1] { "stock_name" }, new CancellationTokenSource(5000).Token));
                subTasks.Add(ls3.Subscribe("MERGE", null, new string[1] { "AXY_COMP" }, new string[1] { "trading_phase" }, new CancellationTokenSource(5000).Token));
                subTasks.Add(ls4.Subscribe("MERGE", "ROUNDTRIP_ADAPTER", new string[1] { "roundtrip3" }, new string[2] { "message", "IP" }, new CancellationTokenSource(5000).Token));
                subTasks.Add(ls5.Subscribe("MERGE", "STOCKS", new string[2] { "item2", "item4" }, new string[1] { "stock_name" }, new CancellationTokenSource(5000).Token));

                var allSubTasksAwaiter = Task.WhenAll(subTasks);

                try
                {
                    allSubTasksAwaiter.Wait();
                }
                catch (Exception e)
                {
                    System.Console.WriteLine("ERRORE ********** " + e.Message);

                }

                ls1.disconnectAsync(false);
                ls2.disconnectAsync(false);
                ls3.disconnectAsync(false);
                ls4.disconnectAsync(false);
                await ls5.disconnectAsync(true);

                Thread.Sleep(5000);

                System.Console.WriteLine("Memory usage :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::" +  GC.GetTotalMemory(true));

                Thread.Sleep(5000);
            }
        }
    }
}
