namespace SurvivorsGame
{
    using Godot;
    using Godot.Collections;

    public partial class LootComponent : Node
    {
        [Export] Array<PackedScene> LootTable = new();
        [Export] float XpDropRate = 100;
        [Export] float HealthDropRate = .1f;
        [Export] float CoinDroprate = 1f;
        [Export] float LootDropRate = .1f;
        [Export] Enemy enemy;
        [Export] EnemyDeath deathState;
        [Export] PackedScene XpOrbScene;
        [Export] PackedScene HpOrbScene;
        [Export] PackedScene CoinScene;
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
            DropAbsorbable(XpOrbScene, XpDropRate, enemy.XpAmount);
            DropAbsorbable(HpOrbScene, HealthDropRate);
            DropAbsorbable(CoinScene, CoinDroprate, enemy.CoinAmount);
            DropItem(LootDropRate);
        }

        public void DropAbsorbable(PackedScene packedScene, float chance, float amount = 0)
        {
            if (packedScene == null)
            {
                GD.PrintErr("Dropped scene is null");
                return;
            }
            float Rand = (float)GD.RandRange(0f, 100f);
            if (Rand > chance)
            {
                return;
            }
            Absorbable Instance = DropScene(packedScene) as Absorbable;
            Instance.SetAmount(amount);
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