using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateOverTime : MonoBehaviour
{
    [SerializeField]
    private Vector3 rotationDirection;
    [SerializeField]
    [Tooltip("Speed per axis")]
    private Vector3 rotationSpeed;

    private Transform thisTransform;

    private void Start()
    {
        thisTransform = this.transform;
    }

    private void Update()
    {
        Vector3 rot = new Vector3(rotationDirection.x, rotationDirection.y, rotationDirection.z);
        rot.Scale(rotationSpeed);
        thisTransform.Rotate(rot);
    }
}
