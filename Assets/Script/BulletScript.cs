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
    GameManager GM;

    Vector3 curPosPv;

    public BoxCollider2D boxCol;

    bool once;

    private void Awake()
    {
        GM = FindObjectOfType<GameManager>();
    }

    private void OnEnable()
    {
        OP = FindObjectOfType<ObjectPooler>();
        if (isPlayerAttack && once)
        {
            if (pv.IsMine)
            {
                boxCol.enabled = true;
            }
            else
            {
                boxCol.enabled = false;
            }
        }
        once = true;
    }

    private void Update()
    {
        if (isRotate)
        {
            transform.Rotate(Vector3.forward * 10);
        }

        if (!GM.isPlaying)
        {
            OP.PoolDestroy(gameObject);
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
