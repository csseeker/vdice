using System.Text.Json;

namespace VDice;

public class StatsTracker
{
    private List<int> _history = [];
    private readonly string _filePath;

    public StatsTracker()
    {
        _filePath = Path.Combine(AppContext.BaseDirectory, "stats.json");
    }

    // --- Data access ---

    public int TotalRolls => _history.Count;
    public double Average => _history.Count > 0 ? _history.Average() : 0;
    public IReadOnlyList<int> History => _history.AsReadOnly();

    /// <summary>
    /// Returns the count for each face value (index 0 = face 1, etc.)
    /// </summary>
    public int[] GetCounts()
    {
        int[] counts = new int[6];
        foreach (int v in _history)
            counts[v - 1]++;
        return counts;
    }

    // --- Mutations ---

    public void Record(int value)
    {
        _history.Add(value);
    }

    public void Reset()
    {
        _history.Clear();
    }

    // --- Persistence ---

    public void Save()
    {
        try
        {
            string json = JsonSerializer.Serialize(_history);
            File.WriteAllText(_filePath, json);
        }
        catch
        {
            // Silently ignore write errors (e.g. read-only filesystem)
        }
    }

    public void Load()
    {
        try
        {
            if (File.Exists(_filePath))
            {
                string json = File.ReadAllText(_filePath);
                _history = JsonSerializer.Deserialize<List<int>>(json) ?? [];
            }
        }
        catch
        {
            _history = [];
        }
    }

    // --- Rendering ---

    /// <summary>
    /// Render the stats table at the given console position.
    /// Table width = 30 chars, height = 14 lines.
    /// </summary>
    public void RenderStats(int left, int top)
    {
        int[] counts = GetCounts();
        double expectedPct = 100.0 / 6.0; // ~16.67%

        //  Table layout (30 chars wide):
        //  ┌────────────────────────────┐
        //  │     📊 STATISTICS          │   row 0-1
        //  ├────────┬───────┬───────────┤   row 2
        //  │  Face  │ Count │  Freq %   │   row 3
        //  ├────────┼───────┼───────────┤   row 4
        //  │  [1]   │    0  │    0.0%   │   rows 5-10
        //  ├────────┼───────┼───────────┤   row 11
        //  │ Total  │    0  │  Avg ─.── │   row 12
        //  └────────┴───────┴───────────┘   row 13

        void WriteRow(int row, string content)
        {
            Console.SetCursorPosition(left, top + row);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(content);
        }

        // Title
        WriteRow(0, "┌────────────────────────────┐");

        Console.SetCursorPosition(left, top + 1);
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("│");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("     📊 STATISTICS         ");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write(" │");

        WriteRow(2, "├────────┬───────┬───────────┤");

        // Header
        Console.SetCursorPosition(left, top + 3);
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("│");
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.Write("  Face  ");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("│");
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.Write(" Count ");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("│");
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.Write("  Freq %  ");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write(" │");

        WriteRow(4, "├────────┼───────┼───────────┤");

        // Per-face rows
        for (int i = 0; i < 6; i++)
        {
            double pct = TotalRolls > 0 ? (counts[i] * 100.0 / TotalRolls) : 0;

            Console.SetCursorPosition(left, top + 5 + i);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("│");

            // Face number (8 chars)
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"  [{i + 1}]   ");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("│");

            // Count (7 chars)
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"{counts[i],5}  ");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("│");

            // Percentage (10 chars)
            if (TotalRolls == 0)
                Console.ForegroundColor = ConsoleColor.DarkGray;
            else if (pct >= expectedPct)
                Console.ForegroundColor = ConsoleColor.Green;
            else
                Console.ForegroundColor = ConsoleColor.Red;

            Console.Write($"{pct,6:F1}%   ");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(" │");
        }

        // Separator before summary
        WriteRow(11, "├────────┴───────┼───────────┤");

        // Summary row: Total count + Average
        Console.SetCursorPosition(left, top + 12);
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("│");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write(" Total  ");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("│");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write($"{TotalRolls,5}  ");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("│");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write(" Avg ");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write(TotalRolls > 0 ? $"{Average:F2}" : "─.──");
        Console.Write(" ");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write(" │");

        WriteRow(13, "└────────┴───────┴───────────┘");

        Console.ResetColor();
    }

    /// <summary>
    /// Clear the stats area (overwrite with spaces).
    /// </summary>
    public void ClearStats(int left, int top)
    {
        Console.ResetColor();
        string blank = new(' ', 30);
        for (int i = 0; i < 14; i++)
        {
            Console.SetCursorPosition(left, top + i);
            Console.Write(blank);
        }
    }
}
