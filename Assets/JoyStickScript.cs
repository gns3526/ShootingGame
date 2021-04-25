﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class JoyStickScript : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Player myPlayerScript;

    private bool touchStart = false;
    private Vector2 pointA;
    private Vector2 pointB;

    [SerializeField] Transform circle;
    [SerializeField] Transform outerCircle;
    [SerializeField] RectTransform lever;
    [SerializeField] RectTransform rectTransform;

    [SerializeField] int right ,left;
    

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

 

    public void OnBeginDrag(PointerEventData eventData)
    {

        var inputpos = eventData.position - rectTransform.anchoredPosition;
        var inputVector = inputpos;
        lever.anchoredPosition = inputpos;

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

    public void OnDrag(PointerEventData eventData)
    {
        var inputpos = eventData.position - rectTransform.anchoredPosition;
        lever.anchoredPosition = inputpos;
       // pointB = eventData.pressPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {

        lever.anchoredPosition = Vector2.zero;
       // touchStart = false;

       // circle.transform.position = outerCircle.transform.position;
    }
}
