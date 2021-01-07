using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class BulletScript : MonoBehaviour
{
    public int dmg;
    [SerializeField] bool isPlayerAttack;
    [SerializeField] bool isRotate;
    [SerializeField] bool isPassThrough;
    [SerializeField] PhotonView pv;

    private void Update()
    {
        if (isRotate)
        {
            transform.Rotate(Vector3.forward * 10);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isPlayerAttack)
        {
            if (other.tag == "BulletBorder" || other.tag == "Enemy")
            {
                Debug.Log("111111");
                pv.RPC("TurnOffRpc", RpcTarget.All, false);
            }
            
        }
        else
        {
            if (other.tag == "BulletBorder")
            {
                pv.RPC("TurnOffRpc", RpcTarget.All, false);
            }
        }
    }
    [PunRPC]
    void TurnOffRpc(bool actives)
    {
        Debug.Log("222222");
        gameObject.SetActive(actives);
    }
}
