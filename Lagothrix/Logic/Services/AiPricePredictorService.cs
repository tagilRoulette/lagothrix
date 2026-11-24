using System.ClientModel;
using Logic.Models;
using Logic.Services.Interfaces;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using OpenAI;

namespace Logic.Services;

public class AiPricePredictorService : IAiAssistantService
{
    public readonly IChatClient ChatClient;
    private readonly IConfigurationRoot _secrets;
    private readonly string _modelName;
    private readonly string _APIkey;
    private const string _endpointUri = "https://openrouter.ai/api/v1";
    private const string _briefing =
        """
        You will be given various information about some product or service.
        Judging from that information you should give approximate value of the
        product/service in RUB.
        """;

    public AiPricePredictorService()
    {
        _secrets = new ConfigurationBuilder()
            .AddUserSecrets<AiPricePredictorService>()
            .Build();
        _modelName = _secrets["OpenRouterAIModel"]!;
        _APIkey = _secrets["OpenRouterPriceRecommender"]!;

        ApiKeyCredential credential = new(_APIkey);
        OpenAIClientOptions options = new() { Endpoint = new Uri(_endpointUri) };
        ChatClient = new OpenAIClient(credential, options)
            .GetChatClient(_modelName)
            .AsIChatClient();
    }

    public async Task<decimal> GetPriceRecommendation(
        LotModel lot,
        CancellationToken cancellationToken)
    {
        ICollection<ChatMessage> chatHistory = [new ChatMessage(ChatRole.System, _briefing)];
        chatHistory.Add(new(ChatRole.User, lot.ToString()));
        var chatResponse = await ChatClient.GetResponseAsync<decimal>(chatHistory, cancellationToken: cancellationToken);
        return chatResponse.Result;
    }

    public async Task<string?> Ping(CancellationToken cancellationToken)
    {
        ChatResponse chatResponse = await ChatClient.GetResponseAsync("Ping", cancellationToken: cancellationToken);
        if (cancellationToken.IsCancellationRequested) return null;
        return chatResponse.Text;
    }
}
