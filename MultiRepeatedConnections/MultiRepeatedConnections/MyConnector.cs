using com.lightstreamer.client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    class MyConnector : IDisposable
    {
		private LightstreamerClient client;

		public MyConnector(string host, string adapter_set)
		{
			try
			{
				this.client = new LightstreamerClient(host, adapter_set);
				var clientListener = new MyListener(client.connectionDetails.AdapterSet);
				client.connectionOptions.ForcedTransport = "WS-STREAMING";
				client.addListener(clientListener);
			}
			catch (Exception e)
			{
				System.Console.WriteLine("Errore Create. " + e.Message) ;
			}
			
		}

		public async Task Connect(CancellationToken cancellationToken = default)
		{
			try
			{
				client.connect();
			}
			catch (Exception e)
			{
				System.Console.WriteLine("Errore Create. " + e.Message);
			}

			while (!IAmConnected() && !cancellationToken.IsCancellationRequested)
				await Task.Delay(70);

			if (!IAmConnected() && cancellationToken.IsCancellationRequested)
				throw new Exception(client.connectionDetails.ServerAddress);
		}

		public async Task Subscribe(string mode, string da, string[] items, string[] fields, CancellationToken cancellationToken = default)
		{
			var sub = new Subscription(mode, items, fields);
			if (da != null)
			{
				sub.DataAdapter = da;
			}
			
			MySubListener listnr = new MySubListener();
			sub.addListener(listnr);
			if (client != null)
			{
				client.subscribe(sub);

				while (!listnr.ReceivedWhatINeed() && !cancellationToken.IsCancellationRequested)
					await Task.Delay(70);

				client.unsubscribe(sub);
			}
		}

		public void Dispose()
		{
			if (client != null)
			{
				// 
			}
		}

		public async Task disconnectAsync(bool future)
		{
			if (client != null)
			{
				if (future)
				{
					await client.DisconnectFuture();
				} else
				{
					client.disconnect();
				}
				
			}
		}

		private bool IAmConnected() 
		{
			return (client.Status.StartsWith("CONNECTED:WS") || client.Status.StartsWith("CONNECTED:HTTP"));
		}

	}

	
}
