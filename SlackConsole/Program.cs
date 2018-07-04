using System;
using System.Linq;
using System.Threading.Tasks;
using SlackConnector;
using SlackConnector.Models;

namespace SlackConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new ConfigReader().ReadOrCreateIfDoesNotExist();
            
            ISlackConnector connector = new SlackConnector.SlackConnector();
            ISlackConnection conn = connector
                .Connect(config.ApiKey).Result;
            conn.OnMessageReceived += message => Task.Run(() => WriteButKeepBuffer($"{message.User.Name}< {message.Text}\n"));

            SlackChatHub chatHub = null;
            while (true)
            {
                Console.Write($"{chatHub?.Name ?? ""}> ");
                string s = Console.ReadLine();
                if (s.StartsWith("/s"))
                {
                    String sendingTo = s.Split(" ").Skip(1).LastOrDefault() ?? "";
                    chatHub = conn.ConnectedHubs.Values.FirstOrDefault(x => x.Name == sendingTo);
                }
                else
                {
                    conn.Say(new BotMessage
                    {
                        ChatHub = chatHub,
                        Text = s
                    });
                }
            }
        }

        static void WriteButKeepBuffer(String message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            int y = Console.CursorTop;
            if (Console.CursorTop != y) return;
            int x = Console.CursorLeft;
            Console.MoveBufferArea(0, y, Console.WindowWidth, 1, 0, y + 1);
            Console.SetCursorPosition(0, y);
            Console.Write(message);
            Console.SetCursorPosition(x, y + 1);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}