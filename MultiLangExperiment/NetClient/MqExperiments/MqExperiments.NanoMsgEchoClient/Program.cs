namespace MqExperiments.NanoMsgEchoClient
{
    using System.Text;

    using NNanomsg.Protocols;
    using static System.Console;

    internal class Program
    {
        static void Main(string[] args)
        {
            var socket = new RequestSocket();
            socket.Options.ReceiveTimeout = null;
            socket.Options.SendTimeout = null;
            socket.Connect("tcp://localhost:6789");

            while (true)
            {
                string input;
                if ((input = ReadLine()) == "exit")
                {
                    break;
                }

                if (input == null)
                {
                    continue;
                }

                var bytesToSend = Encoding.UTF8.GetBytes(input);
                socket.Send(bytesToSend);
                var receivedBytes = socket.Receive();
                WriteLine($"Получено {Encoding.UTF8.GetString(receivedBytes)}");
            }
        }
    }
}
