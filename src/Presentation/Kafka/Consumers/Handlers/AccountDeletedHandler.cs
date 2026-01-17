using Application.Services.Interfaces;
using Itmo.Dev.Platform.Kafka.Consumer;
using Presentation.Kafka.Consumers.Keys;
using Presentation.Kafka.Consumers.Values;

namespace Presentation.Kafka.Consumers.Handlers;

public class AccountDeletedHandler : IKafkaInboxHandler<AccountDeletedKeyMessage, AccountDeletedMessage>
{
    private readonly IAccountDeleteService _accountDeleteService;

    public AccountDeletedHandler(IAccountDeleteService accountDeleteService)
    {
        _accountDeleteService = accountDeleteService;
    }

    public async ValueTask HandleAsync(
        IEnumerable<IKafkaInboxMessage<AccountDeletedKeyMessage, AccountDeletedMessage>> messages,
        CancellationToken cancellationToken)
    {
        foreach (IKafkaInboxMessage<AccountDeletedKeyMessage, AccountDeletedMessage> message in messages)
        {
            await _accountDeleteService.AccountDeleteAsync(message.Value.AccountId, cancellationToken);
        }
    }
}