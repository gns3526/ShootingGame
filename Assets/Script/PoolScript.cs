using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PoolScript : MonoBehaviourPun
{
    [SerializeField] float[] bulletSpeed;

    [Header("PlayerBullet")]
    [SerializeField] int[] bulletDamage;
    [SerializeField] Sprite[] bulletSpriteP;
    [SerializeField] Vector2[] bulletScaleP;
    [SerializeField] Vector2[] bulletBoxSizeP;
    [SerializeField] Vector2[] bulletBoxOffsetP;

    [Header("EnemyBullet")]
    [SerializeField] Sprite[] bulletSpriteE;
    [SerializeField] Vector2[] bulletScaleE;
    [SerializeField] Vector2[] bulletBoxSizeE;
    [SerializeField] Vector2[] bulletBoxOffsetE;


    [PunRPC]
    void SetActiveRPC(bool a, int bulletIndex, int bulletSpeedIndex, bool isPlayerAttack)
    {
        if(bulletIndex > -1)
        { 

            if (isPlayerAttack)
            {
                GetComponent<BulletScript>().dmg = bulletDamage[bulletIndex];
                GetComponent<BulletScript>().bulletSpeed = -bulletSpeed[bulletSpeedIndex];
                GetComponent<BulletScript>().isPlayerAttack = isPlayerAttack;
                GetComponent<SpriteRenderer>().sprite = bulletSpriteP[bulletIndex];
                transform.localScale = bulletScaleP[bulletIndex];
                GetComponent<BoxCollider2D>().size = bulletBoxSizeP[bulletIndex];
                GetComponent<BoxCollider2D>().offset = bulletBoxOffsetP[bulletIndex];
            }
            else
            {
                GetComponent<BulletScript>().bulletSpeed = bulletSpeed[bulletSpeedIndex];
                GetComponent<BulletScript>().isPlayerAttack = isPlayerAttack;
                GetComponent<SpriteRenderer>().sprite = bulletSpriteE[bulletIndex];
                transform.localScale = bulletScaleE[bulletIndex];
                GetComponent<BoxCollider2D>().size = bulletBoxSizeE[bulletIndex];
                GetComponent<BoxCollider2D>().offset = bulletBoxOffsetE[bulletIndex];
            }
        }
        gameObject.SetActive(a);
    }
}
