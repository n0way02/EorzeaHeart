using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.Json;

namespace EorzeaHeart.Models
{
    public static class ProfileManager
    {
        // Simula um banco de dados de perfis
        public static List<PlayerProfile> Profiles { get; } = new();
        // Simula matches: chave = nome do jogador, valor = lista de nomes que deram match
        public static Dictionary<string, List<string>> Matches { get; } = new();
        private static readonly string ProfilesFile = "profiles.json";
        private static readonly string MatchesFile = "matches.json";

        public static void Load()
        {
            if (File.Exists(ProfilesFile))
            {
                var json = File.ReadAllText(ProfilesFile);
                var loaded = JsonSerializer.Deserialize<List<PlayerProfile>>(json);
                if (loaded != null)
                {
                    Profiles.Clear();
                    Profiles.AddRange(loaded);
                }
            }
            if (File.Exists(MatchesFile))
            {
                var json = File.ReadAllText(MatchesFile);
                var loaded = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(json);
                if (loaded != null)
                {
                    Matches.Clear();
                    foreach (var kv in loaded)
                        Matches[kv.Key] = kv.Value;
                }
            }
        }

        public static void Save()
        {
            File.WriteAllText(ProfilesFile, JsonSerializer.Serialize(Profiles));
            File.WriteAllText(MatchesFile, JsonSerializer.Serialize(Matches));
        }

        public static PlayerProfile? GetProfile(string name)
        {
            return Profiles.FirstOrDefault(p => p.Name == name);
        }

        public static void SaveProfile(PlayerProfile profile)
        {
            var existing = GetProfile(profile.Name);
            if (existing != null)
            {
                Profiles.Remove(existing);
            }
            Profiles.Add(profile);
            Save();
        }

        public static List<PlayerProfile> GetOtherProfiles(string myName)
        {
            return Profiles.Where(p => p.Name != myName).ToList();
        }

        public static void Like(string from, string to)
        {
            if (!Matches.ContainsKey(from))
                Matches[from] = new List<string>();
            if (!Matches[from].Contains(to))
                Matches[from].Add(to);
            Save();
        }

        public static bool IsMatch(string user1, string user2)
        {
            return Matches.ContainsKey(user1) && Matches[user1].Contains(user2)
                && Matches.ContainsKey(user2) && Matches[user2].Contains(user1);
        }

        public static List<PlayerProfile> GetMatches(string myName)
        {
            return Profiles.Where(p => p.Name != myName && IsMatch(myName, p.Name)).ToList();
        }
    }
} 