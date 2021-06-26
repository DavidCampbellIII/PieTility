using UnityEngine;

namespace PieTility
{
    public class BezierCurve : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Visual handles for controlling the bezier curve (Must have EXACTLY 4 handles)")]
        private Transform[] _handles;
        public Transform[] handles { get => _handles; }

        [SerializeField]
        [Range(2, 1000)]
        [Tooltip("Number of vertices on the curve.  Higher values mean smoother curves")]
        private int resolution = 50;

        public float step { get => 1f / resolution; }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (resolution <= 1)
            {
                Debug.LogError("Bezier curves must have at least 2 vertices");
                return;
            }

            if(handles.Length != 4)
            {
                Debug.LogError("Beizer curve requires EXACTLY 4 handles to work correctly!");
                return;
            }

            for (float time = 0; time <= 1; time += step)
            {
                Vector2 gizmosPosition = Mathf.Pow(1 - time, 3) * handles[0].position + 3 * Mathf.Pow(1 - time, 2) * time * handles[1].position + 3 * (1 - time) * Mathf.Pow(time, 2) * handles[2].position + Mathf.Pow(time, 3) * handles[3].position;
                Gizmos.DrawSphere(gizmosPosition, 0.15f);
            }

            Gizmos.DrawLine(handles[0].position, handles[1].position);
            Gizmos.DrawLine(handles[2].position, handles[3].position);
        }
    }
#endif
}
