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
			this.client = new LightstreamerClient(host, adapter_set);
			var clientListener = new MyListener(client.connectionDetails.AdapterSet);
			client.addListener(clientListener);
		}

		public async Task Connect(CancellationToken cancellationToken = default)
		{
			
			client.connect();

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

		public void disconnect()
		{
			if (client != null)
			{
				client.disconnect();
			}
		}

		private bool IAmConnected() 
		{
			return (client.Status.StartsWith("CONNECTED:WS") || client.Status.StartsWith("CONNECTED:HTTP"));
		}

	}

	
}
