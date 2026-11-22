namespace SurvivorsGame
{
    using Godot;
    using System.Collections.Generic;

    public class ObjectPool<T> where T : Node, IPoolable
    {
        private readonly Queue<T> _pool = new();
        private readonly PackedScene _scene;
        private readonly Node _parent;

        public ObjectPool(PackedScene scene, Node parent, int initialSize)
        {
            _scene = scene;
            _parent = parent;

            for (int i = 0; i < initialSize; i++)
            {
                T instance = CreateNewInstance();
                Return(instance);
            }
        }

        private T CreateNewInstance()
        {
            T instance = _scene.Instantiate<T>();
            _parent.AddChild(instance);
            return instance;
        }

        public T Get()
        {
            return _pool.Count > 0 ? _pool.Dequeue() : CreateNewInstance();
        }

        public void Return(T instance)
        {
            if (!GodotObject.IsInstanceValid(instance)) return;
            instance.OnDespawn();
            _pool.Enqueue(instance);
        }

        public void Clear()
        {
            while (_pool.Count > 0)
            {
                T instance = _pool.Dequeue();
                if (GodotObject.IsInstanceValid(instance))
                {
                    instance.QueueFree();
                }
            }
        }
    }
}
