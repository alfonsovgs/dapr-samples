using System;
using sample.microservice.dto.order;

namespace sample.microservice.order.State
{
    public class OrderState
    {
        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }
        public Order Order { get; set; }
    }
}
