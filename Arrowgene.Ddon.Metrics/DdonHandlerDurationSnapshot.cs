using System;

namespace Arrowgene.Ddon.Metrics
{
    public class DdonHandlerDurationSnapshot
    {
        public DdonHandlerDurationSnapshot(string handlerId, string handlerName, string clientIdentity, TimeSpan duration)
        {
            HandlerId = handlerId ?? string.Empty;
            HandlerName = handlerName ?? string.Empty;
            ClientIdentity = clientIdentity ?? string.Empty;
            Duration = duration;
        }

        public string HandlerId { get; }
        public string HandlerName { get; }
        public string ClientIdentity { get; }
        public TimeSpan Duration { get; }
    }
}
