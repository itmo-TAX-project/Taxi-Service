using Application.Kafka.Consumers.MessageValues;
using Application.Services.Interfaces;
using Itmo.Dev.Platform.Kafka.Consumer;

namespace Application.Kafka.Consumers.Handlers;

public class AccountDeletedHandler : IKafkaInboxHandler<long, AccountDeletedMessage>
{
    private readonly IAccountDeleteService _accountDeleteService;

    public AccountDeletedHandler(IAccountDeleteService accountDeleteService)
    {
        _accountDeleteService = accountDeleteService;
    }

    public async ValueTask HandleAsync(
        IEnumerable<IKafkaInboxMessage<long, AccountDeletedMessage>> messages,
        CancellationToken cancellationToken)
    {
        foreach (IKafkaInboxMessage<long, AccountDeletedMessage> message in messages)
        {
            await _accountDeleteService.AccountDeleteAsync(message.Value.AccountId, cancellationToken);
        }
    }
}