// See https://aka.ms/new-console-template for more information

using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

//prefetch count = 50
var connectionFactory = new ConnectionFactory
{
    Uri = new Uri("amqps://xletlzam:ru0dUCE5XtiSKzpC2befkSXcuDwuKBQh@chimpanzee.rmq.cloudamqp.com/xletlzam")
};

Console.WriteLine("Mesajlar dinleniyor...");
using var connection = connectionFactory.CreateConnection();

var channel = connection.CreateModel();


channel.BasicQos(0, 30, true);


//create queue
channel.QueueDeclare("fanout-queue", true, false, false, null);

channel.QueueBind("fanout-queue", "demo-fanout-exchange", string.Empty);

var consumer = new EventingBasicConsumer(channel);


consumer.Received += (sender, eventArgs) =>
{
    var body = eventArgs.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine(message);
    channel.BasicAck(eventArgs.DeliveryTag, false);
};

channel.BasicConsume("fanout-queue", false, consumer);


Console.ReadLine();