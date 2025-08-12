using Grpc.Net.Client;
using Grpc.Core;
using grpcMessageClient;



using var channel = GrpcChannel.ForAddress("http://localhost:5260");
    var messageClient = new Message.MessageClient(channel);
 //unary
  //  var reply = await messageClient.SendMessageAsync(new MessageRequest 
   // { 
  //      Name = "Client User",
  //      Message = "Hello, gRPC!" 
  //  });


/// <summary>
/// server streaming
/// </summary>
//  var response=  messageClient.SendMessage(new MessageRequest
//   {
//       Name = "Client User",
//       Message = "Hello, gRPC!"
//   });

 CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
//   while (await response.ResponseStream.MoveNext(cancellationTokenSource.Token))
//   {
//       Console.WriteLine("Server Response: " + response.ResponseStream.Current.Message);
//   }



/// <summary>
/// Client streaming
/// </summary>
// var request= messageClient.SendMessage();
// for (int i = 0; i < 10; i++)
// {
//     await request.RequestStream.WriteAsync(new MessageRequest
//     {
//         Name = "Client User",
//         Message = $"Hello, gRPC! {i + 1}"
//     });
//     await Task.Delay(1000); // Simulate some delay
// }
// await request.RequestStream.CompleteAsync(); //indicates that the stream data has ended

// Console.WriteLine((await request.ResponseAsync).Message);


/// <summary>
/// Bidirectional streaming
/// </summary>
/// 
 var request = messageClient.SendMessage();
 var task1=  Task.Run(async () =>
{
  for (int i = 0; i < 100; i++)
    {
       await Task.Delay(100); // Simulate some delay
        await request.RequestStream.WriteAsync(new MessageRequest
        {
            Name = "Client User",
            Message = $"Hello, gRPC! {i + 1}"
        });
    }    
});

 while(await request.ResponseStream.MoveNext(cancellationTokenSource.Token))
    {
        Console.WriteLine("Server Response: " + request.ResponseStream.Current.Message);
    }
 await task1;
 await request.RequestStream.CompleteAsync(); //indicates that the stream data has ended
