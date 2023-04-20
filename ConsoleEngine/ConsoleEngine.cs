using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEngine
{
    public static class ConsoleEngine
    {
        #region Поля

        /// <summary>
        /// Содержит историю ввода команд
        /// </summary>
        internal static List<string> History;

        /// <summary>
        /// Содержит информацию о индексе команды из истории, который меняется стрелками вниз-вверх
        /// </summary>
        public static int HistoryOffset;

        /// <summary>
        /// Содержит число, обозначающее максимально возможное количество запоминаний ввода текста
        /// </summary>
        public static int HistoryCount;

        /// <summary>
        /// Хранит весь выведенный в консоль текст до ввода команды "Clear"
        /// </summary>
        public static string PrintedConsoleText;

        /// <summary>
        /// Текущий набранный текст
        /// </summary>
        public static string CurrentText;

        public static int RightOffset;

        /// <summary>
        /// Граница слева, откуда начинается печать текста. Необходима для вывода идентификатора.
        /// </summary>
        public static int LeftBorder;

        /// <summary>
        /// Показана ли подсказка с доступными командами. Нужна для отчистки если подсказка висит, а пользователь продолжил ввод команды
        /// </summary>
        private static bool IsHelpShow = false;

        /// <summary>
        /// На сколько единиц вниз написан текст подсказки. Переменная нужна для отчиски окна подсказки.
        /// </summary>
        private static int HelpWindowHeight = 0;

        #endregion

        public static string ReadLine()
        {
            Console.ForegroundColor = Configs.OperatorNameColor;
            Console.Write(Configs.OperatorName);
            Console.ForegroundColor = Configs.OperatorPrefixColor;
            Console.Write(Configs.OperatorPrefix);
            Console.ForegroundColor = ConsoleColor.White;

            Cursor.Left = Console.CursorLeft;
            Cursor.Top = Console.CursorTop;

            CurrentText = "";

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);

                HideHelpWindow();

                #region Keys

                if (key.Key == ConsoleKey.Enter)
                {

                    #region Работа с историей ввода

                    if (CurrentText.Length > 0)
                    {
                        if (History.Count > HistoryCount)
                        {
                            History.RemoveAt(0);
                        }
                        History.Add(CurrentText);
                        HistoryOffset = History.Count;
                    }

                    #endregion

                    PrintedConsoleText += Configs.OperatorName + Configs.OperatorPrefix + CurrentText + "\n";

                    try
                    {
                        Console.SetBufferSize(Console.BufferWidth, Console.BufferHeight + (CurrentText.Length / Console.BufferWidth));
                    }
                    catch { }
                    Cursor.SetPosition(0, Cursor.Top + 1);

                    #region Сброс переменных

                    RightOffset = 0;

                    #endregion

                    break;
                }

                if (key.Key == ConsoleKey.Backspace)
                {
                    if (CurrentText.Length > 0)
                    {
                        #region Логика изменения текста

                        CurrentText = CurrentText.Remove((CurrentText.Length - 1) - RightOffset, 1);

                        #endregion

                        #region Логика визуального изменения текста в консоли

                        if (Console.CursorLeft - 1 < 0)
                        {
                            Console.SetCursorPosition(Console.BufferWidth - 1, Console.CursorTop - 1);
                        }
                        else
                        {
                            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                        }

                        string clearString = "";
                        for (int i = 0; i < RightOffset + 1; i += 1)
                        {
                            clearString += " ";
                        }

                        Console.Write(clearString);

                        if (Cursor.Left - 1 < 0)
                        {
                            Console.SetCursorPosition(Console.BufferWidth - 1, Cursor.Top - 1);
                        }
                        else
                        {
                            Console.SetCursorPosition(Cursor.Left - 1, Cursor.Top);
                        }

                        string addString = "";

                        for (int i = (CurrentText.Length - 1) - (RightOffset - 1); i < CurrentText.Length; i += 1)
                        {
                            addString += CurrentText[i];
                        }

                        Console.Write(addString);

                        if (Cursor.Left - 1 < 0)
                        {
                            Cursor.SetPosition(Console.BufferWidth - 1, Cursor.Top - 1);
                        }
                        else
                        {
                            Cursor.SetPosition(Cursor.Left - 1, Cursor.Top);
                        }

                        #endregion
                    }
                    continue;
                }

                if (key.Key == ConsoleKey.Delete)
                {
                    #region Логика изменения текста

                    if (RightOffset == 0)
                    {
                        continue;
                    }
                    else
                    {
                        CurrentText = CurrentText.Remove(CurrentText.Length - RightOffset, 1);
                    }

                    #endregion

                    #region Логика визуального изменения текста в консоли

                    string clearText = "";
                    for (int i = 0; i < RightOffset; i += 1)
                    {
                        clearText += " ";
                    }
                    Console.Write(clearText);
                    Console.SetCursorPosition(Cursor.Left, Cursor.Top);

                    string addString = "";
                    for (int i = (CurrentText.Length + 1) - RightOffset; i < CurrentText.Length; i += 1)
                    {
                        addString += CurrentText[i];
                    }
                    Console.Write(addString);
                    Console.SetCursorPosition(Cursor.Left, Cursor.Top);
                    RightOffset -= 1;

                    #endregion

                    continue;
                }

                if (key.Key == ConsoleKey.LeftArrow)
                {
                    if (RightOffset + 1 <= CurrentText.Length)
                    {
                        Cursor.MoveLeft(1);
                        RightOffset += 1;
                    }

                    continue;
                }

                if (key.Key == ConsoleKey.RightArrow)
                {
                    if (RightOffset - 1 >= 0)
                    {
                        Cursor.MoveRight(1);
                        RightOffset -= 1;
                    }

                    continue;
                }

                if (key.Key == ConsoleKey.UpArrow)
                {
                    if (History.Count == 0)
                    {
                        continue;
                    }
                    else
                    {
                        if (HistoryOffset - 1 >= 0)
                        {
                            HistoryOffset -= 1;
                        }
                        ReplaceCurrentWith(History[HistoryOffset]);

                        continue;
                    }
                }

                if (key.Key == ConsoleKey.DownArrow)
                {
                    if (HistoryOffset + 1 >= History.Count)
                    {
                        HistoryOffset = History.Count;

                        ReplaceCurrentWith("");

                        continue;
                    }
                    else
                    {
                        if (HistoryOffset + 1 <= History.Count)
                        {
                            HistoryOffset += 1;
                        }
                        ReplaceCurrentWith(History[HistoryOffset]);

                        continue;
                    }
                }

                if (key.Key == ConsoleKey.Tab)
                {
                    List<string> similarCommands = new List<string>();

                    //основные команды////////////////////////////////////////
                    foreach (string command in Commands.EngineCommands.Keys)
                    {
                        if (command.ToLower().Contains(CurrentText.ToLower()))
                        {
                            similarCommands.Add(command);
                        }
                    }
                    //////////////////////////////////////////////////////////

                    //добавленные команды/////////////////////////////////////
                    foreach (var command in Commands.AdditionalCommands.Keys)
                    {
                        foreach (string c in command.Keys)
                        {
                            if (c.ToLower().Contains(CurrentText.ToLower()))
                            {
                                similarCommands.Add(c);
                            }
                        }
                    }
                    ///////////////////////////////////////////////////////////

                    if (similarCommands.Count == 0)
                    {
                        Dictionary<string, string> additionalCommands = new Dictionary<string, string>();
                        foreach (var command in Commands.AdditionalCommands.Keys)
                        {
                            foreach (string c in command.Keys)
                            {
                                additionalCommands.Add(c, command[c]);
                            }
                        }
                        ShowHelpWindow(Commands.EngineCommands, additionalCommands);
                    }
                    else if (similarCommands.Count == 1)
                    {
                        #region Логика визуального изменения текста

                        string clearString = "";
                        for (int i = 0; i < CurrentText.Length; i++)
                        {
                            clearString += " ";
                        }

                        Cursor.MoveLeft(CurrentText.Length);
                        Console.Write(clearString);
                        Console.SetCursorPosition(Cursor.Left, Cursor.Top);

                        #region Логика изменения текста

                        CurrentText = similarCommands[0];

                        #endregion

                        ConsoleColor currentColor = Console.ForegroundColor;
                        Console.ForegroundColor = Configs.DetectedCommandTextColor;
                        Console.Write(CurrentText);
                        Cursor.Left = Console.CursorLeft;
                        Cursor.Top = Console.CursorTop;
                        RightOffset = 0;
                        Console.ForegroundColor = currentColor;

                        #endregion
                    }
                    else
                    {
                        Dictionary<string, string> filteredEngineCommands = new Dictionary<string, string>();
                        Dictionary<string, string> filteredadditionalCommands = new Dictionary<string, string>();

                        foreach (string similarCommand in similarCommands)
                        {
                            if (Commands.EngineCommands.ContainsKey(similarCommand))
                            {
                                filteredEngineCommands.Add(similarCommand, Commands.EngineCommands[similarCommand]);
                            }

                            foreach (var command in Commands.AdditionalCommands.Keys)
                            {
                                foreach (string c in command.Keys)
                                {
                                    if (command.ContainsKey(similarCommand))
                                    {
                                        filteredEngineCommands.Add(c, command[c]);
                                    }
                                }
                            }
                        }

                        ShowHelpWindow(filteredEngineCommands, filteredadditionalCommands);

                        string unionPart = CurrentText;
                        int minCommandLength = 999999;
                        foreach (string command in similarCommands)
                        {
                            if (command.Length < minCommandLength)
                            {
                                minCommandLength = command.Length;
                            }
                        }
                        bool equal = true;
                        for (int i = CurrentText.Length; i < minCommandLength; i += 1)
                        {
                            if (equal)
                            {
                                unionPart += similarCommands[0][unionPart.Length];
                            }
                            else
                            {
                                break;
                            }
                            foreach (string command in similarCommands)
                            {
                                string commandLenghtText = "";
                                for (int j = 0; j < unionPart.Length; j += 1)
                                {
                                    commandLenghtText += command.ToLower()[j];
                                }
                                if (commandLenghtText.ToLower() != unionPart.ToLower())
                                {
                                    equal = false;
                                }
                            }
                        }
                        string newText = "";
                        for (int i = 0; i < unionPart.Length - 1; i += 1)
                        {
                            newText += similarCommands[0][i];
                        }
                        ReplaceCurrentWith(newText);
                    }

                    continue;
                }

                #endregion

                if (RightOffset == 0)
                {

                    #region Логика изменения строки

                    CurrentText += key.KeyChar;

                    #endregion


                    #region Логика визуального изменения строки в консоли

                    //дописываем символ в конец, и проверяем как должен располагаться курсор

                    Console.ForegroundColor = Configs.DefaultInputTextColor;
                    Console.Write(key.KeyChar.ToString());
                    Console.ForegroundColor = ConsoleColor.White;

                    Console.SetCursorPosition(Cursor.Left, Cursor.Top);

                    if (Cursor.Left + 1 >= Console.BufferWidth)
                    {
                        Cursor.SetPosition(0, Cursor.Top + 1);
                    }
                    else
                    {
                        Cursor.SetPosition(Cursor.Left + 1, Cursor.Top);
                    }

                    #endregion
                }
                else
                {

                    #region Логика изменения строки

                    CurrentText = CurrentText.Insert(CurrentText.Length - RightOffset, key.KeyChar.ToString());

                    #endregion

                    #region Логика визуального изменения строки в консоли

                    //отчищаем текст с места изменения и с этого места пишем измененную строку

                    string clearString = "";
                    for (int i = 0; i < RightOffset; i += 1)
                    {
                        clearString += " ";
                    }

                    Console.Write(clearString);
                    Console.SetCursorPosition(Cursor.Left, Cursor.Top);

                    string addString = "";

                    for (int i = (CurrentText.Length - 1) - RightOffset; i < CurrentText.Length; i += 1)
                    {
                        addString += CurrentText[i];
                    }

                    Console.Write(addString);
                    addString = "";

                    if (Cursor.Left + 1 >= Console.BufferWidth)
                    {
                        Cursor.SetPosition(0, Cursor.Top + 1);
                    }
                    else
                    {
                        Cursor.SetPosition(Cursor.Left + 1, Cursor.Top);
                    }

                    #endregion

                }
            }

            return CurrentText;
        }


        /// <summary>
        /// Вывести текст в консоль
        /// </summary>
        /// <param name="text">Строка, которую необходимо вывести</param>
        public static void Print(string text)
        {
            //Console.SetBufferSize(Console.BufferWidth, Console.BufferHeight + (text.Length / Console.BufferWidth) + 1);

            ConsoleColor currentColor = Console.ForegroundColor;
            Console.ForegroundColor = Configs.DefaultPrintedTextColor;
            Console.Write(text);
            Console.ForegroundColor = currentColor;

            Cursor.Left = Console.CursorLeft;
            Cursor.Top = Console.CursorTop;

            PrintedConsoleText += text;

            ComputeBufferHeight();
        }

        /// <summary>
        /// Вывести текст в консоль
        /// </summary>
        /// <param name="text">Строка, которую необходимо вывести</param>
        /// <param name="textColor">Цвет выводимого текста</param>
        public static void Print(string text, ConsoleColor textColor)
        {
            //Console.SetBufferSize(Console.BufferWidth, Console.BufferHeight + (text.Length / Console.BufferWidth) + 1);

            ConsoleColor currentColor = Console.ForegroundColor;
            Console.ForegroundColor = textColor;
            Console.Write(text);
            Console.ForegroundColor = currentColor;

            Cursor.Left = Console.CursorLeft;
            Cursor.Top = Console.CursorTop;

            PrintedConsoleText += text;

            ComputeBufferHeight();
        }

        /// <summary>
        /// Вывести текст в консоль
        /// </summary>
        /// <param name="text">Строка, которую необходимо вывести</param>
        public static void PrintLine(string text)
        {
            //Console.SetBufferSize(Console.BufferWidth, Console.BufferHeight + (text.Length / Console.BufferWidth) + 1);

            ConsoleColor currentColor = Console.ForegroundColor;
            Console.ForegroundColor = Configs.DefaultPrintedTextColor;
            Console.WriteLine(text);
            Console.ForegroundColor = currentColor;

            Cursor.Left = Console.CursorLeft;
            Cursor.Top = Console.CursorTop;

            PrintedConsoleText += text + "\n";

            ComputeBufferHeight();
        }

        /// <summary>
        /// Вывести текст в консоль
        /// </summary>
        /// <param name="text">Строка, которую необходимо вывести</param>
        /// <param name="textColor">Цвет выводимого текста</param>
        public static void PrintLine(string text, ConsoleColor textColor)
        {
            //Console.SetBufferSize(Console.BufferWidth, Console.BufferHeight + (text.Length / Console.BufferWidth) + 1);

            ConsoleColor currentColor = Console.ForegroundColor;
            Console.ForegroundColor = textColor;
            Console.WriteLine(text);
            Console.ForegroundColor = currentColor;

            Cursor.Left = Console.CursorLeft;
            Cursor.Top = Console.CursorTop;

            PrintedConsoleText += text + "\n";

            ComputeBufferHeight();
        }

        /// <summary>
        /// Заменить текущий введенный текст
        /// </summary>
        /// <param name="text">Новый текст вместо введенного</param>
        public static void ReplaceCurrentWith(string text)
        {
            string clearString = "";
            for (int i = 0; i < CurrentText.Length; i += 1)
            {
                clearString += " ";
            }
            Cursor.MoveLeft(CurrentText.Length - RightOffset);
            Console.Write(clearString);
            RightOffset = 0;
            Console.SetCursorPosition(Cursor.Left, Cursor.Top);
            Console.Write(text);
            Cursor.Left = Console.CursorLeft;
            Cursor.Top = Console.CursorTop;

            CurrentText = text;

        }

        private static void ComputeBufferHeight()
        {
            #region Расширение BufferHeight

            int height = (CurrentText.Length / Configs.MinWidth) + PrintedConsoleText.Split('\n').Length;
            foreach (string text in PrintedConsoleText.Split('\n'))
            {
                int localheight = text.Length / Configs.MinWidth;
                height += localheight;
            }
            try
            {
                Console.SetBufferSize(Console.BufferWidth, Console.WindowHeight + height);
            }
            catch { }

            #endregion
        }


        private static void ShowHelpWindow(Dictionary<string, string> engineCommands, Dictionary<string, string> additionalCommands)
        {
            if (!IsHelpShow)
            {
                IsHelpShow = true;

                int currentLeft = Cursor.Left;
                int currentTop = Cursor.Top;

                string msg = "Основные доступные команды:";
                Cursor.SetPosition((Console.BufferWidth - msg.Length) / 2, Cursor.Top + 2);
                PrintLine(msg, Configs.DefaultPrintedTextColor);

                string delimiter = "";
                for (int i = 0; i < Console.BufferWidth; i += 1)
                {
                    delimiter += "_";
                }

                Cursor.SetPosition(0, Cursor.Top + 1);
                PrintLine(delimiter);

                foreach (string command in engineCommands.Keys)
                {
                    Print(command, Configs.AviableConsoleCommandColor);
                    Cursor.SetPosition(40, Cursor.Top);
                    PrintLine(engineCommands[command], Configs.AviableConsoleCommandDescriptionColor);
                }
                PrintLine(delimiter);

                if (additionalCommands.Count == 0)
                {
                    msg = "Дополнительных команд нет";
                    Cursor.SetPosition((Console.BufferWidth - msg.Length) / 2, Cursor.Top + 2);
                    PrintLine(msg, Configs.DefaultPrintedTextColor);
                }
                else
                {
                    msg = "Дополнительные доступные команды:";
                    Cursor.SetPosition((Console.BufferWidth - msg.Length) / 2, Cursor.Top + 2);
                    PrintLine(msg, Configs.DefaultPrintedTextColor);

                    Cursor.SetPosition(0, Cursor.Top + 1);
                    PrintLine(delimiter);

                    foreach (var command in additionalCommands.Keys)
                    {
                        Print(command, Configs.AviableConsoleCommandColor);
                        Cursor.SetPosition(40, Cursor.Top);
                        PrintLine(additionalCommands[command], Configs.AviableConsoleCommandDescriptionColor);
                    }
                    PrintLine(delimiter);
                }

                HelpWindowHeight = (Cursor.Top - currentTop);
                Cursor.SetPosition(currentLeft, currentTop);
            }
        }

        private static void HideHelpWindow()
        {
            if (Console.BufferHeight < HelpWindowHeight + Console.WindowHeight)
            {
                try
                {
                    Console.SetBufferSize(Console.BufferWidth, Console.WindowHeight + HelpWindowHeight);
                }
                catch { }
            }
            if (IsHelpShow)
            {
                IsHelpShow = false;

                string clearString = "";
                Console.SetCursorPosition(0, Console.CursorTop + 2);
                for (int i = 0; i < Console.BufferWidth; i++)
                {
                    clearString += " ";
                }

                for (int i = 0; i < HelpWindowHeight; i++)
                {
                    Console.WriteLine(clearString);
                }

                Console.SetCursorPosition(Cursor.Left, Cursor.Top);
            }
        }

        public static int CurrentWidth;
        public static int CurrentHeight;
        public static bool IsWork { get; private set; }
        private static Task Worker;

        /// <summary>
        /// Запустить движок консоли. 
        /// Необходимо для того, чтобы в отдельном потоке считывался размер консоли 
        /// и слова переносились в соответсвии с размером при изменении размера консоли.
        /// </summary>
        /// <param name="currentSize">Текущий размер консоли</param>
        public static void Run()
        {
            #region Инициализация

            try
            {
                Console.SetWindowSize(Configs.DefaultWidth, Configs.DefaultHeight);
                Console.SetBufferSize(Configs.DefaultWidth, Configs.DefaultHeight);
                Console.CursorSize = Configs.CursorSize;
            }
            catch 
            {
                for (int i = 0; i < 10000; i += 1)
                {
                    Console.WriteLine("");
                }
                try
                {
                    while (true)
                    {
                        Console.SetCursorPosition(Cursor.Left, Console.CursorTop - 1);
                    }
                }
                catch { }

                Console.WriteLine(Console.BufferHeight);
            }

            LeftBorder = Configs.OperatorName.Length + Configs.OperatorPrefix.Length;
            if (LeftBorder > Configs.MinWidth)
                Configs.MinWidth = LeftBorder + 10;

            History = new List<string>();
            HistoryOffset = 0;
            HistoryCount = Configs.HistoryCount;

            CurrentText = "";
            PrintedConsoleText = "";

            Cursor.Left = Console.CursorLeft;
            Cursor.Top = Console.CursorTop;
            RightOffset = 0;

            #endregion

            CurrentWidth = Console.WindowWidth;
            CurrentHeight = Console.WindowHeight;
            Worker = new Task(() =>
            {
                #region CheckSize

                while (true)
                {
                    if (Console.WindowWidth != CurrentWidth)
                    {
                        CurrentWidth = Console.WindowWidth;

                        Cursor.Top = Console.CursorTop;
                        Cursor.Left = Console.CursorLeft;
                    }

                    if (Console.WindowHeight != CurrentHeight)
                    {
                        CurrentHeight = Console.WindowHeight;

                        Cursor.Top = Console.CursorTop;
                        Cursor.Left = Console.CursorLeft;
                    }

                    if (Console.WindowWidth < Configs.MinWidth)
                    {
                        try
                        {
                            Console.SetWindowSize(Configs.MinWidth, CurrentHeight);
                        }
                        catch { }
                    }

                    if (Console.WindowHeight < Configs.MinHeight)
                    {
                        try
                        {
                            Console.SetWindowSize(CurrentWidth, Configs.MinHeight);
                        }
                        catch { }
                    }
                }

                #endregion
            });
            Worker.Start();
            IsWork = true;
        }

        /// <summary>
        /// Остановить работу движка консоли
        /// </summary>
        public static void Stop()
        {
            #region Чистка

            History.Clear();
            HistoryOffset = 0;
            HistoryCount = 0;
            PrintedConsoleText = "";

            #endregion

            Worker.Dispose();
            IsWork = false;
        }
    }
}
