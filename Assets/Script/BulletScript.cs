using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class BulletScript : MonoBehaviour, IPunObservable
{
    [SerializeField] int bulletCode;
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
        curPosPv = new Vector3(16, 16, 0);
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
    private void OnDisable()
    {
        curPosPv = new Vector3(16, 16, 0);
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

        if (PhotonNetwork.IsMasterClient)
        {
            if(bulletCode == 1)
            transform.Translate(new Vector3(0, -0.1f));
        }





        if (!pv.IsMine)
        {
            if ((transform.position - curPosPv).sqrMagnitude >= 3) transform.position = curPosPv;
            else
            transform.position = Vector3.Lerp(transform.position, curPosPv, Time.deltaTime * 10);
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
            stream.SendNext(transform.position);
        }
        else
        {
            curPosPv = (Vector3)stream.ReceiveNext();
        }
    }
}
