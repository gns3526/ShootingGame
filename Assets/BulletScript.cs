using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public int dmg;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "BulletBorder")
        {
            Destroy(gameObject);
        }
    }
}
