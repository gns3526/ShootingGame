using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerCody : MonoBehaviourPun
{
    [SerializeField] int bodyCode;
    [SerializeField] Sprite[] bodyCodys;

    [SerializeField] SpriteRenderer spriteRenderer;


    [SerializeField] PhotonView pv;


    [PunRPC]
    void CodyRework(int body)
    {
        spriteRenderer.sprite = bodyCodys[body];
    }

}
