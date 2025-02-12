using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TcpServer2._8
{
    public class Program
    {
        public static int Main(string[] args)
        {
            int portNum = 13;
            TcpListener? listener = null;

            try
            {
                listener = new TcpListener(IPAddress.Any, portNum);
                listener.Start();

                Console.WriteLine("Waiting for connection...");
                TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine("Connection accepted.");

                NetworkStream ns = client.GetStream();

                while (true)
                {
                    Console.WriteLine("Send a message: ");
                    string? message = Console.ReadLine();

                    if (!string.IsNullOrEmpty(message))
                    {
                        byte[] byteMessage = Encoding.ASCII.GetBytes(message);
                        ns.Write(byteMessage, 0, byteMessage.Length);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                if (listener != null)
                {
                    listener.Stop();
                }
            }

            return 0;
        }

        private static void clientHandler(object? ob)
        {
            if (ob == null)
            {
                throw new ArgumentNullException(nameof(ob));
            }

            TcpClient client = (TcpClient)ob;
            NetworkStream ns = client.GetStream();
            // Handle the client connection
        }
    }
}
// For assigment 4
// for (int i = 0; i < 10; i++) {
//     byte[] byteNumber = Encoding.ASCII.GetBytes(i.ToString());

//     try
//     {
//         ns.Write(byteNumber, 0, byteNumber.Length);;
//     }
//     catch (Exception e)
//     {
//         Console.WriteLine(e.ToString());
//     }
// }