using System.Reflection;

namespace Minesweeper.Enumerations.Attributes
{
    public static class MinesweeperSettingsExtensions
    {
        public static MinesweeperSettings GetMinesweeperSettings(this Difficulty difficulty) 
        {
            string name = difficulty.ToString();
            FieldInfo fieldInfo = difficulty.GetType().GetField(name);
            return fieldInfo.GetCustomAttribute<MinesweeperSettings>();
        }
    }
}
