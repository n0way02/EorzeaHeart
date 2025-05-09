using System;
using System.Collections.Generic;
using System.Numerics;
using Dalamud.Interface;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using EorzeaHeart.Models;

namespace EorzeaHeart.Windows
{
    public class MatchesWindow : Window, IDisposable
    {
        private Plugin Plugin;
        private List<PlayerProfile> matches;

        public MatchesWindow(Plugin plugin) : base("EorzeaHeart - Matches")
        {
            this.Plugin = plugin;
            this.SizeConstraints = new WindowSizeConstraints
            {
                MinimumSize = new Vector2(375, 330),
                MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
            };
            this.matches = new List<PlayerProfile>();
        }

        public void Dispose()
        {
        }

        public override void Draw()
        {
            if (ImGui.BeginTabBar("MatchesTabs"))
            {
                if (ImGui.BeginTabItem("New Matches"))
                {
                    DrawNewMatchesTab();
                    ImGui.EndTabItem();
                }

                if (ImGui.BeginTabItem("Messages"))
                {
                    DrawMessagesTab();
                    ImGui.EndTabItem();
                }

                ImGui.EndTabBar();
            }
        }

        private void DrawNewMatchesTab()
        {
            ImGui.Text("New Matches");
            ImGui.Separator();

            if (matches.Count == 0)
            {
                ImGui.Text("No new matches yet. Keep swiping!");
                return;
            }

            foreach (var match in matches)
            {
                if (ImGui.CollapsingHeader($"{match.Name}"))
                {
                    ImGui.Text(match.Bio);
                    
                    if (ImGui.Button($"Message {match.Name}"))
                    {
                        // TODO: Implement messaging
                    }
                }
            }
        }

        private void DrawMessagesTab()
        {
            ImGui.Text("Messages");
            ImGui.Separator();

            // TODO: Implement message list and chat interface
            ImGui.Text("Coming soon: Chat with your matches!");
        }
    }
} 