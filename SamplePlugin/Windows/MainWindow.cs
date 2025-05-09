using System;
using System.Numerics;
using Dalamud.Interface;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using EorzeaHeart.Models;
using System.Collections.Generic;

namespace EorzeaHeart.Windows
{
    public class MainWindow : Window, IDisposable
    {
        private Plugin Plugin;
        private PlayerProfile currentProfile;
        private bool isProfileCreated = false;
        private string name = string.Empty;
        private string bio = string.Empty;
        private string gender = string.Empty;
        private string interests = string.Empty;
        private string lookingFor = string.Empty;
        private int matchIndex = 0;
        private string profileImagePath = string.Empty;

        public MainWindow(Plugin plugin) : base("EorzeaHeart - Find Your Match!")
        {
            this.Plugin = plugin;
            this.currentProfile = new PlayerProfile();
            this.SizeConstraints = new WindowSizeConstraints
            {
                MinimumSize = new Vector2(400, 400),
                MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
            };
            TryLoadProfile();
        }

        public void Dispose()
        {
        }

        private void TryLoadProfile()
        {
            // Tenta carregar o perfil do usuário se já existir
            if (!string.IsNullOrWhiteSpace(currentProfile.Name)) return;
            if (ProfileManager.Profiles.Count > 0)
            {
                // Carrega o primeiro perfil como "meu" (simulação local)
                currentProfile = ProfileManager.Profiles[0];
                isProfileCreated = true;
            }
        }

        public override void Draw()
        {
            ImGui.SetNextWindowSize(new Vector2(500, 600), ImGuiCond.FirstUseEver);
            if (!isProfileCreated)
            {
                DrawProfileCreation();
            }
            else
            {
                DrawMainInterface();
            }
        }

        private void DrawProfileCreation()
        {
            ImGui.Spacing();
            ImGui.SetCursorPosX((ImGui.GetWindowWidth() - ImGui.CalcTextSize("EorzeaHeart").X) * 0.5f);
            ImGui.TextColored(new Vector4(1f, 0.2f, 0.5f, 1f), "EorzeaHeart");
            ImGui.Spacing();
            ImGui.TextWrapped("Let's create your profile to start matching with other players.");
            ImGui.Separator();
            ImGui.Spacing();
            ImGui.InputTextWithHint("##Name", "Name", ref name, 50);
            ImGui.InputTextWithHint("##Gender", "Gender", ref gender, 20);
            ImGui.InputTextMultiline("Bio", ref bio, 200, new Vector2(400, 60));
            ImGui.InputTextWithHint("##Interests", "Interests (comma separated)", ref interests, 100);
            ImGui.InputTextWithHint("##LookingFor", "Looking For (comma separated)", ref lookingFor, 100);
            ImGui.InputTextWithHint("##ProfileImagePath", "Profile Image Path", ref profileImagePath, 200);
            ImGui.SameLine();
            if (ImGui.Button("\uD83D\uDCC1"))
            {
                ImGui.OpenPopup("SelectImagePath");
            }
            if (ImGui.BeginPopup("SelectImagePath"))
            {
                ImGui.Text("Paste the image path below:");
                ImGui.InputText("##ImagePathPopup", ref profileImagePath, 200);
                if (ImGui.Button("OK"))
                    ImGui.CloseCurrentPopup();
                ImGui.EndPopup();
            }
            ImGui.Spacing();
            if (ImGui.Button("Create Profile", new Vector2(200, 0)) && !string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(gender))
            {
                currentProfile = new PlayerProfile
                {
                    Name = name,
                    Gender = gender,
                    Bio = bio,
                    Interests = new List<string>(interests.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)),
                    LookingFor = new List<string>(lookingFor.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)),
                    LastActive = DateTime.Now,
                    IsOnline = true,
                    ProfileImage = profileImagePath
                };
                ProfileManager.SaveProfile(currentProfile);
                isProfileCreated = true;
                matchIndex = 0;
            }
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("Create your profile and start matching!");
        }

        private void DrawMainInterface()
        {
            if (ImGui.BeginTabBar("EorzeaHeartTabs"))
            {
                if (ImGui.BeginTabItem("Find Matches"))
                {
                    DrawFindMatchesTab();
                    ImGui.EndTabItem();
                }

                if (ImGui.BeginTabItem("My Profile"))
                {
                    DrawProfileTab();
                    ImGui.EndTabItem();
                }

                if (ImGui.BeginTabItem("Matches"))
                {
                    DrawMatchesTab();
                    ImGui.EndTabItem();
                }

                ImGui.EndTabBar();
            }
        }

        private void DrawFindMatchesTab()
        {
            ImGui.Spacing();
            ImGui.SetCursorPosX((ImGui.GetWindowWidth() - ImGui.CalcTextSize("Find your perfect match in Eorzea!").X) * 0.5f);
            ImGui.TextColored(new Vector4(1f, 0.2f, 0.5f, 1f), "Find your perfect match in Eorzea!");
            ImGui.Spacing();
            var others = ProfileManager.GetOtherProfiles(currentProfile.Name);
            if (others.Count == 0)
            {
                ImGui.Text("No other profiles found.");
                return;
            }
            if (matchIndex >= others.Count) matchIndex = 0;
            var profile = others.Count > 0 ? others[matchIndex] : null;
            if (profile != null)
            {
                ImGui.BeginChild("##ProfileCard", new Vector2(0, 320), true, ImGuiWindowFlags.NoScrollbar);
                DrawProfileImage(profile.ProfileImage, 96);
                ImGui.SetCursorPosX((ImGui.GetWindowWidth() - ImGui.CalcTextSize(profile.Name).X) * 0.5f);
                ImGui.TextColored(new Vector4(1f, 0.7f, 0.7f, 1f), profile.Name);
                ImGui.TextWrapped($"Bio: {profile.Bio}");
                ImGui.Text($"Interests: {string.Join(", ", profile.Interests)}");
                ImGui.Text($"Looking For: {string.Join(", ", profile.LookingFor)}");
                ImGui.EndChild();
                ImGui.Spacing();
                ImGui.SetCursorPosX((ImGui.GetWindowWidth() - 250) * 0.5f);
                if (!ProfileManager.IsMatch(currentProfile.Name, profile.Name))
                {
                    ImGui.PushStyleColor(ImGuiCol.Button, new Vector4(0.2f, 0.8f, 0.2f, 1f));
                    if (ImGui.Button($"Like ❤️##{profile.Name}", new Vector2(120, 0)))
                    {
                        ProfileManager.Like(currentProfile.Name, profile.Name);
                        matchIndex = (matchIndex + 1) % others.Count;
                    }
                    ImGui.PopStyleColor();
                    ImGui.SameLine();
                    ImGui.PushStyleColor(ImGuiCol.Button, new Vector4(0.8f, 0.2f, 0.2f, 1f));
                    if (ImGui.Button($"Skip ❌##{profile.Name}", new Vector2(120, 0)))
                    {
                        matchIndex = (matchIndex + 1) % others.Count;
                    }
                    ImGui.PopStyleColor();
                }
                else
                {
                    ImGui.TextColored(new Vector4(0, 1, 0, 1), "Matched!");
                    if (ImGui.Button($"Next Match", new Vector2(250, 0)))
                    {
                        matchIndex = (matchIndex + 1) % others.Count;
                    }
                }
            }
        }

        private void DrawProfileTab()
        {
            ImGui.Spacing();
            ImGui.SetCursorPosX((ImGui.GetWindowWidth() - ImGui.CalcTextSize("Your Profile").X) * 0.5f);
            ImGui.TextColored(new Vector4(1f, 0.2f, 0.5f, 1f), "Your Profile");
            ImGui.Spacing();
            ImGui.BeginChild("##MyProfileCard", new Vector2(0, 320), true, ImGuiWindowFlags.NoScrollbar);
            DrawProfileImage(currentProfile.ProfileImage, 96);
            ImGui.SetCursorPosX((ImGui.GetWindowWidth() - ImGui.CalcTextSize(currentProfile.Name).X) * 0.5f);
            ImGui.TextColored(new Vector4(1f, 0.7f, 0.7f, 1f), currentProfile.Name);
            ImGui.TextWrapped($"Bio: {currentProfile.Bio}");
            ImGui.Text($"Interests: {string.Join(", ", currentProfile.Interests)}");
            ImGui.Text($"Looking For: {string.Join(", ", currentProfile.LookingFor)}");
            ImGui.EndChild();
            ImGui.Spacing();
            ImGui.SetCursorPosX((ImGui.GetWindowWidth() - 120) * 0.5f);
            if (ImGui.Button("Edit Profile", new Vector2(120, 0)))
            {
                isProfileCreated = false;
                name = currentProfile.Name;
                gender = currentProfile.Gender;
                bio = currentProfile.Bio;
                interests = string.Join(", ", currentProfile.Interests);
                lookingFor = string.Join(", ", currentProfile.LookingFor);
                profileImagePath = currentProfile.ProfileImage;
            }
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("Edit your profile");
        }

        private void DrawMatchesTab()
        {
            ImGui.Spacing();
            ImGui.SetCursorPosX((ImGui.GetWindowWidth() - ImGui.CalcTextSize("Your Matches").X) * 0.5f);
            ImGui.TextColored(new Vector4(1f, 0.2f, 0.5f, 1f), "Your Matches");
            ImGui.Spacing();
            var matches = ProfileManager.GetMatches(currentProfile.Name);
            if (matches.Count == 0)
            {
                ImGui.Text("No matches yet. Keep swiping!");
                return;
            }
            foreach (var match in matches)
            {
                ImGui.BeginChild($"##MatchCard_{match.Name}", new Vector2(0, 180), true, ImGuiWindowFlags.NoScrollbar);
                DrawProfileImage(match.ProfileImage, 64);
                ImGui.SetCursorPosX((ImGui.GetWindowWidth() - ImGui.CalcTextSize(match.Name).X) * 0.5f);
                ImGui.TextColored(new Vector4(0.7f, 1f, 0.7f, 1f), match.Name);
                ImGui.TextWrapped($"Bio: {match.Bio}");
                ImGui.Text($"Interests: {string.Join(", ", match.Interests)}");
                ImGui.Text($"Looking For: {string.Join(", ", match.LookingFor)}");
                ImGui.EndChild();
                ImGui.Spacing();
            }
        }

        private void DrawProfileImage(string imagePath, float maxSize)
        {
            if (!string.IsNullOrWhiteSpace(imagePath))
            {
                var tex = Plugin.TextureProvider.GetFromFile(imagePath).GetWrapOrDefault();
                if (tex != null)
                {
                    float w = tex.Width;
                    float h = tex.Height;
                    float scale = 1f;
                    if (w > h && w > maxSize) scale = maxSize / w;
                    else if (h > w && h > maxSize) scale = maxSize / h;
                    else if (w > maxSize) scale = maxSize / w;
                    else if (h > maxSize) scale = maxSize / h;
                    var size = new Vector2(w * scale, h * scale);
                    ImGui.SetCursorPosX((ImGui.GetWindowWidth() - size.X) * 0.5f);
                    ImGui.Image(tex.ImGuiHandle, size);
                }
            }
        }
    }
}
