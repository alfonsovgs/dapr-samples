using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapr;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using sample.microservice.dto.reservation;
using sample.microservice.reservation.State;

namespace sample.microservice.reservation.Controllers
{
    [ApiController]
    public class OrderController : ControllerBase
    {
        public const string StoreName = "reservationstore";

        [HttpPost("reserve")]
        public async Task<ActionResult<Item>> Reserve(Item reservation, [FromServices] DaprClient daprClient)
        {
            Console.WriteLine("Enter item reservation");

            var state = await daprClient.GetStateEntryAsync<ItemState>(StoreName, reservation.SKU);
            state.Value ??= new ItemState { SKU = reservation.SKU, Changes = new List<ItemReservation>() };

            // update balance
            state.Value.BalanceQuantity -= reservation.Quantity;

            // record cnage
            ItemReservation change = new()
            {
                SKU = reservation.SKU,
                Quantity = reservation.Quantity,
                ReservedOn = DateTime.UtcNow
            };

            state.Value.Changes.Add(change);
            if (state.Value.Changes.Count > 10)
            {
                state.Value.Changes.RemoveAt(0);
            }

            await state.SaveAsync();

            // return current balance
            var result = new Item
            {
                SKU = state.Value.SKU,
                Quantity = state.Value.BalanceQuantity
            };

            Console.WriteLine($"Reservation of {result.SKU} is now {result.Quantity}");

            return result;

        }

        [HttpGet("balance/{state}")]
        public ActionResult<Item> Get([FromState(StoreName)] StateEntry<ItemState> state)
        {
            Console.WriteLine("Enter item retrieval");

            if (state.Value == null)
            {
                return NotFound();
            }

            var result = new Item
            {
                SKU = state.Value.SKU,
                Quantity = state.Value.BalanceQuantity
            };

            Console.WriteLine($"Retrieved {result.SKU} is {result.Quantity}");

            return result;
        }
    }
}
