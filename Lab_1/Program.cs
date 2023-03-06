using System.Net;
using System.Net.Sockets;
using System.Text;

IPAddress localAddress = IPAddress.Parse("127.0.0.1");
Console.Write("Enter your name: ");
string? username = Console.ReadLine();
Console.Write("Enter the port to recieve messages: ");
if (!int.TryParse(Console.ReadLine(), out var localPort)) return;
Console.Write("Enter port to send messages: ");
if (!int.TryParse(Console.ReadLine(), out var remotePort)) return;
Console.WriteLine();

Task.Run(ReceiveMessageAsync);
await SendMessageAsync();

async Task SendMessageAsync()
{
    using UdpClient sender = new UdpClient();
    Console.WriteLine("Type your message and press Enter");
    while (true)
    {
        var message = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(message)) break;
        message = $"{username}: {message}";
        byte[] data = Encoding.UTF8.GetBytes(message);
        await sender.SendAsync(data, new IPEndPoint(localAddress, remotePort));
    }
}
async Task ReceiveMessageAsync()
{
    using UdpClient receiver = new UdpClient(localPort);
    while (true)
    {
        var result = await receiver.ReceiveAsync();
        var message = Encoding.UTF8.GetString(result.Buffer);
        Console.WriteLine(message);
    }
}