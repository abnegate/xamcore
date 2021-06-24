using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamCore.Services
{
    public class MessageService : IMessageService
    {
        public void Subscribe(MessageType messageType, Action action) =>
            MessagingCenter.Subscribe<MessageService>(
                this,
                messageType.ToString(),
                _ => action());

        public void Subscribe<T>(MessageType messageType, Action<T> action) =>
            MessagingCenter.Subscribe<MessageService, T>(
                this,
                messageType.ToString(),
                (sender, arg) => action(arg));

        public void Subscribe(MessageType messageType, Func<Task> action) =>
            MessagingCenter.Subscribe<MessageService>(
                this,
                messageType.ToString(),
                async sender => await action());

        public void Subscribe<T>(MessageType messageType, Func<T, Task> action) =>
            MessagingCenter.Subscribe<MessageService, T>(
                this,
                messageType.ToString(),
                async (sender, arg) => await action(arg));

        public void Unsubscribe(MessageType messageType) =>
            MessagingCenter.Unsubscribe<MessageService>(this, messageType.ToString());

        public void Unsubscribe(params MessageType[] messageTypes) =>
            messageTypes.ToList().ForEach(Unsubscribe);

        public void Send(MessageType messageType) =>
            MessagingCenter.Send(this, messageType.ToString());

        public void Send<T>(MessageType messageType, T arg) =>
            MessagingCenter.Send(this, messageType.ToString(), arg);
    }
}
