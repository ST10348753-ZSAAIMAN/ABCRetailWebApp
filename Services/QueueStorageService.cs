using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

public class QueueStorageService
{
    private readonly QueueClient _queueClient;

    public QueueStorageService(IConfiguration configuration)
    {
        var queueServiceClient = new QueueServiceClient(configuration.GetConnectionString("AzureStorage"));
        _queueClient = queueServiceClient.GetQueueClient("orderprocessing");
        _queueClient.CreateIfNotExists();
    }

    public async Task SendMessageAsync(string message)
    {
        await _queueClient.SendMessageAsync(message);
    }

    public async Task<QueueMessage[]> ReceiveMessagesAsync(int maxMessages = 10)
    {
        var messages = await _queueClient.ReceiveMessagesAsync(maxMessages);
        return messages.Value;
    }

    public async Task DeleteMessageAsync(string messageId, string popReceipt)
    {
        await _queueClient.DeleteMessageAsync(messageId, popReceipt);
    }

    public async Task ClearMessagesAsync()
    {
        await _queueClient.ClearMessagesAsync();
    }

    public async Task<int> GetApproximateMessageCountAsync()
    {
        var properties = await _queueClient.GetPropertiesAsync();
        return properties.Value.ApproximateMessagesCount;
    }
}
