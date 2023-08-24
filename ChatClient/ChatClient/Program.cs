using System;
using System.Net.Sockets;
using System.Text;

class ChatClient
{
    private Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    private byte[] buffer = new byte[1024];

    public void Connect(string ipAddress, int port)
    {
        clientSocket.Connect(ipAddress, port);
        Console.WriteLine("Connected to server");

        while (true)
        {
            string message = Console.ReadLine();
            clientSocket.Send(Encoding.ASCII.GetBytes(message));

            int bytesRead = clientSocket.Receive(buffer);
            string receivedMessage = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            Console.WriteLine($"Received: {receivedMessage}");
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        ChatClient client = new ChatClient();
        client.Connect("127.0.0.1", 8888);
    }
}
