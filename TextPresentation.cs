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
        // Method to display text centered and letter-by-letter
        public static void WriteCenteredTextWithDelay(string text, int delay = 50, bool leaveCursorBelow = false, int? yPos = null)
        {
            int consoleWidth = Console.WindowWidth;
            int consoleHeight = Console.WindowHeight;

            int xPos = (consoleWidth - text.Length) / 2;
            int yPosition = yPos ?? consoleHeight / 2; // Use yPos if provided; otherwise, center vertically

            Console.CursorVisible = false;  // Hide the cursor while typing

            Console.SetCursorPosition(xPos, yPosition);

            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(delay);  // Adjust delay for speed
            }

            if (leaveCursorBelow)
            {
                // Position cursor in the middle of the screen, below the prompt text
                Console.SetCursorPosition(xPos, yPosition + 1);
            }
            else
            {
                Console.WriteLine();  // Move to the next line after the text
            }
        }
    }

}