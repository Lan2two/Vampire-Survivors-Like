

namespace SurvivorsGame
{
    using System;
    using Godot;
    public partial class StatsContainer : SubMenuContainer
    {
        private RichTextLabel _totalStatsLabel;

        public override void _Ready()
        {
            base._Ready();
            _totalStatsLabel = GetNodeOrNull<RichTextLabel>("Panel/TotalStatsLabel");
            LoadTotalStats();
            GD.Print(animName);
        }

        private void LoadTotalStats()
        {
            SaveSystem saveSystem = new SaveSystem();
            GameSaveData data = saveSystem.LoadData();
            TimeSpan time = TimeSpan.FromSeconds(data.TotalSeconds);
            string[] stats =
            [
                $"Time Played: {time.Hours:D2}:{time.Minutes:D2}:{time.Seconds:D2}",
                $"Total Coins Collected: {data.TotalCoins}",
                $"Total Kills: {data.TotalKills}",
                $"Total Damage Dealt: {data.TotalDamageDealt}",
                $"Total Damage Taken: {data.TotalDamageTaken}",
                $"Total Health Healed: {data.TotalHealthHealed}",
                $"Total Campfires Lit: {data.TotalCampfiresLit}",
                $"Total Xp Gained: {data.TotalTotalXp}"
            ];
            _totalStatsLabel.Text = string.Join(System.Environment.NewLine, stats);
        }
    }
}