namespace TheDashboard.DataSourceService.Domain;

public enum Kind
{
  Http = 1,
  Webhook = 2,
  Query = 3,
  GraphQL = 4,
  GRPC = 5,
  Kafka = 6,
  RabbitMQ = 7,
  AzureServiceBus = 8,
  AzureEventHub = 9,
  AzureIoTHub = 10
}