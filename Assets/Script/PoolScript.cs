using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PoolScript : MonoBehaviourPun
{
    [SerializeField] float[] bulletSpeed;

    [Header("BulletAni")]
    int aniCode;
    [Header("Boss3")]
    [SerializeField] Sprite[] ani1;
    [SerializeField] Sprite[] ani2;
    [SerializeField] Sprite[] ani3;

    [Header("PlayerBullet")]
    [SerializeField] Sprite[] bulletSpriteP;
    [SerializeField] Vector2[] bulletScaleP;
    [SerializeField] Vector2[] bulletBoxSizeP;
    [SerializeField] Vector2[] bulletBoxOffsetP;


    [Header("EnemyBullet")]
    [SerializeField] Sprite[] bulletSpriteE;
    [SerializeField] Vector2[] bulletScaleE;
    [SerializeField] Vector2[] bulletBoxSizeE;
    [SerializeField] Vector2[] bulletBoxOffsetE;

    //-2 = Objects
    //-1 = SpecialBullet
    //0++ = Bullets
    [PunRPC]
    void SetActiveRPC(bool a, int bulletIndex, int bulletAniCode, int bulletSpeedIndex, bool isPlayerAttack)
    {
        aniCode = bulletAniCode;
        if(bulletIndex > -1)
        {
            BulletScript bs = GetComponent<BulletScript>();
            bs.isPlayerAttack = isPlayerAttack;
            if (isPlayerAttack)
            {
                bs.bulletSpeed = -bulletSpeed[bulletSpeedIndex];
                GetComponent<SpriteRenderer>().sprite = bulletSpriteP[bulletIndex];
                transform.localScale = bulletScaleP[bulletIndex];
                GetComponent<BoxCollider2D>().size = bulletBoxSizeP[bulletIndex];
                GetComponent<BoxCollider2D>().offset = bulletBoxOffsetP[bulletIndex];
            }
            else
            {
                bs.bulletSpeed = bulletSpeed[bulletSpeedIndex];
                GetComponent<SpriteRenderer>().sprite = bulletSpriteE[bulletIndex];
                transform.localScale = bulletScaleE[bulletIndex];
                GetComponent<BoxCollider2D>().size = bulletBoxSizeE[bulletIndex];
                GetComponent<BoxCollider2D>().offset = bulletBoxOffsetE[bulletIndex];
            }
            if(bulletAniCode > -1)
            {
                BulletAni();
            }
        }
        else if(bulletIndex == -1)
        {
            BulletScript bs = GetComponent<BulletScript>();
            bs.isPlayerAttack = isPlayerAttack;
            if (isPlayerAttack)
                bs.bulletSpeed = -bulletSpeed[bulletSpeedIndex];
            else
                bs.bulletSpeed = bulletSpeed[bulletSpeedIndex];
        }

        gameObject.SetActive(a);
    }

    void BulletAni()
    {
        BulletScript bs = GetComponent<BulletScript>();
        switch (aniCode)
        {
            case 0:
                for (int i = 0; i < ani1.Length; i++)
                    bs.bulletAniSprites[i] = ani1[i];
                bs.bulletAniDelayCode = 0;
                break;
            case 1:
                for (int i = 0; i < ani2.Length; i++)
                    bs.bulletAniSprites[i] = ani2[i];
                bs.bulletAniDelayCode = 0;
                break;
            case 2:
                for (int i = 0; i < ani3.Length; i++)
                    bs.bulletAniSprites[i] = ani3[i];
                bs.bulletAniDelayCode = 4;
                break;
        }
    }
}
