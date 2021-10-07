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

        /// <summary>
        /// Список команд движка консоли <команда, описание>
        /// </summary>
        public static Dictionary<string, string> EngineCommands = new Dictionary<string, string>
        {
            { "Exit", "выйти из программы" },
            { "Clear", "очистить консоль" },
            { "Console.Change.Title", "изменить название консоли" },
            { "Console.Change.Operator.Name", $"изменить имя пользователя в консоли ({Configs.OperatorName} по умолчанию)" },
            { "Console.Change.Operator.Prefix", $"изменить префикс пользователя в консоли ({Configs.OperatorPrefix} по умолчанию)" },
            { "Console.Change.HistoryCount", $"изменить количество запоминаемых команд ({Configs.HistoryCount} по умолчанию)" },
            { "Console.Change.Color.Text.Input", "изменить цвет вводимого текста" },
            { "Console.Change.Color.Text.Printed", "изменить цвет выводимого (напечатанного) текста" },
            { "Console.Change.Color.Operator.Name", "изменить цвет, которым печается имя оператора" },
            { "Console.Change.Color.Operator.Prefix", "изменить цвет, которым печается префикс оператора" },
            { "Console.Change.Color.Command.Text", "изменить цвет зарезирвированных команд" },
            { "Console.Change.Color.Command.Splitters", "изменить цвет разделителей команд (в командах, в которых есть аргументы)" },
            { "Console.Change.Color.Command.Args", "изменить цвет аргументов команд (в командах, в которых есть аргументы)" },
            { "Console.Change.CursorSize", $"изменить высоту курсора ({Configs.CursorSize} по умолчанию)" },
            { "Console.ShowColors", "показать доступные цвета" },
        };


        /// <summary>
        /// Команды, которые можно встроить извне, и методы для них. Для создания метода используйте Action actionName = new Action(() => { ... });
        /// </summary>
        public static Dictionary<Dictionary<string, string>, Action> AdditionalCommands = new Dictionary<Dictionary<string, string>, Action>();

        public static void Exit()
        {
            Environment.Exit(0);
        }

        public static void Clear()
        {
            Console.Clear();
            Cursor.SetPosition(0, 0);

            #region Инициализация

            try
            {
                Console.SetWindowSize(Configs.DefaultWidth, Configs.DefaultHeight);
            }
            catch { }
            Console.SetBufferSize(Configs.DefaultWidth, Configs.DefaultHeight);
            try
            {
                Console.CursorSize = Configs.CursorSize;
            }
            catch { }

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
