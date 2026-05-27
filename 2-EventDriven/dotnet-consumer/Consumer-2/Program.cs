using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using CodeAcademy.DotnetConsumer.Common.Config;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

Console.WriteLine("Starting Consumer application...");

// Establish connection to RabbitMQ
using var connection = await ConnectionHelper.ConnectAsync();
Console.WriteLine("Connected to RabbitMQ");

var exchangeName = "bjørnars_exchange";

// Implement a basic consumer here.
// Start with:
// - Create a channel
// - Declare a queue
// - Create a consumer and subscribe to the queue
// - Handle incoming messages by deserializing the JSON and printing the content to the console

const string queueName = "Hellu2";
const string routingKey = "T";

var channel = await connection.CreateChannelAsync();

await channel.ExchangeDeclareAsync(exchangeName, ExchangeType.Fanout);

await channel.QueueDeclareAsync(queueName, false, false, true, null);
await channel.QueueBindAsync(queueName, exchangeName, routingKey, null);

var consumer = new AsyncEventingBasicConsumer(channel);

consumer.ReceivedAsync += async (ch, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    
    Console.WriteLine($"Received: {message}");
    
    await channel.BasicAckAsync(ea.DeliveryTag, false);
};

string consumerTag = await channel.BasicConsumeAsync(queueName, false, consumer);

Console.ReadLine();