using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundScript : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] int startIndex;
    [SerializeField] int endIndex;
    [SerializeField] Transform[] sprites;

    [SerializeField] float viewHeight;

    private void Awake()
    {
        viewHeight = Camera.main.orthographicSize * 2;
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 curPos = transform.position;
        Vector3 nextPos = Vector3.down * speed * Time.deltaTime;
        transform.position = curPos + nextPos;

        if(sprites[endIndex].position.y < viewHeight * (-1))
        {
            Vector3 backSpirtePos = sprites[startIndex].localPosition;
            Vector3 frontSpritePos = sprites[endIndex].localPosition;
            sprites[endIndex].transform.localPosition = backSpirtePos + Vector3.up * 10;

            int startIndexSave = startIndex;
            startIndex = endIndex;
            endIndex = (startIndexSave - 1 == -1) ? sprites.Length - 1 : startIndexSave - 1;
        } 
    }
}
