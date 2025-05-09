using System;
using System.Collections.Generic;

namespace EorzeaHeart.Models
{
    public class PlayerProfile
    {
        public string Name { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public List<string> Interests { get; set; } = new();
        public List<string> LookingFor { get; set; } = new();
        public DateTime LastActive { get; set; }
        public bool IsOnline { get; set; }
        public string ProfileImage { get; set; } = string.Empty;
    }
} 