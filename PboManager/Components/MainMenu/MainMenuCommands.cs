using System.Windows.Input;

namespace PboManager.Components.MainMenu
{
    internal static class MainMenuCommands
    {
        public static ICommand PackFolderCommand { get; }

        public static ICommand UnpackFolderCommand { get; }

        public static ICommand ShowHelpCommand { get; }

        static MainMenuCommands()
        {
            PackFolderCommand = new RoutedCommand("PackFolder", typeof(MainMenuCommands));
            UnpackFolderCommand = new RoutedCommand("UnpackFolder", typeof(MainMenuCommands));
            ShowHelpCommand = new RoutedCommand("ShowHelp", typeof(MainMenuCommands));
        }
    }
}
