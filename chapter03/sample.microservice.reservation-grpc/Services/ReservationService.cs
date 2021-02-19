using System;
using System.Threading.Tasks;
using Dapr.Client.Autogen.Grpc.v1;
using Dapr.AppCallback.Autogen.Grpc.v1;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using sample.microservice.reservation_grpc.Model;

namespace sample.microservice.reservation_grpc.Services
{
    public class ReservationService : Dapr.AppCallback.Autogen.Grpc.v1.AppCallback.AppCallbackBase
    {
        public override Task<InvokeResponse> OnInvoke(InvokeRequest request, ServerCallContext context)
        {
            Console.WriteLine(request.Method);

            switch (request.Method)
            {
                case "reserve":
                    var input = Extensions.FromAnyAsync<Item>(request.Data);
                    var output = new Item { SKU = input.SKU, Quantity = -input.Quantity };

                    return Task.FromResult(new InvokeResponse
                    {
                        Data = Extensions.ToAnyAsync<Item>(output)
                    });
                default:
                    Console.WriteLine("Method not supported");
                    return Task.FromResult(new InvokeResponse());
            }
        }

        public override Task<ListInputBindingsResponse> ListInputBindings(Empty request, ServerCallContext context)
        {
            return Task.FromResult(new ListInputBindingsResponse());
        }

        public override Task<ListTopicSubscriptionsResponse> ListTopicSubscriptions(Empty request, ServerCallContext context)
        {
            return Task.FromResult(new ListTopicSubscriptionsResponse());
        }
    }
}
