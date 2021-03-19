using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

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
        if (!PhotonNetwork.IsMasterClient) return;
        transform.Translate(new Vector2(0, -moveSpeed));
    }
}
