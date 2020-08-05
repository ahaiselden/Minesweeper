using Minesweeper.Enumerations;
using Minesweeper.Enumerations.Attributes;
using Minesweeper.Models;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Minesweeper.Game
{
    public class Minesweeper
    {
        public Tile[,] Tiles { get; set; }

        public GameState State { get; set; }

        public Difficulty Difficulty { get; }

        public int UnflaggedCount { get; set; }

        private int sizeX;
        private int sizeY;
        private int mineCount;

        private int tileCount;
        private int investigatedTileCount;

        private readonly List<Point> surroundingPointModifiers = new List<Point>(){
            new Point() { X = -1, Y = -1 },
            new Point() { X = -1, Y = +1 },
            new Point() { X = 0, Y = -1 },
            new Point() { X = 0, Y = +1 },
            new Point() { X = +1, Y = -1 },
            new Point() { X = +1, Y = 0 },
            new Point() { X = +1, Y = +1 },
            new Point() { X = -1, Y = 0 },
        };

        public Minesweeper(Difficulty difficulty)
        {
            this.Difficulty = difficulty;
            MinesweeperSettings minesweeperSettings = difficulty.GetMinesweeperSettings();

            sizeX = minesweeperSettings.X - 1;
            sizeY = minesweeperSettings.Y - 1;
            mineCount = minesweeperSettings.MineCount;
            UnflaggedCount = mineCount;

            tileCount = 0;
            investigatedTileCount = 0;

            Tiles = new Tile[sizeX, sizeY];

            for (int x = 0; x < sizeX; x++)
            {
                for (int y = 0; y < sizeY; y++)
                {
                    Tiles[x, y] = new Tile();
                }
            }

            investigatedTileCount = 0;
            tileCount = sizeX * sizeY;

            Random random = new Random();
            int minesCreated = 0;

            while (mineCount > minesCreated)
            {
                int randomX = random.Next(0, sizeX);
                int randomY = random.Next(0, sizeY);

                if (Tiles[randomX, randomY].HasMine == false)
                {
                    Tiles[randomX, randomY].HasMine = true;

                    AddSurroundingMines(randomX, randomY);

                    minesCreated++;
                }
            }

            State = GameState.InProgress;
        }

        public void TileClicked(int x, int y, long mouseButton)
        {
            if (Tiles[x, y].IsInspected)
            {
                // Chording, Middle Button.
                if (mouseButton == 1)
                {
                    if (CountSurroundingFlags(x, y) == Tiles[x, y].SurroundingMines)
                    {
                        ExposeSurroundingArea(x, y, false);
                        State = GameState.InProgress;
                    }
                    else
                    {
                        State = GameState.Lost;
                    }
                }
            }
            else
            {
                if (mouseButton == 0)
                {
                    Tiles[x, y].IsInspected = true;

                    if (Tiles[x, y].HasMine)
                    {
                        State = GameState.Lost;
                    }
                    else
                    {
                        investigatedTileCount++;

                        if (Tiles[x, y].HasFlag)
                        {
                            Tiles[x, y].HasFlag = false;
                            UnflaggedCount++;
                        }

                        if (Tiles[x, y].SurroundingMines == 0)
                        {
                            ExposeSurroundingArea(x, y);
                        }

                        State = GameState.InProgress;
                    }
                }
                else if (mouseButton == 2)
                {
                    if (Tiles[x, y].HasFlag)
                    {
                        Tiles[x, y].HasFlag = false;
                        UnflaggedCount++;

                    }
                    else
                    {
                        Tiles[x, y].HasFlag = true;
                        UnflaggedCount--;
                    }
                }
            }

            if ((mineCount + investigatedTileCount) >= tileCount)
            {
                State = GameState.Won;
            }
        }

        public string GetImageFromState()
        {
            switch(State)
            {
                case GameState.InProgress:
                    return "inprogress.png";
                case GameState.InPlay:
                    return "inplay.png";
                case GameState.Lost:
                    return "lost.png";
                case GameState.Won:
                    return "won.png";
                default:
                    return "inprogress.png";
            }
        }

        private void ExposeSurroundingArea(int x, int y, bool includeFlags = false)
        {
            List<Point> checkPoints = GetSurroundingPoints(x, y);

            foreach (Point checkPoint in checkPoints)
            {
                if (!Tiles[checkPoint.X, checkPoint.Y].IsInspected)
                {
                    if ((includeFlags == false && !Tiles[checkPoint.X, checkPoint.Y].HasFlag) || (includeFlags == true && Tiles[checkPoint.X, checkPoint.Y].HasFlag))
                    {
                        Tiles[checkPoint.X, checkPoint.Y].IsInspected = true;
                        investigatedTileCount++;

                        if (Tiles[checkPoint.X, checkPoint.Y].SurroundingMines == 0)
                        {
                            ExposeSurroundingArea(checkPoint.X, checkPoint.Y);
                        }
                    }
                }
            }
        }

        private void AddSurroundingMines(int x, int y)
        {
            foreach (Point surroundingPoint in GetSurroundingPoints(x, y))
            {
                Tiles[surroundingPoint.X, surroundingPoint.Y].SurroundingMines++;
            }
        }

        private int CountSurroundingFlags(int x, int y)
        {
            int count = 0;

            foreach (Point surroundingPoint in GetSurroundingPoints(x, y))
            {
                if (Tiles[surroundingPoint.X, surroundingPoint.Y].HasFlag && Tiles[surroundingPoint.X, surroundingPoint.Y].HasMine)
                {
                    count++;
                }
            }

            return count;
        }

        private List<Point> GetSurroundingPoints(int x, int y)
        {
            List<Point> returnPoints = new List<Point>();

            int lowerX = Tiles.GetLowerBound(0);
            int upperX = Tiles.GetUpperBound(0);
            int lowerY = Tiles.GetLowerBound(1);
            int upperY = Tiles.GetUpperBound(1);

            foreach (Point surroundingPointModifier in surroundingPointModifiers)
            {
                if (x + surroundingPointModifier.X >= lowerX && x + surroundingPointModifier.X <= upperX
                    && y + surroundingPointModifier.Y >= lowerY && y + surroundingPointModifier.Y <= upperY)
                {
                    returnPoints.Add(new Point(x + surroundingPointModifier.X, y + surroundingPointModifier.Y));
                }
            }

            return returnPoints;
        }
    }
}
