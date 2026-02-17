# 🎲 VDice

A beautiful console-based dice roller application built with C# and .NET.

## Features

- **Visual Dice Rolling**: Animated dice with ASCII art rendering
- **Statistics Tracking**: Records all rolls with persistent storage
- **Real-time Stats**: View roll counts, averages, and distribution
- **Clean Interface**: Minimalist console UI with colors and Unicode symbols

## Requirements

- .NET 10.0 or later
- Terminal with Unicode support
- Minimum terminal size: 52x22 characters

## Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/csseeker/vdice.git
   cd vdice
   ```

2. Build the project:
   ```bash
   dotnet build
   ```

3. Run the application:
   ```bash
   dotnet run
   ```

## Usage

### Controls

- **`SPACE`** — Roll the dice
- **`R`** — Reset all statistics
- **`ESC`** — Exit the application

### How It Works

1. Press `SPACE` to roll a six-sided die
2. Watch the animated dice roll
3. View your rolling statistics in real-time
4. All roll history is automatically saved to `stats.json`

## Project Structure

```
vdice/
├── Program.cs         # Main application logic and UI
├── DiceRenderer.cs    # Dice animation and ASCII art
├── StatsTracker.cs    # Statistics tracking and persistence
└── vdice.csproj       # Project configuration
```

## Features in Detail

### Dice Animation
The dice uses box-drawing characters to create a visually appealing 3D effect with proper dot positioning for all six faces.

### Statistics Persistence
Roll history is automatically saved to `stats.json` in the application directory, allowing you to track your rolling patterns over time.

### Roll Distribution
The stats panel shows:
- Total number of rolls
- Average roll value
- Count and percentage for each face (1-6)

## License

This project is open source and available under the MIT License.

## Contributing

Contributions are welcome! Feel free to submit issues or pull requests.
