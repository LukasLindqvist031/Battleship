using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    public static class TextPresentation
    {
        public static void WriteCenteredTextWithDelay(string text, int delay = 50, bool leaveCursorBelow = false, int? yPos = null)
        {
            int consoleWidth = Console.WindowWidth;
            int consoleHeight = Console.WindowHeight;

            int xPos = (consoleWidth - text.Length) / 2;
            int yPosition = yPos ?? consoleHeight / 2;

            Console.CursorVisible = false;

            Console.SetCursorPosition(xPos, yPosition);

            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(delay);
            }

            if (leaveCursorBelow)
            {
                Console.SetCursorPosition(xPos, yPosition + 1);
            }
            else
            {
                Console.WriteLine();
            }
        }
    }

}