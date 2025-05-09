using System;
using System.Collections.Generic;
using System.Linq;
using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using System.IO;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using EorzeaHeart.Windows;
using EorzeaHeart.Models;

namespace EorzeaHeart
{
    public sealed class Plugin : IDalamudPlugin
    {
        [PluginService] internal static IDalamudPluginInterface PluginInterface { get; private set; } = null!;
        [PluginService] internal static ITextureProvider TextureProvider { get; private set; } = null!;
        [PluginService] internal static ICommandManager CommandManager { get; private set; } = null!;
        [PluginService] internal static IClientState ClientState { get; private set; } = null!;
        [PluginService] internal static IDataManager DataManager { get; private set; } = null!;
        [PluginService] internal static IPluginLog Log { get; private set; } = null!;

        private const string CommandName = "/eorzeaheart";

        public readonly WindowSystem WindowSystem = new("EorzeaHeart");
        private MainWindow MainWindow { get; init; }
        private ProfileWindow ProfileWindow { get; init; }
        private MatchesWindow MatchesWindow { get; init; }

        public Plugin()
        {
            ProfileManager.Load();
            MainWindow = new MainWindow(this);
            ProfileWindow = new ProfileWindow(this);
            MatchesWindow = new MatchesWindow(this);

            WindowSystem.AddWindow(MainWindow);
            WindowSystem.AddWindow(ProfileWindow);
            WindowSystem.AddWindow(MatchesWindow);

            CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
            {
                HelpMessage = "Open the EorzeaHeart dating plugin"
            });

            PluginInterface.UiBuilder.Draw += DrawUI;
            PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;

            // This adds a button to the plugin installer entry of this plugin which allows
            // to toggle the display status of the configuration ui
            PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;

            // Adds another button that is doing the same but for the main ui of the plugin
            PluginInterface.UiBuilder.OpenMainUi += ToggleMainUI;

            // Add a simple message to the log with level set to information
            // Use /xllog to open the log window in-game
            // Example Output: 00:57:54.959 | INF | [SamplePlugin] ===A cool log message from Sample Plugin===
            Log.Information($"===A cool log message from {PluginInterface.Manifest.Name}===");
        }

        public void Dispose()
        {
            WindowSystem.RemoveAllWindows();

            MainWindow.Dispose();
            ProfileWindow.Dispose();
            MatchesWindow.Dispose();

            CommandManager.RemoveHandler(CommandName);
        }

        private void OnCommand(string command, string args)
        {
            this.MainWindow.IsOpen = true;
        }

        private void DrawUI() => WindowSystem.Draw();

        public void DrawConfigUI()
        {
            this.MainWindow.IsOpen = true;
        }

        public void ToggleMainUI() => MainWindow.Toggle();
    }
}
