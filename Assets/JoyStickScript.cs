using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class JoyStickScript : MonoBehaviour, IDragHandler
{
    public Player myPlayerScript;

    private bool touchStart = false;
    private Vector2 pointA;
    private Vector2 pointB;

    [SerializeField] Transform circle;
    [SerializeField] Transform outerCircle;

    private void Update()
    {
        
    }


    private void FixedUpdate()
    {
        if (touchStart)
        {
            Vector2 offset = pointB - pointA;
            Vector2 direction = Vector2.ClampMagnitude(offset, 1.0f);
            MoveCharacter(direction);

            circle.transform.position = new Vector2(pointA.x + direction.x, pointA.y + direction.y);
        }
    }

    public void TouchDown()
    {
        touchStart = true;

        pointA = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));

        pointA = outerCircle.transform.position;

        pointB = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
    }
    public void TouchUp()
    {
        touchStart = false;

        circle.transform.localPosition = new Vector3(0, 0, 0);
    }

    public void Drag()
    {
        if (myPlayerScript != null)
        {
            pointB = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
        }
    }

    void MoveCharacter(Vector2 direction)
    {
        if ((direction.x < 0 && myPlayerScript.isTouchLeft) || direction.x > 0 && myPlayerScript.isTouchRight) direction.x = 0;
        if ((direction.y < 0 && myPlayerScript.isTouchBottom) || direction.y > 0 && myPlayerScript.isTouchTop) direction.y = 0;
        myPlayerScript.transform.Translate(direction * (myPlayerScript.moveSpeed / 100) * 4 * Time.deltaTime);
    }
}
