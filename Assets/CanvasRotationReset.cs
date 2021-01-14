using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasRotationReset : MonoBehaviour
{
    RectTransform target;

    private void Start()
    {
        target = GetComponent<RectTransform>();
    }

    private void Update()
    {
        target.rotation = Quaternion.identity;
    }
}
