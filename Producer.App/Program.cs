// See https://aka.ms/new-console-template for more information

using System.Linq;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

Console.WriteLine("Producer");


var connectionFactory = new ConnectionFactory
{
    Uri = new Uri("amqps://xletlzam:ru0dUCE5XtiSKzpC2befkSXcuDwuKBQh@chimpanzee.rmq.cloudamqp.com/xletlzam")
};


using var connection = connectionFactory.CreateConnection();

using var channel = connection.CreateModel();


channel.BasicAcks += (sender, message) => { Console.WriteLine(message.DeliveryTag); };

//fanout exchange
channel.ExchangeDeclare("demo-fanout-exchange", ExchangeType.Fanout, true, false, null);


channel.ConfirmSelect();

// send to message
var message = "Hello World";
var body = Encoding.UTF8.GetBytes(message);


channel.BasicPublish("demo-fanout-exchange", string.Empty, false, null, body);


Console.ReadLine();