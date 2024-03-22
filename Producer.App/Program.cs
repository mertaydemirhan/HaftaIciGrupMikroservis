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

//fanout exchange
channel.ExchangeDeclare("demo-fanout-exchange", ExchangeType.Fanout, true, false, null);

channel.QueueDeclare("myqueue3", true, false, false, null);

channel.ConfirmSelect();

// send to message
var message = "Hello World";
var body = Encoding.UTF8.GetBytes(message);


Enumerable.Range(1, 100000).ToList()
    .ForEach(x => { channel.BasicPublish(string.Empty, "myqueue4", false, null, body); });

// 

channel.BasicAcks += (sender, message) => { Console.WriteLine(message.DeliveryTag); };


try
{
    int i = 0;
    Enumerable.Range(1, 1000).ToList().ForEach(x =>
    {
        i++;
        channel.BasicPublish(string.Empty, "myqueue4", false, null, body);
        if (i % 30 == 0)
        {
            channel.WaitForConfirms();
        }
    });


    //channel.BasicPublish("demo-fanout-exchange", string.Empty, null, body);
}
catch (Exception e)
{
    Console.WriteLine(e);
    throw;
}


try
{
    channel.BasicPublish(string.Empty, "myqueue4", false, null, body);
    channel.WaitForConfirms();
}
catch (Exception e)
{
    Console.WriteLine(e);
    throw;
}

Console.ReadLine();