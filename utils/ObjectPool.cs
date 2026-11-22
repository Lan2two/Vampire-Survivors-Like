namespace SurvivorsGame
{
    using Godot;
    using System.Collections.Generic;

    public class ObjectPool<T> where T : Node, IPoolable
    {
        private readonly Queue<T> _pool = new();
        private readonly PackedScene _scene;
        private readonly Node _parent;
        private readonly int _expandSize;

        public ObjectPool(PackedScene scene, Node parent, int initialSize, int expandSize = 16)
        {
            _scene = scene;
            _parent = parent;
            _expandSize = Mathf.Max(1, expandSize);

            for (int i = 0; i < initialSize; i++)
            {
                Return(CreateNewInstance());
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
            if (_pool.Count > 0)
            {
                return _pool.Dequeue();
            }

            for (int i = 1; i < _expandSize; i++)
            {
                Return(CreateNewInstance());
            }
            return CreateNewInstance();
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
