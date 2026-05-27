using CodeAcademy.DotnetConsumer.Common.Config;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

Console.WriteLine("Producer starting...");
// Establish connection to RabbitMQ
using var connection = await ConnectionHelper.ConnectAsync();
Console.WriteLine("Connected to RabbitMQ");

// Implement a basic producer here.
// Start with:
// - Create a channel
// - Declare a queue
// - Publish a message to the queue (you can use a simple JSON string as the message body)

var exchangeName = "chat";

var channel = await connection.CreateChannelAsync();

await channel.QueueDeclareAsync("chat_bjørnar", false, false, false, null);

await channel.ExchangeDeclareAsync(exchangeName, ExchangeType.Fanout);
var i = 0;

while (true)
{
    i++;
    byte[] messageBodyBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new
    {
        Message = $"Stavanger FTW",
        Timestamp = DateTime.UtcNow
    }));
    var props = new BasicProperties();
    await channel.BasicPublishAsync(exchangeName, "", false, props, messageBodyBytes);
    
    await Task.Delay(2000);
}