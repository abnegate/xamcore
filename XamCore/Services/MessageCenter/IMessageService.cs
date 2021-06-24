using System;
using System.Threading.Tasks;

namespace XamCore.Services
{
    public interface IMessageService
    {
        void Subscribe(MessageType messageType, Action action);
        void Subscribe(MessageType messageType, Func<Task> action);
        void Subscribe<T>(MessageType messageType, Action<T> action);
        void Subscribe<T>(MessageType messageType, Func<T, Task> action);

        void Unsubscribe(params MessageType[] messageTypes);

        void Send(MessageType messageType);
        void Send<T>(MessageType messageType, T arg);
    }
}
