using VDice;

// ── Layout constants ──────────────────────────────────────────
const int DiceLeft = 4;
const int DiceTop = 5;
const int StatsLeft = 18;
const int StatsTop = 4;
const int MinWidth = 52;
const int MinHeight = 22;

// ── Setup ─────────────────────────────────────────────────────
Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.Title = "🎲 VDice — Console Dice Roller";
Console.CursorVisible = false;

// Check terminal size
if (Console.WindowWidth < MinWidth || Console.WindowHeight < MinHeight)
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine($"⚠  Terminal too small ({Console.WindowWidth}x{Console.WindowHeight}).");
    Console.WriteLine($"   Please resize to at least {MinWidth}x{MinHeight} and restart.");
    Console.ResetColor();
    Console.CursorVisible = true;
    return;
}

var stats = new StatsTracker();
stats.Load();

DrawScreen(stats);

// ── Main loop ─────────────────────────────────────────────────
while (true)
{
    var key = Console.ReadKey(true);

    switch (key.Key)
    {
        case ConsoleKey.Spacebar:
            int value = Random.Shared.Next(1, 7);
            DiceRenderer.Animate(value, DiceLeft, DiceTop);
            ShowLastRoll(value);
            stats.Record(value);
            stats.Save();
            stats.RenderStats(StatsLeft, StatsTop);
            ShowHint();
            break;

        case ConsoleKey.R:
            stats.Reset();
            stats.Save();
            Console.Clear();
            DrawScreen(stats);
            ShowResetMessage();
            break;

        case ConsoleKey.Escape:
            Console.ResetColor();
            Console.Clear();
            Console.CursorVisible = true;

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Thanks for playing VDice! 🎲");
            Console.ResetColor();
            return;
    }
}

// ── Helper methods ────────────────────────────────────────────

void DrawScreen(StatsTracker tracker)
{
    Console.Clear();

    // Title banner
    Console.SetCursorPosition(2, 0);
    Console.ForegroundColor = ConsoleColor.Black;
    Console.BackgroundColor = ConsoleColor.Magenta;
    Console.Write("  🎲  V D I C E  ");
    Console.ResetColor();

    Console.SetCursorPosition(21, 0);
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.Write(" Console Dice Roller");
    Console.ResetColor();

    // Separator
    Console.SetCursorPosition(2, 1);
    Console.ForegroundColor = ConsoleColor.DarkMagenta;
    Console.Write("══════════════════════════════════════════════");
    Console.ResetColor();

    // Controls legend
    Console.SetCursorPosition(2, 2);
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.Write("  ");

    Console.ForegroundColor = ConsoleColor.White;
    Console.BackgroundColor = ConsoleColor.DarkGray;
    Console.Write(" SPACE ");
    Console.ResetColor();
    Console.ForegroundColor = ConsoleColor.Gray;
    Console.Write(" Roll  ");

    Console.ForegroundColor = ConsoleColor.White;
    Console.BackgroundColor = ConsoleColor.DarkGray;
    Console.Write(" R ");
    Console.ResetColor();
    Console.ForegroundColor = ConsoleColor.Gray;
    Console.Write(" Reset  ");

    Console.ForegroundColor = ConsoleColor.White;
    Console.BackgroundColor = ConsoleColor.DarkGray;
    Console.Write(" ESC ");
    Console.ResetColor();
    Console.ForegroundColor = ConsoleColor.Gray;
    Console.Write(" Quit");

    Console.ResetColor();

    // Separator
    Console.SetCursorPosition(2, 3);
    Console.ForegroundColor = ConsoleColor.DarkMagenta;
    Console.Write("══════════════════════════════════════════════");
    Console.ResetColor();

    // "Dice" label
    Console.SetCursorPosition(DiceLeft, DiceTop - 1);
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.Write("  🎯 Die");
    Console.ResetColor();

    // Draw stats (possibly restored from previous session)
    tracker.RenderStats(StatsLeft, StatsTop);

    // Show welcome or continuation prompt (below dice area)
    if (tracker.TotalRolls == 0)
    {
        Console.SetCursorPosition(DiceLeft, DiceTop + 8);
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("Hit ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("SPACE");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("!");
        Console.ResetColor();
    }
    else
    {
        Console.SetCursorPosition(DiceLeft, DiceTop + 8);
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("Roll again!");
        Console.ResetColor();
    }

    ShowHint();
}

void ShowLastRoll(int value)
{
    Console.SetCursorPosition(DiceLeft, DiceTop + 8);
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.Write("Last roll: ");
    Console.ForegroundColor = ConsoleColor.White;
    Console.BackgroundColor = ConsoleColor.DarkMagenta;
    Console.Write($" {value} ");
    Console.ResetColor();
    Console.Write("   "); // clear any leftover chars
}

void ShowResetMessage()
{
    Console.SetCursorPosition(DiceLeft, DiceTop + 8);
    Console.ForegroundColor = ConsoleColor.Green;
    Console.Write("✔ Reset!    ");
    Console.ResetColor();
}

void ShowHint()
{
    int hintRow = StatsTop + 14;
    Console.SetCursorPosition(2, hintRow);
    Console.ForegroundColor = ConsoleColor.DarkMagenta;
    Console.Write("══════════════════════════════════════════════");
    Console.ResetColor();

    Console.SetCursorPosition(2, hintRow + 1);
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.Write("  Session stats saved automatically • ");
    Console.ForegroundColor = ConsoleColor.DarkYellow;
    Console.Write("R");
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.Write(" to reset");
    Console.ResetColor();
}
