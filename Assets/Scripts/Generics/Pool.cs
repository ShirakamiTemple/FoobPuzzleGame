//***
// Author: Nate
// Description: Handler.cs is a generic class for pooling any kind of object
//***

using System.Collections.Generic;
using UnityEngine;

namespace FoxHerding.Generics
{
    /// <summary>
    /// A generic object pool for reusing objects to avoid costly instantiation and destruction.
    /// </summary>
    public class Pool<T> where T : Component
    {
        private readonly T _prefab;
        private readonly Stack<T> _inactiveInstances = new();

        /// <summary>
        /// Creates a new object pool that uses the specified prefab to instantiate new objects as needed.
        /// </summary>
        /// <param name="prefab">The prefab to use when instantiating new objects.</param>
        public Pool(T prefab)
        {
            _prefab = prefab;
        }

        /// <summary>
        /// Returns an instance of the specified type from the pool, or creates a new one if none are available.
        /// </summary>
        /// <returns>An instance of the specified type.</returns>
        public T Get()
        {
            if (_inactiveInstances.Count > 0)
            {
                T instance = _inactiveInstances.Pop();
                instance.gameObject.SetActive(true);
                return instance;
            }
            else
            {
                T instance = Object.Instantiate(_prefab);
                return instance;
            }
        }

        /// <summary>
        /// Returns an object to the pool for reuse, deactivating it so it can be reactivated later.
        /// </summary>
        /// <param name="instance">The instance to return to the pool.</param>
        public void ReturnToPool(T instance)
        {
            instance.gameObject.SetActive(false);
            _inactiveInstances.Push(instance);
        }
    }
}