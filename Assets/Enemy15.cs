using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Enemy15 : MonoBehaviour
{
    [SerializeField] EnemyBasicScript EB;
    [SerializeField] LineScript lineScript;

    [SerializeField] GameObject enemy1;
    [SerializeField] GameObject enemy2;


    [SerializeField] float[] moveSpeedRange;

    float randomSpeed1;
    float randomSpeed2;

    [SerializeField] GameObject wire;
    [SerializeField] GameObject spawnedOb;

    [SerializeField] GameObject hpBar1;
    [SerializeField] GameObject hpBar2;
    bool once;


    private void OnEnable()
    {
        randomSpeed1 = Random.Range(moveSpeedRange[0], moveSpeedRange[1]);
        randomSpeed2 = Random.Range(moveSpeedRange[0], moveSpeedRange[1]);
        EB.healthBarGameObject.transform.rotation = Quaternion.identity;

        if (once)
        {
            StartCoroutine(Delay());
        }
        once = true;
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.5f);
        spawnedOb = EB.GM.OP.PoolInstantiate("ElectricWire", transform.position, Quaternion.identity, -1, -1, 0, false);
        BulletScript bs = spawnedOb.GetComponent<BulletScript>();
        bs.parentOb = enemy1;
        wire = spawnedOb;
    }

    private void OnDisable()
    {
        EB.GM.OP.PoolDestroy(spawnedOb);
    }
    private void Update()
    {
        hpBar1.transform.position = enemy1.transform.position;
        hpBar2.transform.position = enemy2.transform.position;

        if (!PhotonNetwork.IsMasterClient) return;
        enemy1.transform.Translate(new Vector2(0, -randomSpeed1 * Time.deltaTime));
        enemy2.transform.Translate(new Vector2(0, -randomSpeed2 * Time.deltaTime));

        float angle = Mathf.Atan2(enemy1.transform.position.y - enemy2.transform.position.y, enemy1.transform.position.x - enemy2.transform.position.x) * Mathf.Rad2Deg;

        wire.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
