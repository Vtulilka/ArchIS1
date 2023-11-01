using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LB1
{
    class Client
    {
        private const int Port = 8001;

        public static async Task SendRequest(string request)
        {
            UdpClient udpClient = new UdpClient();
            byte[] requestData = Encoding.UTF8.GetBytes(request);

            await udpClient.SendAsync(requestData, requestData.Length, new IPEndPoint(IPAddress.Loopback, Port));

            var result = await udpClient.ReceiveAsync();
            string response = Encoding.UTF8.GetString(result.Buffer);

            Console.WriteLine(response);
        }
    }
}
