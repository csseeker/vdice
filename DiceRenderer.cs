namespace VDice;

public static class DiceRenderer
{
    // Each face is 7 lines tall, 9 chars wide (using box-drawing)
    private static readonly string[][] Faces =
    [
        // Face 1
        [
            "╔═══════╗",
            "║       ║",
            "║       ║",
            "║   ●   ║",
            "║       ║",
            "║       ║",
            "╚═══════╝"
        ],
        // Face 2
        [
            "╔═══════╗",
            "║     ● ║",
            "║       ║",
            "║       ║",
            "║       ║",
            "║ ●     ║",
            "╚═══════╝"
        ],
        // Face 3
        [
            "╔═══════╗",
            "║     ● ║",
            "║       ║",
            "║   ●   ║",
            "║       ║",
            "║ ●     ║",
            "╚═══════╝"
        ],
        // Face 4
        [
            "╔═══════╗",
            "║ ●   ● ║",
            "║       ║",
            "║       ║",
            "║       ║",
            "║ ●   ● ║",
            "╚═══════╝"
        ],
        // Face 5
        [
            "╔═══════╗",
            "║ ●   ● ║",
            "║       ║",
            "║   ●   ║",
            "║       ║",
            "║ ●   ● ║",
            "╚═══════╝"
        ],
        // Face 6
        [
            "╔═══════╗",
            "║ ●   ● ║",
            "║       ║",
            "║ ●   ● ║",
            "║       ║",
            "║ ●   ● ║",
            "╚═══════╝"
        ]
    ];

    /// <summary>
    /// Render a dice face at the given console position with color.
    /// </summary>
    public static void Render(int value, int left, int top)
    {
        var face = Faces[value - 1];
        for (int i = 0; i < face.Length; i++)
        {
            Console.SetCursorPosition(left, top + i);
            foreach (char c in face[i])
            {
                if (c == '●')
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.Write(c);
                }
                else if (c == '║' || c == '╔' || c == '╗' || c == '╚' || c == '╝' || c == '═')
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write(c);
                }
                else // spaces inside the die
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.Write(c);
                }
            }
            Console.ResetColor();
        }
    }

    /// <summary>
    /// Animate a dice roll: show 5 intermediate random faces before landing on the final value.
    /// Total animation time ~400ms.
    /// </summary>
    public static void Animate(int finalValue, int left, int top)
    {
        int[] delays = [60, 70, 80, 90, 100];
        int prev = -1;
        for (int i = 0; i < delays.Length; i++)
        {
            int v;
            do { v = Random.Shared.Next(1, 7); } while (v == prev);
            prev = v;
            Render(v, left, top);
            Thread.Sleep(delays[i]);
        }
        // Final face
        Render(finalValue, left, top);
    }

    /// <summary>
    /// Clear the dice area (overwrite with spaces).
    /// </summary>
    public static void Clear(int left, int top)
    {
        Console.ResetColor();
        string blank = new(' ', 9);
        for (int i = 0; i < 7; i++)
        {
            Console.SetCursorPosition(left, top + i);
            Console.Write(blank);
        }
    }
}
