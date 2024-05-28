using Microsoft.Azure.CognitiveServices.Language.LUIS.Runtime;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Runtime.Models;

namespace Transport.API.Services
{
    public interface ILuisService
    {
        Task<PredictionResponse> PredictAsync(string query);
    }

    public class LuisService : ILuisService
    {
        private readonly IConfiguration _configuration;
        private readonly LUISRuntimeClient _client;
        private readonly string _appId;

        public LuisService(IConfiguration configuration)
        {
            _configuration = configuration;
            _appId = _configuration["Luis:AppId"];
            var endpointKey = _configuration["Luis:EndpointKey"];
            var endpoint = _configuration["Luis:Endpoint"];

            _client = new LUISRuntimeClient(new ApiKeyServiceClientCredentials(endpointKey))
            {
                Endpoint = endpoint
            };
        }

        public async Task<PredictionResponse> PredictAsync(string query)
        {
            var predictionRequest = new PredictionRequest { Query = query };
            return await _client.Prediction.GetSlotPredictionAsync(Guid.Parse(_appId), "production", predictionRequest);
        }
    }

}
