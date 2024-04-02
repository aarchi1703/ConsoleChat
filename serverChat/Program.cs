using System;
using System.Collections.Generic;
using System.Net;   
using System.Net.Sockets;
using System.IO;
class MyTcpListener
{
    public static void Main()
    {
        TcpListener server = new TcpListener(IPAddress.Any, 13000);
        server.Start();
        // Цикл прослушивания
        while (true)
        {
            try
            {
                Console.Write("Жду подключения... ");
                using (TcpClient client = server.AcceptTcpClient())
                {
                    Console.WriteLine("Подключение состоялось!");

                    // Получить поток клиентского сокета
                    using (Stream stream = client.GetStream())
                    {
                        // Получить строку из потока сокета
                        using (var br = new BinaryReader(stream))
                        {
                            string data = br.ReadString();

                            Console.WriteLine("Получено: {0}", data);

                            // Обработать строку, которую прислал клиент
                            data = ProcessCommand(data);

                            // Отправить строку в поток сокета
                            using (var bw = new BinaryWriter(stream))
                            {
                                bw.Write(data);
                                Console.WriteLine("Отправлено: {0}", data);
                            }
                        }
                    }
                }
            }
            catch
            { }
        }
    }

    static Dictionary<string, string> users = new Dictionary<string, string>();

    static List<string> messages = new List<string>();

    public static string ProcessCommand(string data)
    {

        try
        {
            if (data.Length == 0)
                return "Пустая строка недопустима";
            switch (Char.ToLower(data[0]))
            {
                case 'r':
                    var ss = data.Split();
                    var login = ss[1];
                    var password = ss[2];
                    if (!users.ContainsKey(login))
                    {
                        users[login] = password;
                        return "Ok!";
                    }
                    return "Пользователь с таким логином уже зарегистрирован";

                case 'c':
                    var ss1 = data.Split();
                    var login1 = ss1[1];
                    var password1 = ss1[2];
                    if (!users.ContainsKey(login1))
                    {
                        return "Пользователья с таким логином нет!";
                    }
                    if (users[login1] != password1)
                    {
                        return "Пароль неверный";
                    }
                    return "Ok";
                case 'm':
                    var ss2 = data.Split();
                    var login2 = ss2[1];
                    var password2 = ss2[2];
                    if (!users.ContainsKey(login2))
                    {
                        return "Пользователья с таким логином нет. Вы не можете отправлять сообщения!";
                    }
                    if (users[login2] != password2)
                    {
                        return "Пароль неверный. Вы не можете отправлять сообщения!";
                    }
                    var message = login2 + ": " + string.Join(" ", ss2.Skip(3));
                    messages.Add(message);
                    return "Ok";
                case 'g':
                    var ss3 = data.Split();
                    int n = int.Parse(ss3[1]);
                    return string.Join("\n", messages.TakeLast(n));
            }
            return "Неизвестная команда" + " " + data[0];
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }
}