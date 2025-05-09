# EorzeaHeart

EorzeaHeart is a dating plugin for Final Fantasy XIV, inspired by Tinder, allowing players to create a profile, browse other players, and match with those they like—all in-game!

## Features
- Create and edit your own profile (name, gender, bio, interests, what you're looking for, and profile image)
- Browse other player profiles one by one (swipe-like experience)
- Like or skip profiles
- If two players like each other, they become a match and appear in the Matches tab
- All data is stored locally (no server required)
- Modern, beautiful, and user-friendly ImGui interface

## How to Use
1. **Build the plugin** using `dotnet build` (requires .NET 9+ and Dalamud SDK 12.x)
2. Copy the generated `SamplePlugin.dll` and `SamplePlugin.json` to your Dalamud dev plugins folder
3. In-game, use `/eorzeaheart` to open the plugin
4. Create your profile and start matching!

## Screenshots
*Add your screenshots here!*

## Development
- Stack: C# (.NET), Dalamud.NET.Sdk, ImGui.NET
- All logic and UI are contained in the plugin, no external server required
- For development, just clone, build, and load in Dalamud

## License
AGPL-3.0-or-later

---

Made with ❤️ for the FFXIV community.
