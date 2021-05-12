using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Enemy15 : MonoBehaviour
{
    [SerializeField] EnemyBasicScript EB;

    [SerializeField] GameObject enemy1;
    [SerializeField] GameObject enemy2;


    [SerializeField] float[] moveSpeedRange;

    float randomSpeed1;
    float randomSpeed2;

    private void OnEnable()
    {
        randomSpeed1 = Random.Range(moveSpeedRange[0], moveSpeedRange[1]);
        randomSpeed2 = Random.Range(moveSpeedRange[0], moveSpeedRange[1]);
        EB.healthBarGameObject.transform.rotation = Quaternion.identity;
    }
    private void Update()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        enemy1.transform.Translate(new Vector2(0, -randomSpeed1 * Time.deltaTime));
        enemy2.transform.Translate(new Vector2(0, -randomSpeed2 * Time.deltaTime));

    }
}
