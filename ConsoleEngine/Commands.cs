using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleEngine
{
    public static class Commands
    {
        private static Dictionary<string, ConsoleColor> Colors = new Dictionary<string, ConsoleColor>
        {
            { "Black", ConsoleColor.Black },
            { "DarkBlue", ConsoleColor.DarkBlue },
            { "DarkGreen", ConsoleColor.DarkGreen },
            { "DarkCyan", ConsoleColor.DarkCyan },
            { "DarkRed", ConsoleColor.DarkRed },
            { "DarkMagenta", ConsoleColor.DarkMagenta },
            { "DarkYellow", ConsoleColor.DarkYellow },
            { "Gray", ConsoleColor.Gray },
            { "DarkGray", ConsoleColor.DarkGray },
            { "Blue", ConsoleColor.Blue },
            { "Green", ConsoleColor.Green },
            { "Cyan", ConsoleColor.Cyan },
            { "Red", ConsoleColor.Red },
            { "Magenta", ConsoleColor.Magenta },
            { "Yellow", ConsoleColor.Yellow },
            { "White", ConsoleColor.White },
        };

        public static Dictionary<string, string> EngineCmds = new Dictionary<string, string>
        {
            { "Exit", "выйти из программы" },
            { "Clear", "очистить консоль" },
            { "Console.Change.Title", "изменить название консоли" },
            { "Console.Change.Operator.Name", "изменить название консоли" },
            { "Console.Change.Operator.Prefix", "изменить название консоли" },
            { "Console.Change.HistoryCount", "изменить название консоли" },
            { "Console.Change.Color.Text.Input", "изменить название консоли" },
            { "Console.Change.Color.Text.Printed", "изменить название консоли" },
            { "Console.Change.Color.Operator.Name", "изменить название консоли" },
            { "Console.Change.Color.Operator.Prefix", "изменить название консоли" },
            { "Console.Change.Color.Command.Text", "изменить название консоли" },
            { "Console.Change.Color.Command.Splitters", "изменить название консоли" },
            { "Console.Change.Color.Command.Args", "изменить название консоли" },
            { "Console.Change.CursorSize", "изменить название консоли" },
            { "Console.ShowColors", "изменить название консоли" },
        };

        public static void Exit()
        {
            Environment.Exit(0);
        }

        public static void Clear()
        {
            Console.Clear();
            Cursor.SetCursorPosition(0, 0);

            #region Инициализация

            Console.SetWindowSize(Configs.DefaultWidth, Configs.DefaultHeight);
            Console.SetBufferSize(Configs.DefaultWidth, Configs.DefaultHeight);
            Console.CursorSize = Configs.CursorSize;

            ConsoleEngine.LeftBorder = Configs.OperatorName.Length + Configs.OperatorPrefix.Length;
            if (ConsoleEngine.LeftBorder > Configs.MinWidth)
                Configs.MinWidth = ConsoleEngine.LeftBorder + 10;

            ConsoleEngine.History = new List<string>();
            ConsoleEngine.HistoryOffset = 0;
            ConsoleEngine.HistoryCount = Configs.HistoryCount;

            ConsoleEngine.CurrentText = "";
            ConsoleEngine.PrintedConsoleText = "";

            Cursor.Left = Console.CursorLeft;
            Cursor.Top = Console.CursorTop;
            ConsoleEngine.RightOffset = 0;

            #endregion
        }

        public static void ChangeTitle(string title)
        {
            Console.Title = title;
        }

        public static void ChangeOperatorName(string name)
        {
            Configs.OperatorName = name;

            ConsoleEngine.LeftBorder = Configs.OperatorName.Length + Configs.OperatorPrefix.Length;
            if (ConsoleEngine.LeftBorder > Configs.MinWidth)
                Configs.MinWidth = ConsoleEngine.LeftBorder + 10;
        }

        public static void ChangeOperatorPrefix(string prefix)
        {
            Configs.OperatorPrefix = prefix;

            ConsoleEngine.LeftBorder = Configs.OperatorName.Length + Configs.OperatorPrefix.Length;
            if (ConsoleEngine.LeftBorder > Configs.MinWidth)
                Configs.MinWidth = ConsoleEngine.LeftBorder + 10;
        }

        public static void ChangeHistoryCount(int count)
        {
            Configs.HistoryCount = count;

            if (count < ConsoleEngine.History.Count)
            {
                List<string> tmp = ConsoleEngine.History;
                ConsoleEngine.History = new List<string>();
                for (int i = tmp.Count - 1; i != tmp.Count - count; i -= 1)
                {
                    ConsoleEngine.History.Add(tmp[i]);
                }
                ConsoleEngine.History.Reverse();
                tmp.Clear();
            }
        }
    }
}
