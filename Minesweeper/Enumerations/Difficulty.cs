using Minesweeper.Enumerations.Attributes;

namespace Minesweeper.Enumerations
{
    public enum Difficulty
    {
        [MinesweeperSettings(9, 9, 10)]
        Beginner,

        [MinesweeperSettings(16, 16, 40)]
        Intermediate,

        [MinesweeperSettings(30, 16, 99)]
        Expert
    }
}
