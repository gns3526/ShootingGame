using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class BulletScript : MonoBehaviour, IPunObservable
{
    public bool isSpecialBullet;
    public int dmgPer;
    public float dmg;
    public float bulletSpeed;
    [SerializeField] float maxBulletDestroyTime;
    float bulletDestroyTime;
    public bool isPlayerAttack;
    public bool ispetAttack;
    [SerializeField] bool isRotate;
    [SerializeField] bool isPassThrough;
    public GameObject target;
    public bool isFollowTarget;
    public int attackAmount;

    [SerializeField] PhotonView pv;
    public GameObject parentOb;

    public bool isBossBullet;

    public ObjectPooler OP;
    public GameManager GM;
    PlayerState ps;

    Vector3 curPosPv;

    public BoxCollider2D boxCol;
    public CircleCollider2D circleCol;
    public Animator animator;
    [SerializeField] SpriteRenderer spriteRender;
    public Sprite[] bulletAniSprites;
    [SerializeField] Rigidbody2D rigid;

    public int homingPower;

    public int bulletAniDelayCode;
    public float[] bulletAniDelays;

    bool once;


    private void Awake()
    {
        GM = FindObjectOfType<GameManager>();
        OP = GM.OP;
        ps = GM.ps;
    }
    private void OnEnable()
    {
        bulletDestroyTime = maxBulletDestroyTime;

        if (animator != null)
           animator.SetBool("Start",true);



        once = true;
        Delay();

        //if(isPlayerAttack)
        //    transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 0, 90);
        //else
        //    transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 0, -90);
    }
    void Delay()
    {
        if (bulletAniSprites[0] != null)
            StartCoroutine(BulletAni());
    }
    private void OnDisable()
    {
        if (!isSpecialBullet)
        {
            spriteRender.sprite = null;
            boxCol.size = new Vector2(0.01f, 0.01f);
            boxCol.offset = new Vector2(0.01f, 0.01f);
        }
        curPosPv = new Vector3(16, 16, 0);

        Invoke("ResetBulletInfo", 0.1f);

        isBossBullet = false;

        if (animator != null)
            animator.SetBool("Start", false);
        parentOb = null;

        if (bulletAniSprites[0] != null)
            for (int i = 0; i < bulletAniSprites.Length; i++)
                bulletAniSprites[i] = null;
        bulletAniDelayCode = 0;
    }

    void ResetBulletInfo()
    {
        dmg = 0;
        bulletSpeed = 0;
        dmgPer = 0;
        attackAmount = 1;
        ispetAttack = false;
        isFollowTarget = false;
        target = null;

        transform.rotation = Quaternion.identity;
        //transform.GetChild(0).transform.rotation = Quaternion.identity;
    }

    private void Update()
    {
        if (pv.IsMine)
        {
            

            bulletDestroyTime -= Time.deltaTime;
            if(bulletDestroyTime < 0) OP.PoolDestroy(gameObject);
            if (isFollowTarget && target.activeSelf)
            {
                //float angle = Mathf.Atan2(target.transform.position.y - gameObject.transform.position.y, target.transform.position.x - gameObject.transform.position.x) * Mathf.Rad2Deg;
                //transform.rotation = Quaternion.Euler(0, 0, angle + 90);
                if(isPlayerAttack)
                    //transform.Translate(-bulletSpeed * Time.deltaTime, 0, 0);
                    transform.Translate(0, -bulletSpeed * Time.deltaTime, 0);
                else
                    //transform.Translate(bulletSpeed * Time.deltaTime, 0, 0);
                    transform.Translate(0, bulletSpeed * Time.deltaTime, 0);
                Vector2 direction = transform.position - target.transform.position;

                direction.Normalize();

                float cross = Vector3.Cross(direction, transform.up).z;

                rigid.angularVelocity = cross * homingPower;
            }
            else
            {
                if(rigid != null)
                rigid.angularVelocity = 0;
                transform.Translate(new Vector3(0, -bulletSpeed * Time.deltaTime));
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
        if(!pv.IsMine && isPlayerAttack)
        transform.position = curPosPv;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (isPlayerAttack)
        {
            if (isPassThrough) return;
            if (other.tag == "BulletBorder")
            {
                OP.PoolDestroy(gameObject);
            }
            else if(other.tag == "Enemy")
            {
                attackAmount--;

                int penetrate = Random.Range(0, 101);
                if (penetrate <= ps.penetratePer)
                    attackAmount++;

                if(attackAmount < 1)
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

    bool stopCor;
    public IEnumerator BulletAni()
    {
        Debug.Log("실행");
        stopCor = false;
        yield return new WaitForSeconds(bulletAniDelays[bulletAniDelayCode]);
        spriteRender.sprite = bulletAniSprites[0]; // 1

        yield return new WaitForSeconds(bulletAniDelays[bulletAniDelayCode]);
        spriteRender.sprite = bulletAniSprites[1]; // 2

        if (bulletAniSprites[2] != null)
        {
            yield return new WaitForSeconds(bulletAniDelays[bulletAniDelayCode]);
            spriteRender.sprite = bulletAniSprites[2]; // 3
        }
        else
            stopCor = true;

        if (!stopCor)
        {
            yield return new WaitForSeconds(bulletAniDelays[bulletAniDelayCode]);
            if (bulletAniSprites[3] != null)
                spriteRender.sprite = bulletAniSprites[3]; // 4
        }

        StartCoroutine(BulletAni());
    }


    public void DestroyObject()
    {
        GM.OP.PoolDestroy(gameObject);
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
