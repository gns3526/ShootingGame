using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class BulletScript : MonoBehaviour, IPunObservable
{
    public int dmg;
    [SerializeField] bool isPlayerAttack;
    [SerializeField] bool isRotate;
    [SerializeField] bool isPassThrough;
    [SerializeField] PhotonView pv;

    [SerializeField] ObjectPooler OP;

    Vector3 curPosPv;

    [SerializeField] BoxCollider2D boxCol;
    private void OnEnable()
    {
        OP = FindObjectOfType<ObjectPooler>();
        if (isPlayerAttack)
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                //boxCol.enabled = false;
            }
            else
            {

            }
        }
    }

    private void Update()
    {
        if (isRotate)
        {
            transform.Rotate(Vector3.forward * 10);
        }
        //if(!pv.IsMine && isPlayerAttack)
        //transform.position = curPosPv;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isPlayerAttack)
        {
            if (other.tag == "BulletBorder" || other.tag == "Enemy")
            {
                Debug.Log("111111");
                OP.PoolDestroy(gameObject);
            }
        }
        else
        {
            if (other.tag == "BulletBorder")
            {
                OP.PoolDestroy(gameObject);
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)//isMine = true
        {
            //if(isPlayerAttack)
            //stream.SendNext(transform.position);
        }
        else
        {
            //if(isPlayerAttack)
            //curPosPv = (Vector3)stream.ReceiveNext();
        }
    }
}
