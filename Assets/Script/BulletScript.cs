using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class BulletScript : MonoBehaviour, IPunObservable
{
    [SerializeField] int bulletCode;
    public int dmg;
    public float bulletSpeed;
    [SerializeField] float maxBulletDestroyTime;
    float bulletDestroyTime;
    [SerializeField] bool isPlayerAttack;
    [SerializeField] bool isRotate;
    [SerializeField] bool isPassThrough;
    public GameObject target;
    public bool isFollowTarget;


    [SerializeField] PhotonView pv;
    public GameObject parentOb;

    public bool isBossBullet;

    [SerializeField] ObjectPooler OP;
    GameManager GM;

    Vector3 curPosPv;

    public BoxCollider2D boxCol;
    public Animator animator;

    bool once;

    private void Awake()
    {
        GM = FindObjectOfType<GameManager>();
        OP = FindObjectOfType<ObjectPooler>();
    }

    private void OnEnable()
    {
        if(animator != null)
           animator.SetBool("Start",true);

        gameObject.transform.position = new Vector3(16, 16, 0);

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
        bulletDestroyTime = maxBulletDestroyTime;
        curPosPv = new Vector3(16, 16, 0);
        bulletSpeed = 0;
        isBossBullet = false;

        if (animator != null)
            animator.SetBool("Start", false);
        parentOb = null;
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

        if (PhotonNetwork.IsMasterClient && !isBossBullet)
        {
            transform.Translate(new Vector3(0, -bulletSpeed));
            bulletDestroyTime -= Time.deltaTime;
            if(bulletDestroyTime < 0) OP.PoolDestroy(gameObject);
            if (isFollowTarget && target != null)
            {
                float angle = Mathf.Atan2(target.transform.position.y - gameObject.transform.position.y, target.transform.position.x - gameObject.transform.position.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle + 90);
            }
        }

        if (parentOb != null)
        {
            transform.position = parentOb.transform.position;
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
                OP.PoolDestroy(gameObject);
            }
        }
        else
        {
            if (isPassThrough) return;
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
