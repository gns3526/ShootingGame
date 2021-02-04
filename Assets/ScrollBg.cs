using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBg : MonoBehaviour
{
    [SerializeField] SpriteRenderer Scroll;
    [SerializeField] float ScrollSpeed;
    Vector2 newOffset;
    // Start is called before the first frame update
    void Start()
    {
        Vector2 newOffset = Scroll.material.mainTextureOffset;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 newOffset = Scroll.material.mainTextureOffset;
        newOffset.Set(0, newOffset.y + (ScrollSpeed * Time.deltaTime));
        Scroll.material.mainTextureOffset = newOffset;
    }
}
