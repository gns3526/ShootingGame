using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string type;
    [SerializeField] Rigidbody2D rigid;
    void Start()
    {
        rigid.velocity = Vector2.down * 3;
    }

}
