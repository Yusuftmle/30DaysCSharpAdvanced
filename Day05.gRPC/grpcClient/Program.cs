using Grpc.Net.Client;
using Grpc.Core;
using grpcServer;


using var channel = GrpcChannel.ForAddress("http://localhost:5260");
var client = new Greeter.GreeterClient(channel);

var reply = await client.SayHelloAsync(new HelloRequest { Name = "mistik" });

Console.WriteLine("Greeting: " + reply.Message);
