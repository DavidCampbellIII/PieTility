using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PieTility
{
    public abstract class Singleton<T> : MonoBehaviour
    {
        public static T instance;

        //forces child to override, thus reminding them that InitializeSingleton must be called
        protected abstract void Awake();

        /// <summary>
        /// Initializes the singleton
        /// </summary>
        /// <param name="instance">Singleton instance (usually "this" if being called from child of Singleton<T>)</param>
        /// <returns>Success in initializing Singleton or not.  If false, calling method should end immediately</returns>
        protected bool InitializeSingleton(T singletonInstance)
        {
            if (instance == null)
            {
                instance = singletonInstance;
            }
            else
            {
                Destroy(this);
                return false;
            }
            return true;
        }
    }
}
