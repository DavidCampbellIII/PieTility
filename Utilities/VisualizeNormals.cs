using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualizeNormals : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Should normals draw?")]
    private bool draw = true;

    [Header("==SETTINGS==")]
    [SerializeField]
    [Tooltip("Color of normal ray")]
    [ConditionalField("draw")]
    private Color rayColor = Color.red;
    [SerializeField]
    [Tooltip("Length of normal rays")]
    [ConditionalField("draw")]
    private float rayLength = 5f;

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if(!draw)
        {
            return;
        }

        Gizmos.color = rayColor;
        Transform thisTransform = this.transform;
        Vector3[] normals = GetComponent<MeshFilter>().sharedMesh.normals;
        foreach(Vector3 normal in normals)
        {
            Vector3 rotatedNormal = Quaternion.Euler(thisTransform.eulerAngles) * normal * rayLength;
            Vector3 withOffset = thisTransform.position + rotatedNormal;
            Gizmos.DrawLine(thisTransform.position, withOffset);
        }
    }
#endif
}
