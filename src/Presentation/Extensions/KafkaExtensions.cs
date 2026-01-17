using Application.Ports.ProducersPorts;
using Itmo.Dev.Platform.Kafka.Configuration;
using Itmo.Dev.Platform.Kafka.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Presentation.Kafka.Consumers.Handlers;
using Presentation.Kafka.Consumers.Keys;
using Presentation.Kafka.Consumers.Values;
using Presentation.Kafka.Producers;
using Presentation.Kafka.Producers.Keys;
using Presentation.Kafka.Producers.Values;

namespace Presentation.Extensions;

public static class KafkaExtensions
{
    public static IServiceCollection AddKafka(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // https://github.com/itmo-is-dev/platform/wiki/Kafka:-Configuration
        services.AddPlatformKafka(builder => builder
            .ConfigureOptions(configuration.GetSection("Kafka"))

            .AddTaxiDriverCreatedProducer(configuration)
            .AddTaxiDriverStatusChangedProducer(configuration)
            .AddTaxiVehicleChangedProducer(configuration)

            .AddAccountDeletedConsumer(configuration)
            .AddTaxiDriverCreateConsumer(configuration)
            .AddTaxiDriverStatusChangeConsumer(configuration)
            .AddTaxiVehicleChangedConsumer(configuration)
            .AddTaxiVehicleCreateConsumer(configuration));
        services.AddScoped<IDriverProducer, KafkaDriverProducer>();

        return services;
    }

    private static IKafkaConfigurationBuilder AddTaxiDriverCreatedProducer(
        this IKafkaConfigurationBuilder builder,
        IConfiguration configuration)
    {
        return builder.AddProducer(p => p
            .WithKey<TaxiDriverCreatedMessageKey>()
            .WithValue<TaxiDriverCreatedMessage>()
            .WithConfiguration(configuration.GetSection("Kafka:Producers:TaxiDriverCreatedMessage"))
            .SerializeKeyWithNewtonsoft()
            .SerializeValueWithNewtonsoft()
            .WithOutbox());
    }

    private static IKafkaConfigurationBuilder AddTaxiDriverStatusChangedProducer(
        this IKafkaConfigurationBuilder builder,
        IConfiguration configuration)
    {
        return builder.AddProducer(p => p
            .WithKey<TaxiDriverStatusChangedMessageKey>()
            .WithValue<TaxiDriverStatusChangedMessage>()
            .WithConfiguration(configuration.GetSection("Kafka:Producers:TaxiDriverStatusChangedMessage"))
            .SerializeKeyWithNewtonsoft()
            .SerializeValueWithNewtonsoft()
            .WithOutbox());
    }

    private static IKafkaConfigurationBuilder AddTaxiVehicleChangedProducer(
        this IKafkaConfigurationBuilder builder,
        IConfiguration configuration)
    {
        return builder.AddProducer(p => p
            .WithKey<TaxiDriverVehicleChangedMessageKey>()
            .WithValue<TaxiDriverVehicleChangedMessage>()
            .WithConfiguration(configuration.GetSection("Kafka:Producers:TaxiVehicleChangedMessage"))
            .SerializeKeyWithNewtonsoft()
            .SerializeValueWithNewtonsoft()
            .WithOutbox());
    }

    private static IKafkaConfigurationBuilder AddAccountDeletedConsumer(
        this IKafkaConfigurationBuilder builder,
        IConfiguration configuration)
    {
        return builder.AddConsumer(c => c
            .WithKey<AccountDeletedKeyMessage>()
            .WithValue<AccountDeletedMessage>()
            .WithConfiguration(configuration.GetSection("Kafka:Consumers:AccountDeletedMessage"))
            .DeserializeKeyWithNewtonsoft()
            .DeserializeValueWithNewtonsoft()
            .HandleInboxWith<AccountDeletedHandler>());
    }

    private static IKafkaConfigurationBuilder AddTaxiDriverCreateConsumer(
        this IKafkaConfigurationBuilder builder,
        IConfiguration configuration)
    {
        return builder.AddConsumer(c => c
            .WithKey<TaxiDriverCreateKeyMessage>()
            .WithValue<TaxiDriverCreateMessage>()
            .WithConfiguration(configuration.GetSection("Kafka:Consumers:TaxiDriverCreateMessage"))
            .DeserializeKeyWithNewtonsoft()
            .DeserializeValueWithNewtonsoft()
            .HandleInboxWith<TaxiDriverCreateHandler>());
    }

    private static IKafkaConfigurationBuilder AddTaxiDriverStatusChangeConsumer(
        this IKafkaConfigurationBuilder builder,
        IConfiguration configuration)
    {
        return builder.AddConsumer(c => c
            .WithKey<TaxiDriverStatusChangeKeyMessage>()
            .WithValue<TaxiDriverStatusChangeMessage>()
            .WithConfiguration(configuration.GetSection("Kafka:Consumers:TaxiDriverStatusChangeMessage"))
            .DeserializeKeyWithNewtonsoft()
            .DeserializeValueWithNewtonsoft()
            .HandleInboxWith<TaxiDriverStatusChangeHandler>());
    }

    private static IKafkaConfigurationBuilder AddTaxiVehicleChangedConsumer(
        this IKafkaConfigurationBuilder builder,
        IConfiguration configuration)
    {
        return builder.AddConsumer(c => c
            .WithKey<TaxiVehicleChangeKeyMessage>()
            .WithValue<TaxiVehicleChangeMessage>()
            .WithConfiguration(configuration.GetSection("Kafka:Consumers:TaxiDriverVehicleChangeMessage"))
            .DeserializeKeyWithNewtonsoft()
            .DeserializeValueWithNewtonsoft()
            .HandleInboxWith<TaxiVehicleChangeHandler>());
    }

    private static IKafkaConfigurationBuilder AddTaxiVehicleCreateConsumer(
        this IKafkaConfigurationBuilder builder,
        IConfiguration configuration)
    {
        return builder.AddConsumer(c => c
            .WithKey<TaxiVehicleCreateKeyMessage>()
            .WithValue<TaxiVehicleCreateMessage>()
            .WithConfiguration(configuration.GetSection("Kafka:Consumers:TaxiVehicleCreateMessage"))
            .DeserializeKeyWithNewtonsoft()
            .DeserializeValueWithNewtonsoft()
            .HandleInboxWith<TaxiVehicleCreateHandler>());
    }
}