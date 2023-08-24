using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class ChatServer
{
    private List<Socket> clients = new List<Socket>();
    private Socket serverSocket;
    private byte[] buffer = new byte[1024];

    public void Start(int port)
    {
        serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        serverSocket.Bind(new IPEndPoint(IPAddress.Any, port));
        serverSocket.Listen(10);

        Console.WriteLine("Server started. Waiting for connections...");

        while (true)
        {
            Socket clientSocket = serverSocket.Accept();
            clients.Add(clientSocket);

            Thread clientThread = new Thread(HandleClient);
            clientThread.Start(clientSocket);
        }
    }

    private void HandleClient(object clientObj)
    {
        Socket clientSocket = (Socket)clientObj;

        Console.WriteLine($"Client connected: {clientSocket.RemoteEndPoint}");

        while (true)
        {
            int bytesRead = clientSocket.Receive(buffer);
            string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);

            Console.WriteLine($"Received: {message}");

            foreach (Socket client in clients)
            {
                if (client != clientSocket)
                {
                    client.Send(Encoding.ASCII.GetBytes(message));
                }
            }
        }
    }

    public void Stop()
    {
        foreach (Socket client in clients)
        {
            client.Close();
        }
        serverSocket.Close();
    }
}

class Program
{
    static void Main(string[] args)
    {
        ChatServer server = new ChatServer();
        server.Start(8888);
    }
}
