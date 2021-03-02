using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    [SerializeField] EnemyBasicScript EB;
    [SerializeField] GameManager GM;

    [SerializeField] float moveSpeed;

    private void OnEnable()
    {
        EB.healthBarGameObject.transform.rotation = Quaternion.identity;
    }
    private void Update()
    {
        transform.Translate(new Vector2(0, -moveSpeed));
    }
}
