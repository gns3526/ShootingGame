using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineScript : MonoBehaviour
{
    [SerializeField] LineRenderer lr;

    [SerializeField] Transform point1;
    [SerializeField] Transform point2;

    [SerializeField] float lineWidth;

    private void Start()
    {
        lr.startWidth = lineWidth;
        lr.endWidth = lineWidth;
    }

    void Update()
    {
        lr.SetPosition(0, point1.position);
        lr.SetPosition(1, point2.position);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Debug.Log("닿음");
        }
    }
}
