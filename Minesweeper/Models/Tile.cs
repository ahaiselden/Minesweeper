namespace Minesweeper.Models
{
    public class Tile
    {
        public bool HasMine { get; set; }
        
        public bool HasFlag { get; set; }
        
        public bool IsInspected { get; set; }
        
        public int SurroundingMines { get; set; }
    }
}
