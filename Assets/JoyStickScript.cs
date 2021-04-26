using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class JoyStickScript : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Player myPlayerScript;

    [SerializeField] RectTransform lever;
    [SerializeField] RectTransform rectTransform;

    [SerializeField, Range(10, 150)]
    private float leverRange;

    private Vector2 inputDirection;
    private bool isInput;

    [SerializeField] int right ,left;

    void MoveCharacter(Vector2 direction)
    {
        if ((direction.x < 0 && myPlayerScript.isTouchLeft) || direction.x > 0 && myPlayerScript.isTouchRight) direction.x = 0;
        if ((direction.y < 0 && myPlayerScript.isTouchBottom) || direction.y > 0 && myPlayerScript.isTouchTop) direction.y = 0;
        myPlayerScript.transform.Translate(direction * (myPlayerScript.moveSpeed / 100) * 4 * Time.deltaTime);

        if(direction.x > 0)
        myPlayerScript.ani.SetInteger("AxisX", 1);
        else if (direction.x < 0)
            myPlayerScript.ani.SetInteger("AxisX", -1);
        else
            myPlayerScript.ani.SetInteger("AxisX", 0);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        ControlJoySticklever(eventData);
        isInput = true;
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        ControlJoySticklever(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {

        lever.anchoredPosition = Vector2.zero;
        isInput = false;
    }

    private void ControlJoySticklever(PointerEventData eventData)
    {
        var inputpos = eventData.position - rectTransform.anchoredPosition;
        var inputVector = inputpos.magnitude < leverRange ? inputpos : inputpos.normalized * leverRange;
        lever.anchoredPosition = inputVector;
        inputDirection = inputVector / leverRange;
    }

    private void Update()
    {
        if (isInput)
            MoveCharacter(inputDirection);
    }
}
