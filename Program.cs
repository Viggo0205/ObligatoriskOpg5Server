using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading.Tasks;
//using System.Threading.Tasks;

Console.WriteLine("TCP server");

TcpListener listener = new TcpListener(IPAddress.Any, 7);
listener.Start();
while (true)
{
    TcpClient socket = listener.AcceptTcpClient();
    Task.Run(() => HandleClient(socket));
}

void HandleClient(TcpClient socket)
{

    NetworkStream ns = socket.GetStream();
    StreamReader reader = new StreamReader(ns);
    StreamWriter writer = new StreamWriter(ns);
    writer.AutoFlush = true;

    while (socket.Connected)
    {


        try
        {

            string message = reader.ReadLine();
            if (string.IsNullOrEmpty(message))
            {
                writer.WriteLine("Du skal altså sende noget?");
            }
            var jsonMessage = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(message);
            var method = jsonMessage["method"].GetString().ToLower();
            int tal1 = jsonMessage["Tal1"].GetInt32();
            int tal2 = jsonMessage["Tal2"].GetInt32();
            
            if (method == "stop")
            {
                writer.WriteLine("Goodbye world");
                socket.Close();
            }


            else if (method == "random")
            {
                

                Random random = new Random();
                int result = random.Next(tal1, tal2);
                
                var response = new { result = result };
                writer.WriteLine(JsonSerializer.Serialize(response));

            }
            else if (method == "add")
            {

                int result = tal1 + tal2;
                var response = new { result = result };
                writer.WriteLine(JsonSerializer.Serialize(response));
            }
            else if (method == "subtract")
            {
 
                int result = tal1 - tal2;
                var response = new { result = result };
                writer.WriteLine(JsonSerializer.Serialize(response));
            }
            

        }
        catch (ArgumentOutOfRangeException ex) 
        { 
            writer.WriteLine($"Error {ex.Message}");
        }
       
       
       

    }
}


//listener.Stop();
