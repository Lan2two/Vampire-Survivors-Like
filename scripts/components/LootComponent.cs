namespace SurvivorsGame
{
    using Godot;
    using Godot.Collections;

    public partial class LootComponent : Node
    {
        [Export] Array<PackedScene> LootTable = new();
        [Export] float HealthDropRate = 5;
        [Export] float LootDropRate = .1f;
        [Export] Enemy enemy;
        [Export] EnemyDeath deathState;
        [Export] PackedScene XpOrbScene;
        [Export] PackedScene HpOrbScene;
        private Node currentScene;
        private PackedScene worldItem;

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
            DropHP(HealthDropRate);
            DropItem(LootDropRate);
        }
        public void DropXP()
        {
            XpOrb XpInstance = DropScene(XpOrbScene) as XpOrb;
            XpInstance.SetXPAmount(enemy.XpAmount);
        }

        public void DropHP(float chance)
        {
            float HpChance = (float)GD.RandRange(0f, 100f);
            if (HpChance > chance)
            {
                return;
            }
            DropScene(HpOrbScene);
        }

        public void DropItem(float chance)
        {
            if (LootTable.Count == 0)
            {
                return;
            }
            float LootChance = (float)GD.RandRange(0f, 100f);
            if (LootChance > chance)
            {
                return;
            }
            int randItemIndex = GD.RandRange(0, LootTable.Count - 1);
            DropScene(LootTable[randItemIndex]);
        }

        private Node2D DropScene(PackedScene scene)
        {
            Node2D sceneInstance = scene.Instantiate<Node2D>();
            if (sceneInstance == null)
            {
                GD.PrintErr("LootComponent failed to instantiate Scene.");
                return null;
            }
            if (currentScene == null)
            {
                sceneInstance.Free();
                GD.PrintErr("LootComponent cannot drop loot because there is no current scene.");
                return null;
            }
            currentScene.AddChild(sceneInstance);
            sceneInstance.GlobalPosition = enemy.GlobalPosition + new Vector2(GD.RandRange(-5, 5), GD.RandRange(-5, 5));
            return sceneInstance;
        }
    }
}