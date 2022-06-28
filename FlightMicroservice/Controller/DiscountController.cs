using FlightMicroservice.Model;
using FlightMicroservice.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace FlightMicroservice.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountRepository _discountRepository;

        public DiscountController(IDiscountRepository discountRepository)
        {
            _discountRepository = discountRepository;
        }
      //  [Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            var flights = _discountRepository.GetDiscounts();
            return new OkObjectResult(flights);
        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var f = _discountRepository.GetDiscountByID(id);
            return new OkObjectResult(f);
        }

        //[Authorize]
        [HttpPost("AddDiscount")]
        public IActionResult AddDiscount([FromBody] Discount f)
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqp://guest:guest@localhost:5672")
            };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "AddDiscountQueue",
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);
                string message = f.DiscountCode + "|" + f.DiscountAmount;
                    var body = Encoding.UTF8.GetBytes((message));

                channel.BasicPublish(exchange: "",
                    routingKey: "AddDiscountQueue",
                    basicProperties: null,
                    body: body);
            }

            using (var scope = new TransactionScope())
            {
                _discountRepository.AddDiscount(f);
                scope.Complete();
                //return CreatedAtAction(nameof(Get), new { id = f.DiscountId }, f);
                return Ok();
            }
        }

        //[Authorize]
        [HttpPut("updateDiscount")]
        public IActionResult updateDiscount([FromBody] Discount f)
        {
            if (f != null)
            {
                using (var scope = new TransactionScope())
                {
                    _discountRepository.UpdateDiscount(f);
                    scope.Complete();
                    return new OkResult();
                }
            }
            return new NoContentResult();
        }

      //  [Authorize]
        [HttpDelete("DeleteDiscount")]
        public IActionResult DeleteDiscount(int id)
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqp://guest:guest@localhost:5672")
            };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "DeleteDiscountQueue",
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);
                string message = id.ToString();
                var body = Encoding.UTF8.GetBytes((message));

                channel.BasicPublish(exchange: "",
                    routingKey: "DeleteDiscountQueue",
                    basicProperties: null,
                    body: body);
            }


            _discountRepository.DeleteDiscount(id);
            
            return new OkResult();
        }
    }
}
