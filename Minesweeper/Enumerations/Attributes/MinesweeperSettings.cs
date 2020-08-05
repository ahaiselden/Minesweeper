using System;

namespace Minesweeper.Enumerations.Attributes
{
    public class MinesweeperSettings : Attribute
    {
        public MinesweeperSettings(int x, int y, int mineCount)
        {
            X = x;
            Y = y;
            MineCount = mineCount;
        }

        public int X { get; set; }

        public int Y { get; set; }

        public int MineCount { get; set; }
    }
}
