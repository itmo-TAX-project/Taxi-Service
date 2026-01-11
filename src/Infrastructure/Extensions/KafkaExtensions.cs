using Application.Kafka.Consumers.Handlers;
using Application.Kafka.Consumers.MessageValues;
using Application.Kafka.Producers.MessageValues;
using Application.Kafka.Serialiser;
using Itmo.Dev.Platform.Common.Extensions;
using Itmo.Dev.Platform.Kafka.Configuration;
using Itmo.Dev.Platform.Kafka.Extensions;
using Itmo.Dev.Platform.MessagePersistence;
using Itmo.Dev.Platform.MessagePersistence.Postgres.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;

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

        return services;
    }

    public static IServiceCollection AddMessagePersistence(this IServiceCollection services)
    {
        services.AddUtcDateTimeProvider();
        services.AddSingleton(new Newtonsoft.Json.JsonSerializerSettings());

        services.AddPlatformMessagePersistence(builder => builder
            .WithDefaultPublisherOptions("MessagePersistence:Publisher:Default")
            .UsePostgresPersistence(
                configurator => configurator.ConfigureOptions("MessagePersistence")));

        return services;
    }

    private static IKafkaConfigurationBuilder AddTaxiDriverCreatedProducer(
        this IKafkaConfigurationBuilder builder,
        IConfiguration configuration)
    {
        return builder.AddProducer(p => p
            .WithKey<long>()
            .WithValue<TaxiDriverCreatedMessage>()
            .WithConfiguration(configuration.GetSection("Kafka:Producers:TaxiDriverCreatedMessage"))
            .SerializeKeyWith<LongSerializerDeserializer>()
            .SerializeValueWithNewtonsoft()
            .WithOutbox());
    }

    private static IKafkaConfigurationBuilder AddTaxiDriverStatusChangedProducer(
        this IKafkaConfigurationBuilder builder,
        IConfiguration configuration)
    {
        return builder.AddProducer(p => p
            .WithKey<long>()
            .WithValue<TaxiDriverStatusChangedMessage>()
            .WithConfiguration(configuration.GetSection("Kafka:Producers:TaxiDriverStatusChangedMessage"))
            .SerializeKeyWith<LongSerializerDeserializer>()
            .SerializeValueWithNewtonsoft()
            .WithOutbox());
    }

    private static IKafkaConfigurationBuilder AddTaxiVehicleChangedProducer(
        this IKafkaConfigurationBuilder builder,
        IConfiguration configuration)
    {
        return builder.AddProducer(p => p
            .WithKey<long>()
            .WithValue<TaxiDriverVehicleChangedProducerMessage>()
            .WithConfiguration(configuration.GetSection("Kafka:Producers:TaxiVehicleChangedMessage"))
            .SerializeKeyWith<LongSerializerDeserializer>()
            .SerializeValueWithNewtonsoft()
            .WithOutbox());
    }

    private static IKafkaConfigurationBuilder AddAccountDeletedConsumer(
        this IKafkaConfigurationBuilder builder,
        IConfiguration configuration)
    {
        return builder.AddConsumer(c => c
            .WithKey<long>()
            .WithValue<AccountDeletedMessage>()
            .WithConfiguration(configuration.GetSection("Kafka:Consumers:AccountDeletedMessage"))
            .DeserializeKeyWith<LongSerializerDeserializer>()
            .DeserializeValueWithNewtonsoft()
            .HandleInboxWith<AccountDeletedHandler>());
    }

    private static IKafkaConfigurationBuilder AddTaxiDriverCreateConsumer(
        this IKafkaConfigurationBuilder builder,
        IConfiguration configuration)
    {
        return builder.AddConsumer(c => c
            .WithKey<long>()
            .WithValue<TaxiDriverCreateMessage>()
            .WithConfiguration(configuration.GetSection("Kafka:Consumers:TaxiDriverCreateMessage"))
            .DeserializeKeyWith<LongSerializerDeserializer>()
            .DeserializeValueWithNewtonsoft()
            .HandleInboxWith<TaxiDriverCreateHandler>());
    }

    private static IKafkaConfigurationBuilder AddTaxiDriverStatusChangeConsumer(
        this IKafkaConfigurationBuilder builder,
        IConfiguration configuration)
    {
        return builder.AddConsumer(c => c
            .WithKey<long>()
            .WithValue<TaxiDriverStatusChangedConsumerMessage>()
            .WithConfiguration(configuration.GetSection("Kafka:Consumers:TaxiDriverStatusChangeMessage"))
            .DeserializeKeyWith<LongSerializerDeserializer>()
            .DeserializeValueWithNewtonsoft()
            .HandleInboxWith<TaxiDriverStatusChangeHandler>());
    }

    private static IKafkaConfigurationBuilder AddTaxiVehicleChangedConsumer(
        this IKafkaConfigurationBuilder builder,
        IConfiguration configuration)
    {
        return builder.AddConsumer(c => c
            .WithKey<long>()
            .WithValue<TaxiVehicleChangeMessage>()
            .WithConfiguration(configuration.GetSection("Kafka:Consumers:TaxiDriverVehicleChangeMessage"))
            .DeserializeKeyWith<LongSerializerDeserializer>()
            .DeserializeValueWithNewtonsoft()
            .HandleInboxWith<TaxiVehicleChangeHandler>());
    }

    private static IKafkaConfigurationBuilder AddTaxiVehicleCreateConsumer(
        this IKafkaConfigurationBuilder builder,
        IConfiguration configuration)
    {
        return builder.AddConsumer(c => c
            .WithKey<long>()
            .WithValue<TaxiVehicleCreateMessage>()
            .WithConfiguration(configuration.GetSection("Kafka:Consumers:TaxiVehicleCreateMessage"))
            .DeserializeKeyWith<LongSerializerDeserializer>()
            .DeserializeValueWithNewtonsoft()
            .HandleInboxWith<TaxiVehicleCreateHandler>());
    }
}