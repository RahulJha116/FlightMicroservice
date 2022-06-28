using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FlightMicroservice
{
    public class Program
    {
        public static void Main(string[] args)
        {
            
            CreateWebHostBuilder(args).Build().Run();

            // var factory = new ConnectionFactory
            //{
            //    Uri = new Uri("amqp://guest:guest@localhost:5672")
            //};

            //var connection = factory.CreateConnection();

            //var channel = connection.CreateModel();
            //QueueConsumer.Consume(channel);

        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
