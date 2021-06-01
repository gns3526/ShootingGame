using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationStock : MonoBehaviour
{
    [SerializeField] BulletScript bs;

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {

        gameObject.transform.rotation = Quaternion.identity;
    }


}
