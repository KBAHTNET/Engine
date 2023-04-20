using System;

namespace ConsoleEngine
{
    public static class Cursor
    {
        /// <summary>
        /// Текущее местоположение курсора по вертикали
        /// </summary>
        public static int Top;

        /// <summary>
        /// Текущее местоположение курсора по горизонтали
        /// </summary>
        public static int Left;

        /// <summary>
        /// Сдвинуть курсон на некоторое количество шагов влево
        /// </summary>
        /// <param name="count">Кол-во шагов</param>
        public static void MoveLeft(int count)
        {
            int Up = 0;

            while ((Left + (Console.BufferWidth * Up)) - count < 0)
            {
                Up += 1;
            }

            Top -= Up;
            Left = Left - (count - (Console.BufferWidth * Up));
            SetPosition(Left, Top);
        }

        /// <summary>
        /// Сдвинуть курсон на некоторое количество шагов влево
        /// </summary>
        /// <param name="count">Кол-во шагов</param>
        public static void MoveRight(int count)
        {
            Top += (Left + count) / Console.BufferWidth;
            Left = (Left + count) % Console.BufferWidth;
            SetPosition(Left, Top);
        }

        /// <summary>
        /// Устнавливает позицию курсора в консоли
        /// </summary>
        /// <param name="left">Смещение слева-направо</param>
        /// <param name="top">Смещение сверху-вниз</param>
        public static void SetPosition(int left, int top)
        {
            if(top + left / Console.BufferWidth + 1 >= Console.BufferHeight)
            {
                try
                {
                    Console.SetBufferSize(Console.BufferWidth, Console.BufferHeight + 1);
                }
                catch { }
            }
            Console.SetCursorPosition(left % Console.BufferWidth, top + left / Console.BufferWidth);
            Left = Console.CursorLeft;
            Top = Console.CursorTop;
        }
    }
}
