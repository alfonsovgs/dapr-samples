using System;
using System.Threading.Tasks;
using Dapr.Client;
using Dapr.Client.Http;
using Microsoft.AspNetCore.Mvc;
using sample.microservice.order.Model;

namespace sample.microservice.order.Controllers
{
    [ApiController]
    public class OrderController : ControllerBase
    {
        [HttpPost("order")]
        public async Task<ActionResult<Guid>> SubmitOrder(Order order, [FromServices] DaprClient daprClient)
        {
            Console.WriteLine("Enter submit order");
            order.Id = Guid.NewGuid();

            var httpExtension = new HTTPExtension
            {
                Verb = HTTPVerb.Post
            };

            foreach (var item in order.Items)
            {
                var data = new
                {
                    product = item.ProductCode,
                    quantity = item.Quantity
                };

                await daprClient.InvokeMethodAsync<object>("reservation-service", "reserve", data, httpExtension);
            }

            Console.WriteLine($"Submitted order {order.Id}");
            return order.Id;
        }

    }
}
