using System;
using System.Numerics;
using Dalamud.Interface;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using EorzeaHeart.Models;

namespace EorzeaHeart.Windows
{
    public class ProfileWindow : Window, IDisposable
    {
        private Plugin Plugin;
        private PlayerProfile profile;
        private string bioText = string.Empty;

        public ProfileWindow(Plugin plugin) : base("EorzeaHeart - Profile")
        {
            this.Plugin = plugin;
            this.profile = new PlayerProfile();
            this.bioText = profile.Bio;
            this.SizeConstraints = new WindowSizeConstraints
            {
                MinimumSize = new Vector2(375, 330),
                MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
            };
        }

        public void Dispose()
        {
        }

        public override void Draw()
        {
            if (ImGui.BeginTabBar("ProfileTabs"))
            {
                if (ImGui.BeginTabItem("Edit Profile"))
                {
                    DrawEditProfileTab();
                    ImGui.EndTabItem();
                }

                if (ImGui.BeginTabItem("Preferences"))
                {
                    DrawPreferencesTab();
                    ImGui.EndTabItem();
                }

                ImGui.EndTabBar();
            }
        }

        private void DrawEditProfileTab()
        {
            ImGui.Text("Edit Your Profile");
            ImGui.Separator();

            // Basic Information
            ImGui.Text("Basic Information");
            if (ImGui.InputText("Bio", ref bioText, 500))
            {
                profile.Bio = bioText;
            }
            
            // Interests
            ImGui.Text("Interests");
            // TODO: Implement interests selection

            // Looking For
            ImGui.Text("Looking For");
            // TODO: Implement looking for preferences

            if (ImGui.Button("Save Profile"))
            {
                // TODO: Implement profile saving
            }
        }

        private void DrawPreferencesTab()
        {
            ImGui.Text("Matching Preferences");
            ImGui.Separator();

            // Distance
            ImGui.Text("Maximum Distance");
            // TODO: Implement distance slider

            // Age Range
            ImGui.Text("Age Range");
            // TODO: Implement age range selection

            // Job Preferences
            ImGui.Text("Job Preferences");
            // TODO: Implement job preferences

            if (ImGui.Button("Save Preferences"))
            {
                // TODO: Implement preferences saving
            }
        }
    }
} 