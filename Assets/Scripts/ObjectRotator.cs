using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    [SerializeField] private Vector3 _angleVelocity;
    void Update()
    {
        // Rotate the object
        transform.localEulerAngles += _angleVelocity * Time.deltaTime;
    }
}
