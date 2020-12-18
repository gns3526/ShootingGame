using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public int dmg;
    [SerializeField] bool isRotate;

    private void Update()
    {
        if (isRotate)
        {
            transform.Rotate(Vector3.forward * 10);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "BulletBorder")
        {
            gameObject.SetActive(false);
        }
    }
}
