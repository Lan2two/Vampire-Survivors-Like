namespace SurvivorsGame
{
    using Godot;

    public partial class LootComponent : Node
    {
        [Export] Enemy enemy;
        [Export] EnemyDeath deathState;
        [Export] PackedScene XpOrbScene;
        [Export] PackedScene HpOrbScene;
        private Node currentScene;

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
            currentScene = GetTree().CurrentScene;
            deathState.OnDeath += _OnDeath;
        }

        public override void _ExitTree()
        {
            if (deathState != null)
            {
                deathState.OnDeath -= _OnDeath;
            }
        }

        private void _OnDeath()
        {
            DropXP();
            DropLoot();
        }

        private void DropLoot()
        {
            int randInt = GD.RandRange(0, 10);
            if (randInt == 10)
            {
                HpOrb HpInstance = HpOrbScene.Instantiate<HpOrb>();
                if (HpInstance == null)
                {
                    GD.PrintErr("LootComponent failed to instantiate HpOrbScene.");
                    return;
                }
                if (currentScene == null)
                {
                    HpInstance.Free();
                    GD.PrintErr("LootComponent cannot drop loot because there is no current scene.");
                    return;
                }
                currentScene.AddChild(HpInstance);
                HpInstance.GlobalPosition = enemy.GlobalPosition;
            }
        }

        private void DropXP()
        {
            XpOrb XpInstance = XpOrbScene.Instantiate<XpOrb>();
            if (XpInstance == null)
            {
                GD.PrintErr("LootComponent failed to instantiate XpOrbScene.");
                return;
            }
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