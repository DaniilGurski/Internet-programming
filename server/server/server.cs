using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TcpServer2._8
{
    public class Program
    {
        private static List<TcpClient> clients = new List<TcpClient>();
        private static readonly object lockObj = new object();

        public static int Main(string[] args)
        {
            int portNum = 13;
            TcpListener? listener = null;

            try
            {
                listener = new TcpListener(IPAddress.Any, portNum);
                listener.Start();

                Console.WriteLine("Waiting for connections...");

                // Accept clients in a separate thread
                Thread acceptThread = new Thread(() =>
                {
                    while (true)
                    {
                        TcpClient client = listener.AcceptTcpClient();
                        lock (lockObj)
                        {
                            clients.Add(client);
                        }
                        Console.WriteLine("Client connected.");
                        Thread clientThread = new Thread(HandleClient);
                        clientThread.Start(client);
                    }
                });
                acceptThread.Start();

                // Handle broadcasting messages
                while (true)
                {
                    Console.WriteLine("Send a message to all clients: ");
                    string? message = Console.ReadLine();

                    if (!string.IsNullOrEmpty(message))
                    {
                        BroadcastMessage(message);
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

        private static void HandleClient(object? obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            TcpClient client = (TcpClient)obj;
            NetworkStream ns = client.GetStream();

            try
            {
                byte[] buffer = new byte[1024];
                while (true)
                {
                    int bytesRead = ns.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break; // Client disconnected

                    string receivedMessage = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    Console.WriteLine($"Received: {receivedMessage}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Client error: {e.Message}");
            }
            finally
            {
                lock (lockObj)
                {
                    clients.Remove(client);
                }
                client.Close();
                Console.WriteLine("Client disconnected.");
            }
        }

        private static void BroadcastMessage(string message)
        {
            byte[] byteMessage = Encoding.ASCII.GetBytes(message);

            lock (lockObj)
            {
                foreach (var client in clients)
                {
                    try
                    {
                        NetworkStream ns = client.GetStream();
                        ns.Write(byteMessage, 0, byteMessage.Length);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error sending to client: {e.Message}");
                    }
                }
            }
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