using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PieTility
{
    public class DestroyTimer : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Should the destroy timer start on Start()?")]
        private bool startDestroyTimerOnStart = false;
        [SerializeField]
        [Tooltip("Duration waited before destroying this gameobject")]
        private float timeToDestroy = 3f;

        private void Start()
        {
            if (startDestroyTimerOnStart)
            {
                StartDestroyTimer();
            }
        }

        /// <summary>
        /// Start the destroy timer
        /// </summary>
        /// <param name="time">How long until the object should destroys itself.  Defaults to -1, which will use the provided instance field above</param>
        public void StartDestroyTimer(float time = -1f)
        {
            if (time == -1f)
            {
                time = timeToDestroy;
            }
            Destroy(this.gameObject, time);
        }
    }
}
