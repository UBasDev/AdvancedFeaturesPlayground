using Newtonsoft.Json;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ
{
    public class EventBusRabbitMq : BaseEventBus
    {
        private readonly RabbitMqPersistentConnection _persistentConnection;
        private readonly IModel _consumerChannel;

        public EventBusRabbitMq(IServiceProvider serviceProvider, EventBusConfig config) : base(serviceProvider, config)
        {
            IConnectionFactory connectionFactory;
            if (eventBusConfig.Connection != null)
            {
                var connJson = JsonConvert.SerializeObject(eventBusConfig.Connection,
                    new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                connectionFactory = JsonConvert.DeserializeObject<ConnectionFactory>(connJson);
            }
            else
            {
                connectionFactory = new ConnectionFactory();
            }

            _persistentConnection = new RabbitMqPersistentConnection(connectionFactory, config.ConnectionRetryCount);
            _consumerChannel = CreateConsumerChannel();
            SubscriptionManager.OnEventRemoved += SubscriptionManager_OnEventRemoved;
        }

        private void SubscriptionManager_OnEventRemoved(object? sender, string eventName)
        {
            eventName = ProcessEventName(eventName);

            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            _consumerChannel.QueueUnbind(queue: eventName, exchange: eventBusConfig.DefaultTopicName,
                routingKey: eventName);

            if (SubscriptionManager.IsEmpty)
            {
                _consumerChannel.Close();
            }
        }

        public override void Publish(IntegrationEvent @event)
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            var policy = Policy.Handle<BrokerUnreachableException>()
                .Or<SocketException>()
                .WaitAndRetry(eventBusConfig.ConnectionRetryCount,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) => { });

            var eventName = @event.GetType().Name;
            eventName = ProcessEventName(eventName);
            _consumerChannel.ExchangeDeclare(exchange: eventBusConfig.DefaultTopicName, type: "direct");
            var message = JsonConvert.SerializeObject(@event);
            var body = Encoding.UTF8.GetBytes(message);
            policy.Execute(() =>
            {
                var properties = _consumerChannel.CreateBasicProperties();
                properties.DeliveryMode = 2;
                _consumerChannel.BasicPublish(exchange: eventBusConfig.DefaultTopicName, routingKey: eventName,
                    mandatory: true, basicProperties: properties, body: body);
            });
        }

        public override void Subscribe<T, TH>()
        {
            var eventName = typeof(T).Name;
            eventName = ProcessEventName(eventName);
            if (!SubscriptionManager.HasSubscriptionsForEvent(eventName))
            {
                if (!_persistentConnection.IsConnected)
                {
                    _persistentConnection.TryConnect();
                }

                _consumerChannel.QueueDeclare(queue: GetSubName(eventName), durable: true, exclusive: false,
                    autoDelete: false, arguments: null);
                _consumerChannel.QueueBind(queue: GetSubName(eventName), exchange: eventBusConfig.DefaultTopicName,
                    routingKey: eventName);
            }

            SubscriptionManager.AddSubscription<T, TH>();
            StartBasicConsume(eventName);
        }

        public override void UnSubscribe<T, TH>()
        {
            SubscriptionManager.RemoveSubscription<T, TH>();
        }

        private IModel CreateConsumerChannel()
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            var channel = _persistentConnection.CreateModel();
            channel.ExchangeDeclare(exchange: eventBusConfig.DefaultTopicName, type: "direct");
            return channel;
        }

        private void StartBasicConsume(string eventName)
        {
            if (_consumerChannel == null) return;
            var consumer = new EventingBasicConsumer(_consumerChannel);
            consumer.Received += Consumer_Received;
            _consumerChannel.BasicConsume(queue: GetSubName(eventName), autoAck: false, consumer: consumer);
        }

        private async void Consumer_Received(object? sender, BasicDeliverEventArgs eventArgs)
        {
            var eventName = eventArgs.RoutingKey;
            eventName = ProcessEventName(eventName);
            var message = Encoding.UTF8.GetString(eventArgs.Body.Span);
            try
            {
                await ProcessEvent(eventName, message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            _consumerChannel.BasicAck(eventArgs.DeliveryTag, multiple: false);
        }
    }
}
