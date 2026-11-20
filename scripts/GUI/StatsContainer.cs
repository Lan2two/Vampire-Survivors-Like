

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
            string[] stats = new[]
            {
                $"Time Played: {time:hh\\:mm\\:ss}",
                $"Total Kills: {data.TotalKills}",
                $"Total Damage Dealt: {data.TotalDamageDealt}",
                $"Total Damage Taken: {data.TotalDamageTaken}",
                $"Total Health Healed: {data.TotalHealthHealed}"
            };
            _totalStatsLabel.Text = string.Join(System.Environment.NewLine, stats);
        }
    }
}