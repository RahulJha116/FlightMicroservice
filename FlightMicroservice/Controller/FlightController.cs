
using FlightMicroservice.IJwtAuthentication;
using FlightMicroservice.Model;
using FlightMicroservice.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
    public class FlightController : ControllerBase
    {
        private readonly IFlightRepository _flightRepository;
        private readonly IJwtAuthenticationManager _JwtAuthenticationManager;

        public FlightController(IFlightRepository flightRepository, IJwtAuthenticationManager jwtAuthenticationManager)
        {
            _flightRepository = flightRepository;
            _JwtAuthenticationManager = jwtAuthenticationManager;
        }

       // [Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            var flights = _flightRepository.GetFlights();
            return new OkObjectResult(flights);
        }

        //[Authorize]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var f = _flightRepository.GetFlightByID(id);
            return new OkObjectResult(f);
        }
        [HttpGet("GetFlightByAirlineId")]
        public IActionResult GetFlightByAirlineId(int airlineId)
        {
            var f = _flightRepository.GetFlightByAirlineId(airlineId);
            return new OkObjectResult(f);
        }


        // [Authorize]
        [HttpPost("Add")]
        public IActionResult AddFlights([FromBody] Model.Flights f)
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqp://guest:guest@localhost:5672")
            };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "AddFlightQueue",
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);
                string message = f.FromPlace + "|" + f.ToPlace + "|" + f.StartDateTime + "|" + f.EndDateTime + "|" + f.FlightNumber
                    + "|" + f.ScheduleDayOfWeek + "|" + f.NoOfBusinessClassSeat + "|" + f.NoOfNonBusinessClassSeat + "|" + f.FlightBusinessClassTicketPrice
                    + "|" + f.FlightNonBusinessClassTicketPrice + "|" + f.Indicator + "|" + f.airlineId + "|" + f.Meal;
                var body = Encoding.UTF8.GetBytes((message));

                channel.BasicPublish(exchange: "",
                    routingKey: "AddFlightQueue",
                    basicProperties: null,
                    body: body);
            }

            using (var scope = new TransactionScope())
            {
                _flightRepository.AddFlights(f);
                scope.Complete();
                return CreatedAtAction(nameof(Get), new { id = f.FlightId }, f);
            }
        }

        [Authorize]
        [HttpPut("updateFlight")]
        public IActionResult updateFlight([FromBody] Model.Flights f)
        {
            if (f != null)
            {
                using (var scope = new TransactionScope())
                {
                    _flightRepository.UpdateFlight(f);
                    scope.Complete();
                    return new OkResult();
                }
            }
            return new NoContentResult();
        }

        [Authorize]
        [HttpDelete("DeleteFlight")]
        public IActionResult DeleteFlight(int id)
        {
            _flightRepository.DeleteFlight(id);
            return new OkResult();
        }

      //  [Authorize]
        [HttpPost("BlockFlight")]
        public IActionResult BlockFlight(int airlineid)
        {

            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqp://guest:guest@localhost:5672")
            };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "AddAirlineBlockQueue",
                    durable: true, 
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);
                string message = airlineid.ToString();
                    var body = Encoding.UTF8.GetBytes((message));

                channel.BasicPublish(exchange: "",
                    routingKey: "AddAirlineBlockQueue",
                    basicProperties: null,
                    body: body);
            }


            _flightRepository.BlockFlight(airlineid);
            return new OkObjectResult("All flight realated to this airline blocked," +
                " user will not see these filght in SearchFlight");
        }

       // [Authorize]
        [HttpPost("UnBlockFlight")]
        public IActionResult UnBlockFlight(int airlineid)
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqp://guest:guest@localhost:5672")
            };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "AddAirlineUnBlockQueue",
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);
                string message = airlineid.ToString();
                var body = Encoding.UTF8.GetBytes((message));

                channel.BasicPublish(exchange: "",
                    routingKey: "AddAirlineUnBlockQueue",
                    basicProperties: null,
                    body: body);
            }

            _flightRepository.UnBlockFlight(airlineid);
            return new OkObjectResult("All flight realated to this airline Un-blocked," +
               " user will  see related filghts in SearchFlight");
        }

        [HttpPost("Adminlogin")]
        public IActionResult AdminLogin(string adminEmailId, string adminPasskey)
        {

            var token = _JwtAuthenticationManager.Authenticate(adminEmailId, adminPasskey);
            if (token == null)
            {
                return Unauthorized();
            }
            return Ok(token);
        }
    }
}
