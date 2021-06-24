namespace XamCore.Services
{
    public abstract class DeepLinkServiceBase : IDeepLinkService
    {
        private readonly IMessageService _messageService;

        public DeepLinkServiceBase(
            IMessageService messageService)
        {
            _messageService = messageService;
        }

        public abstract void HandleDeepLink(string link);
    }
}
