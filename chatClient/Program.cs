using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.IO;
class MyTCPClient
{
    static void Main()
    {
        do
        {
            string message = Console.ReadLine();
            string request = SendRequestAndReceiveAnswer
            ("127.0.0.1", 13000, message);
            Console.WriteLine("Ответ сервера: {0}", request);
        } while (true);
    }
    public static string SendRequestAndReceiveAnswer
    (string server, int port, string message)
    {
        string request = "";
        using (var client = new TcpClient(server, port))
        {
            using (Stream stream = client.GetStream())
            {
                using (var bw = new BinaryWriter(stream))
                {
                    bw.Write(message);

                    using (var br = new BinaryReader(stream))
                    {
                        request = br.ReadString();
                    }
                }
            }
        }
        return request;
    }
}
