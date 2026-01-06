namespace testing.Core.Domain.Interfaces
{
    public interface IMessageBusClient
    {
        void PublishUserCreated(string message);
    }
}
