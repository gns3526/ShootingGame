using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuckRotation : MonoBehaviour
{
    float x;
    float y;
    float z;
    private void OnEnable()
    {
        x = transform.rotation.x;
        y = transform.rotation.y;
        z = transform.rotation.z;
    }
    void Update()
    {
        transform.rotation = Quaternion.Euler(x, y, z);
    }
}
