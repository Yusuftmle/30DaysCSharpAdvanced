using Grpc.Core;
using grpcMessageServer;

namespace grpcServer.Services;

public class MessageService : Message.MessageBase
{
    /// <summary>
    /// Sends a message to the client and streams responses back.
    /// </summary>
    //  public override async Task SendMessage(MessageRequest request, IServerStreamWriter<MessageResponse> responseStream, ServerCallContext context)
    //  {
        

    //     Console.WriteLine($"Received message from {request.Name}: {request.Message}");
    //      for (int i = 0; i < 20; i++)
    //      {
    //          await Task.Delay(1000);//simulate some work
    //          await responseStream.WriteAsync(new MessageResponse
    //          {
    //              Message = $"Response {i + 1} from server"
    //          });
    //      }
    //  }


     /// <summary>
     /// Server streaming
     /// </summary>
    // public override async Task SendMessage(MessageRequest request, IServerStreamWriter<MessageResponse> responseStream, ServerCallContext context)
    // {
    //     Console.WriteLine($"Received message from {request.Name}: {request.Message}");
        
    //     for (int i = 0; i < 20; i++)
    //     {
    //         await Task.Delay(1000); // Simulate some work
    //         await responseStream.WriteAsync(new MessageResponse
    //         {
    //             Message = $"Response {i + 1} from server"
    //         });
    //     }
    // }


   /// <summary>
     /// Client streaming
     /// </summary>
//     public override async Task<MessageResponse> SendMessage(IAsyncStreamReader<MessageRequest> RequestStream, ServerCallContext context)
// {
//     while (await RequestStream.MoveNext(context.CancellationToken))
//     {
//         Console.WriteLine($"Received message from {RequestStream.Current.Name}: {RequestStream.Current.Message}");
//     }

//     // Return a final response after processing all messages
//     var finalResponse = new MessageResponse
//     {
//         Message = "All messages received."
//     };
    
//     return finalResponse; // ✅ Bu satırı ekleyin!
// }

/// <summary>
     /// Bi-directional streaming
     /// </summary>

public override async Task SendMessage(IAsyncStreamReader<MessageRequest> requestStream, IServerStreamWriter<MessageResponse> responseStream, ServerCallContext context)
{
   var task1= Task.Run(async () =>
    {
        while (await requestStream.MoveNext(context.CancellationToken))
        {
            Console.WriteLine($"Received message from {requestStream.Current.Name}: {requestStream.Current.Message}");

            
        }
    });
     for(int i=0; i<10; i++)
        {
            await Task.Delay(100);
            await responseStream.WriteAsync(new MessageResponse
            {
                Message = $"Response {i + 1} from server"
            });
        }
    await task1; // Wait for the request stream processing to complete
     

}
}
