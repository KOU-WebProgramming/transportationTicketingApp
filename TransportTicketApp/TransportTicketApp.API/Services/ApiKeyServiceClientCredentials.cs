using Microsoft.Rest;

namespace TransportTicketApp.API.Services
{

    public class ApiKeyServiceClientCredentials : ServiceClientCredentials
    {
        private readonly string _subscriptionKey;

        public ApiKeyServiceClientCredentials(string subscriptionKey)
        {
            _subscriptionKey = subscriptionKey;
        }

        public override Task ProcessHttpRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Add("Ocp-Apim-Subscription-Key", _subscriptionKey);
            return base.ProcessHttpRequestAsync(request, cancellationToken);
        }
    }

}
