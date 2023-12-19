using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace UDP
{
    internal class Program
    {
        static IPAddress RemoteIpAddress;
        static int LocalPort;
        static int RemotePort;
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Введіть remote ip: ");
                RemoteIpAddress = IPAddress.Parse(Console.ReadLine());
                Console.WriteLine("Введіть RemotePort: ");
                RemotePort = int.Parse(Console.ReadLine());
                Console.WriteLine("Введіть LocalPort: ");
                LocalPort = int.Parse(Console.ReadLine());
                Thread thread = new Thread(new ThreadStart(ReceiveTimeFromAddress));
                thread.IsBackground = true;
                thread.Start();
                while (true)
                {
                    SendTimeToAddress(Console.ReadLine());
                    System.Threading.Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        static void ReceiveTimeFromAddress()
        {
            try
            {
                while (true)
                {
                    UdpClient udpClient = new UdpClient(LocalPort);
                    IPEndPoint ipEnd = null;
                    byte[] bytes = udpClient.Receive(ref ipEnd);
                    string datagramText = Encoding.Unicode.GetString(bytes);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Отримано повідомлення: " + datagramText);
                    Console.ForegroundColor = ConsoleColor.Red;
                    udpClient.Close();
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine("Socket" + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        static void SendTimeToAddress(string datagram)
        {
            UdpClient udpClient = new UdpClient();
            IPEndPoint ipEnd = new IPEndPoint(RemoteIpAddress, RemotePort);
            try
            {
                byte[] bytes = Encoding.Unicode.GetBytes(DateTime.Now.ToString());
                udpClient.Send(bytes, bytes.Length, ipEnd);
                Console.WriteLine("Час було відправленно", ipEnd.ToString());
            }
            catch (SocketException ex)
            {
                Console.WriteLine("Socket" + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                udpClient.Close();
            }

        }
    }
}
