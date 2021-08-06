using System;

namespace ConsoleEngine
{
    public static class Configs
    {
        // Лимит запоминания введенных команд
        public static int HistoryCount = 30;

        // Минимальные размеры консоли
        public static int MinWidth = 30;
        public static int MinHeight = 10;

        // Размеры при запуске
        public static int DefaultWidth = 85;
        public static int DefaultHeight = 15;

        // Размер курсора
        public static int CursorSize = 100;

        // Идентификатор
        public static string OperatorName = "admin";
        public static string OperatorPrefix = "#";

        // Цвета для идентификатора
        public static ConsoleColor OperatorNameColor = ConsoleColor.Green;
        public static ConsoleColor OperatorPrefixColor = ConsoleColor.Green;

        // Цвета введнных команд
        public static ConsoleColor DetectedCommandText = ConsoleColor.Green;
        public static ConsoleColor CommandArgument = ConsoleColor.Yellow;
        public static ConsoleColor CommandSplitters = ConsoleColor.Cyan;

        // Стандартный цвет текста
        public static ConsoleColor DefaultInputTextColor = ConsoleColor.White;
        public static ConsoleColor DefaultPrintedTextColor = ConsoleColor.White;
    }
}
