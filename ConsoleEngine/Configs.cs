using System;

namespace ConsoleEngine
{
    public static class Configs
    {
        // Лимит запоминания введенных команд
        public static int HistoryCount = 30;

        // Минимальные размеры консоли
        public static int MinWidth = 75;
        public static int MinHeight = 15;

        // Размеры при запуске
        public static int DefaultWidth = 75;
        public static int DefaultHeight = 15;

        // Размер курсора
        public static int CursorSize = 100;

        // Идентификатор
        public static string OperatorName = "admin";
        public static string OperatorPrefix = "#";

        // Цвета для идентификатора
        public static ConsoleColor OperatorNameColor = ConsoleColor.Magenta;
        public static ConsoleColor OperatorPrefixColor = ConsoleColor.DarkMagenta;

        // Цвета введнных команд
        public static ConsoleColor DetectedCommandTextColor = ConsoleColor.Green;
        public static ConsoleColor CommandArgumentColor = ConsoleColor.Yellow;
        public static ConsoleColor CommandSplittersColor = ConsoleColor.Cyan;

        // Стандартный цвет текста
        public static ConsoleColor DefaultInputTextColor = ConsoleColor.White;
        public static ConsoleColor DefaultPrintedTextColor = ConsoleColor.White;

        //Цвет подсказок
        public static ConsoleColor AviableConsoleCommandColor = ConsoleColor.Cyan;
        public static ConsoleColor AviableConsoleCommandDescriptionColor = ConsoleColor.DarkCyan;
        public static ConsoleColor AdditionalCommands = ConsoleColor.DarkBlue;
    }
}
