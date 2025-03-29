using System.Net;
using System.Net.Sockets;
using System.Text;

class UDPServer
{
    UdpClient udp = new UdpClient(12345);
    List<IPEndPoint> clients = new List<IPEndPoint>();

    void Start()
    {
        Console.WriteLine("ChatOpen");
        while (true)
        {
            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
            byte[] messageByte = udp.Receive(ref sender);
            string message = Encoding.UTF8.GetString(messageByte);

            if (!clients.Contains(sender))
            {
                clients.Add(sender);
                Console.WriteLine("NEW!");
                Console.WriteLine($"Sender_IP : {sender.Address}");
                Console.WriteLine($"Sender_Port : {sender.Port}");
            }
            BroadCast(message);

        }
    }

    void BroadCast(string message)
    {
        byte[] messageByte = Encoding.UTF8.GetBytes(message);
        foreach (IPEndPoint client in clients)
        {
            udp.Send(messageByte, messageByte.Length, client);
        }
        Console.WriteLine($"받은메세지 : {message}");

    }

    static void Main()
    {
        UDPServer uDPServer = new UDPServer();
        uDPServer.Start();
    }
}