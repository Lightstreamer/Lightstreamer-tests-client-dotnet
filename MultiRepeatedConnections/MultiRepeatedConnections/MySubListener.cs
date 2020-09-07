using com.lightstreamer.client;

namespace ConsoleApp3
{
    internal class MySubListener : SubscriptionListener
    {

        bool recv = false;

        void SubscriptionListener.onClearSnapshot(string itemName, int itemPos)
        {
            System.Console.WriteLine("onClearSnapshot");
        }

        void SubscriptionListener.onCommandSecondLevelItemLostUpdates(int lostUpdates, string key)
        {
            throw new System.NotImplementedException();
        }

        void SubscriptionListener.onCommandSecondLevelSubscriptionError(int code, string message, string key)
        {
            throw new System.NotImplementedException();
        }

        void SubscriptionListener.onEndOfSnapshot(string itemName, int itemPos)
        {
            System.Console.WriteLine("onEndOfSnapshot");
        }

        void SubscriptionListener.onItemLostUpdates(string itemName, int itemPos, int lostUpdates)
        {
            System.Console.WriteLine("onItemLostUpdates");
        }

        void SubscriptionListener.onItemUpdate(ItemUpdate itemUpdate)
        {
            System.Console.WriteLine("onItemUpdate for " + itemUpdate.ItemName);
            recv = true;
        }

        void SubscriptionListener.onListenEnd(Subscription subscription)
        {
            throw new System.NotImplementedException();
        }

        void SubscriptionListener.onListenStart(Subscription subscription)
        {
            throw new System.NotImplementedException();
        }

        void SubscriptionListener.onRealMaxFrequency(string frequency)
        {
            throw new System.NotImplementedException();
        }

        void SubscriptionListener.onSubscription()
        {
            throw new System.NotImplementedException();
        }

        void SubscriptionListener.onSubscriptionError(int code, string message)
        {
            throw new System.NotImplementedException();
        }

        void SubscriptionListener.onUnsubscription()
        {
            throw new System.NotImplementedException();
        }

        public bool ReceivedWhatINeed()
        {
            return recv;
        }
    }
}