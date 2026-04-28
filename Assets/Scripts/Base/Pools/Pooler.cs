using System.Collections.Generic;
using IdxZero.Base.MVP;
using UnityEngine;

namespace IdxZero.Base.Pools
{
    public class Pooler<T> where T : MonoBehaviour, IPoolable
    {
        private Transform _handler;
        private Queue<T> _pool;
        private int _startCount;
        private readonly IViewFactory<T> _viewFactory;

        public Pooler(IViewFactory<T> viewFactory)
        {
            _viewFactory = viewFactory;
        }

        protected void CreatePool(int count, string handlerName)
        {
            _handler = new GameObject(handlerName).transform;
            _pool = new Queue<T>(count);
            _startCount = count;
            GrowPool(count);
        }

        protected T GetFromPool()
        {
            if (_pool.Count == 0)
                GrowPool((int)(0.1f * _startCount)); // Grow pool on 10 percent from start count
            T poolableItem = _pool.Dequeue();

            poolableItem.gameObject.SetActive(true);
            poolableItem.GetFromPool();

            return poolableItem;
        }

        protected void SetToPool(T poolableItem)
        {
            poolableItem.SetToPool();
            DeactiveItem(poolableItem);
            _pool.Enqueue(poolableItem);
        }

        private void DeactiveItem(T poolableItem)
        {
            poolableItem.gameObject.SetActive(false);
            poolableItem.transform.SetParent(_handler);
        }

        private void GrowPool(int count)
        {
            for (int i = 0; i < count; i++)
            {
                T poolableItem = _viewFactory.Create();
                DeactiveItem(poolableItem);
                _pool.Enqueue(poolableItem);
            }
        }
    }
}