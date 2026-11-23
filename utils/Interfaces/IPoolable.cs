namespace SurvivorsGame
{
    using System.Collections.Generic;
    using Godot;
    public interface IPoolable
    {
        void OnSpawn();
        void OnDespawn();
    }

    public interface IPoolPrewarmer
    {
        IEnumerable<(PackedScene scene, int count)> GetPoolRequirements();
    }
}