namespace SurvivorsGame
{
    using Godot;
    using System;

    public partial class LootComponent : Node
    {
        [Export] Enemy enemy;
        [Export] EnemyDeath deathState;
        [Export] PackedScene XpOrbScene;

        public override void _Ready()
        {
            if (deathState == null)
            {
                GD.PrintErr("EnemyDeath unassigned in LootComponent.");
                return;
            }
            if (enemy == null)
            {
                GD.PrintErr("Enemy unassigned in LootComponent.");
                return;
            }
            if (XpOrbScene == null)
            {
                GD.PrintErr("XpOrbScene unassigned in LootComponent.");
                return;
            }
            deathState.OnDeath += _OnDeath;
        }

        public override void _ExitTree()
        {
            deathState.OnDeath -= _OnDeath;
        }

        private void _OnDeath()
        {
            DropXP();
            DropLoot();
        }

        private void DropLoot()
        {

        }

        private void DropXP()
        {
            XpOrb XpInstance = XpOrbScene.Instantiate<XpOrb>();
            if (XpInstance == null)
            {
                GD.PrintErr("LootComponent failed to instantiate XpOrbScene.");
                return;
            }
            Node currentScene = GetTree().CurrentScene;
            if (currentScene == null)
            {
                XpInstance.Free();
                GD.PrintErr("LootComponent cannot drop loot because there is no current scene.");
                return;
            }
            currentScene.AddChild(XpInstance);
            XpInstance.SetXPAmount(enemy.XpAmount);
            XpInstance.GlobalPosition = enemy.GlobalPosition;
        }
    }
}