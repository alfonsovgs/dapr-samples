
dapr run --app-id "order-service" --app-port "5001" --dapr-grpc-port "50010" --dapr-http-port "5010" --components-path "./components/redis" -- dotnet run --project ./sample.microservice.order/sample.microservice.order.csproj --urls="http://+:5001"
dapr run --app-id "reservation-service" --app-port "5002" --dapr-http-port "5020" --components-path "./components/redis" -- dotnet run --project ./sample.microservice.reservation/sample.microservice.reservation.csproj --urls="http://+:5002"
