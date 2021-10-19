using System;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace GISdotNet.Core.Net
{
    public class UdpSocket
    {
        public static UdpClient CreateUdpClient(ushort port)
        {
            IPAddress[] ipv4Addresses = Array.FindAll(
                Dns.GetHostEntry(string.Empty).AddressList,
                a => a.AddressFamily == AddressFamily.InterNetwork);
            UdpClient udpClient = new UdpClient();
            bool isSuccess = false;
            for (int i = 0; i < ipv4Addresses.Length; i++)
            {
                try
                {
                    udpClient.Client.Bind(new IPEndPoint(ipv4Addresses[i], port));
                    isSuccess = true;
                    Console.WriteLine($"Успешно создал сокет {ipv4Addresses[i]}:{port} ");
                    break;
                }
                catch (Exception ex)
                {
                    //Console.WriteLine(ex.Message);
                    continue;
                }
            }
            if (!isSuccess)
            {
                throw new Exception("Не удалось создать сокет!");
            }
            return udpClient;
        }
    }
}
