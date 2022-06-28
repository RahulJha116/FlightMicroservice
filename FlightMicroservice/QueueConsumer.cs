using FlightMicroservice.DbContextFlight;
using FlightMicroservice.Model;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightMicroservice
{

    public  class QueueConsumer
    {
        ConnectionFactory factory {get; set;}
        IConnection connection { get; set; }
        IModel channel { get; set; }

        public  void Consume()
        {
            
            channel.QueueDeclare("AddAirlineQueue",
                durable:false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (Sender, e) =>
            {

                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                var airline = new Airline();
                Task.Run(() =>
                {
                    var chunks = message.Split("|");
                    if (chunks.Length != 0)
                    {
                        airline.airlineName = chunks[0];
                        airline.airlineAddress = chunks[1];
                        airline.airlineContactNumber = long.Parse(chunks[2]);
                        airline.airlineLogo = chunks[3];
                    }

                    
                }

                );
                FlightContext db = new FlightContext();
                db.Add(airline);
                db.SaveChanges();
                Console.WriteLine(message);
            };

            channel.BasicConsume("AddAirlineQueue", true, consumer);
            Console.WriteLine("Consumer started");
            Console.ReadLine();
        }

        public void Deregister()
        {
            this.connection.Close();

        }

        public QueueConsumer()
        {
            this.factory = new ConnectionFactory()
            {
                Uri = new Uri("amqp://guest:guest@localhost:5672")
            };

            this.connection = factory.CreateConnection();
            this.channel = connection.CreateModel();
        }

    }
}
