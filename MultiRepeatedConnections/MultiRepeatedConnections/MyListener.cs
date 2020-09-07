using com.lightstreamer.client;

namespace ConsoleApp3
{
    internal class MyListener : ClientListener
    {

        private string serverAddress = ".";

        public MyListener(string serverAddress)
        {
            this.serverAddress = serverAddress;
        }

        void ClientListener.onListenEnd(LightstreamerClient client)
        {
            // ...
        }

        void ClientListener.onListenStart(LightstreamerClient client)
        {
            // ...
        }

        void ClientListener.onPropertyChange(string property)
        {
            // ...
        }

        void ClientListener.onServerError(int errorCode, string errorMessage)
        {
            System.Console.WriteLine(serverAddress + " - Server error " + errorMessage + " - " + errorCode);
        }

        void ClientListener.onStatusChange(string status)
        {
            System.Console.WriteLine("New connection status for " + serverAddress + ": " + status);
        }
    }
}