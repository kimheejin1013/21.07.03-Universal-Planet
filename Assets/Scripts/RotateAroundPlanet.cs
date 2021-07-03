using System.Collections;
using UnityEngine;

public class RotateAroundPlanet : MonoBehaviour
{
    public Transform ParentTransform;
    public Vector3 RotateAxis = Vector3.up;
    public float RotateSpeed = 10.0f;

    private void Update()
    {
        transform.RotateAround(
            ParentTransform.position,
            RotateAxis,
            RotateSpeed * Time.deltaTime);
    }

}