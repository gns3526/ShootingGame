using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class Player : MonoBehaviourPunCallbacks, IPunObservable
{
    public bool godMode;

    [SerializeField] bool isTouchTop;
    [SerializeField] bool isTouchLeft;
    [SerializeField] bool isTouchRight;
    [SerializeField] bool isTouchBottom;

    public int life;
    public int maxLife;
    [SerializeField] int maximumLife;
    public int score;
    public float moveSpeed;
    public int power;
    public int maxPower;
    public int increaseDamage;
    [SerializeField] int boom;
    [SerializeField] int maxBoom;
    [SerializeField] float maxShotCoolTime;
    public float shotCoolTimeReduce;
    [SerializeField] float curShotCoolTime;

    [SerializeField] GameObject bullet0;
    [SerializeField] GameObject bullet1;
    [SerializeField] GameObject boomEffect;

    [SerializeField] Animator ani;
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] Rigidbody2D rigid;

    [SerializeField] GameManager GM;
    [SerializeField] ObjectManager OM;

    [SerializeField] bool isBoomActive;

    [SerializeField] GameObject[] followers;
    [SerializeField] Sprite[] followerSprites;

    public bool canHit;
    public bool isRespawned;

    public bool[] joyControl;
    public bool isControl;
    [SerializeField] bool isButtenA;
    [SerializeField] bool isButtenB;

    public float dmgPer;
    public float firePer;


    //Photon Panel

    NetwordManager NM;
    [SerializeField] Text nickNameText;
    [SerializeField] PhotonView pv;

    [SerializeField] Vector3 curPosPv;


    private void Awake()
    {
        GM = FindObjectOfType<GameManager>();
        OM = FindObjectOfType<ObjectManager>();
        NM = FindObjectOfType<NetwordManager>();

        GM.player = gameObject;
        NM.player = this;

        nickNameText.text = pv.IsMine ? PhotonNetwork.NickName : pv.Owner.NickName;//NickName Setting
    }
    private void OnEnable()
    {
        Unbeatable();
        GM.UpdateLifeIcon(life);
        Invoke("Unbeatable", 3);//무적시간
    }
    void Unbeatable()
    {
        isRespawned = !isRespawned;
        if (isRespawned)
        {
            isRespawned = true;
            sprite.color = new Color(1, 1, 1, 0.5f);//투명도
            for (int i = 0; i < followers.Length; i++)
            {
                followers[i].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
            }
        }
        else
        {
            isRespawned = false;
            sprite.color = new Color(1, 1, 1, 1);//투명도
            for (int i = 0; i < followers.Length; i++)
            {
                followers[i].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            }
        }
    }

    private void Update()
    {
        if (pv.IsMine)
        {
            Move();
            Fire();
            Reload();
            UseBoom();
            if (Input.GetKeyDown(KeyCode.R))
            {
                AddFollower(1);
            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                AddFollower(2);
            }
        }
        else if((transform.position - curPosPv).sqrMagnitude >= 100)
        {
            transform.position = curPosPv;
        }
        else//Moving Softly
        {
            transform.position = Vector3.Lerp(transform.position, curPosPv, Time.deltaTime * 10);
        }

       
    }

    public void JoyPanel(int type)
    {
        for (int i = 0; i < 9; i++)//어디방향키 눌렀나 확인
        {
            joyControl[i] = i == type;
        }
    }
    public void JoyDown()
    {
        isControl = true;
    }
    public void JoyUp()
    {
        isControl = false;
    }

    void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if ((isTouchRight && h == 1) || (isTouchLeft && h == -1)/* || !isControl*/) h = 0;
        if ((isTouchTop && v == 1) || (isTouchBottom && v == -1)/* || !isControl*/) v = 0;
        //Vector3 curPos = transform.position;
        //Vector3 nextPos = new Vector3(h, v, 0) * moveSpeed * Time.deltaTime;

        //transform.position = curPos + nextPos;

        //transform.Translate(Vector3.right * 7 * Time.deltaTime * h);
        rigid.velocity = new Vector2(4 * h, 4 * v);

        if (Input.GetButtonDown("Horizontal") || Input.GetButtonUp("Horizontal"))
        {
            ani.SetInteger("AxisX", (int)h);
        }
    }

    public void ButtonADown()
    {
        isButtenA = true;
    }
    public void ButtonAUp()
    {
        isButtenA = false;
    }
    public void ButtonBDown()
    {
        isButtenB = true;
    }
    public void BottonBUp()
    {
        isButtenB = false;
    }

 
    void Fire()
    {
        if (!Input.GetButton("Fire1")) return;

        //if (!isButtenA) return;

        if (curShotCoolTime < (maxShotCoolTime * (shotCoolTimeReduce / 100))) return;

        switch (power)
        {
            case 1:

                pv.RPC("shhotiung", RpcTarget.AllBuffered);
                /* GameObject bullet = OM.MakeObj("BulletPlayer0");
                 bullet.transform.position = transform.position;
                 Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
                 rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);*/
                break;
            case 2:
                GameObject bulletR = OM.MakeObj("BulletPlayer0");
                GameObject bulletL = OM.MakeObj("BulletPlayer0");
                bulletR.transform.position = transform.position + Vector3.right * 0.1f;
                bulletL.transform.position = transform.position + Vector3.left * 0.1f;

                Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
                rigidR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

                Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
                rigidL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
            default:
                GameObject bulletRR = OM.MakeObj("BulletPlayer0");
                bulletRR.transform.position = transform.position + Vector3.right * 0.35f;

                GameObject bulletCC = OM.MakeObj("BulletPlayer1");
                bulletCC.transform.position = transform.position;

                GameObject bulletLL = OM.MakeObj("BulletPlayer0");
                bulletLL.transform.position = transform.position + Vector3.left * 0.35f;

                Rigidbody2D rigidRR = bulletRR.GetComponent<Rigidbody2D>();
                rigidRR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

                Rigidbody2D rigidCC = bulletCC.GetComponent<Rigidbody2D>();
                rigidCC.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

                Rigidbody2D rigidLL = bulletLL.GetComponent<Rigidbody2D>();
                rigidLL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
        }


        curShotCoolTime = 0;
    }

    [PunRPC]
    void shhotiung()
    {
        GameObject bullet = OM.MakeObj("BulletPlayer0");
        bullet.SetActive(true); // test

        bullet.transform.position = transform.position;
        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
        rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
    }
    void Reload()
    {
        curShotCoolTime += Time.deltaTime;
    }
    void UseBoom()//폭탄사용
    {
        //if (!Input.GetButton("Fire2"))
        //    return;
        if (!isButtenB)
            return;

        if (isBoomActive)
            return;

        if (boom == 0)
            return;

        boom--;
        isBoomActive = true;
        GM.UpdateBoomIcon(boom);

        boomEffect.SetActive(true);
        Invoke("BoomFalse", 4);

        GameObject[] enemy1 = OM.GetPool("1");
        GameObject[] enemy2 = OM.GetPool("2");
        GameObject[] enemy3 = OM.GetPool("3");
        GameObject[] enemy4 = OM.GetPool("4");
        for (int i = 0; i < enemy1.Length; i++)//모든적데미지주기
        {
            if (enemy1[i].activeSelf)
            {
                EnemyScript enemyScript = enemy1[i].GetComponent<EnemyScript>();
                enemyScript.Hit(1000);
            }
        }
        for (int i = 0; i < enemy2.Length; i++)//모든적데미지주기
        {
            if (enemy2[i].activeSelf)
            {
                EnemyScript enemyScript = enemy2[i].GetComponent<EnemyScript>();
                enemyScript.Hit(1000);
            }
        }
        for (int i = 0; i < enemy3.Length; i++)//모든적데미지주기
        {
            if (enemy3[i].activeSelf)
            {
                EnemyScript enemyScript = enemy3[i].GetComponent<EnemyScript>();
                enemyScript.Hit(1000);
            }
        }
        for (int i = 0; i < enemy4.Length; i++)//모든적데미지주기
        {
            if (enemy4[i].activeSelf)
            {
                EnemyScript enemyScript = enemy4[i].GetComponent<EnemyScript>();
                enemyScript.Hit(1000);
            }
        }

        GameObject[] BulletEnemy0 = OM.GetPool("BulletEnemy0");
        GameObject[] BulletEnemy1 = OM.GetPool("BulletEnemy1");
        GameObject[] BulletEnemy2 = OM.GetPool("BulletEnemy2");
        GameObject[] BulletEnemy3 = OM.GetPool("BulletEnemy3");

        for (int i = 0; i < BulletEnemy0.Length; i++)
        {
            if (BulletEnemy0[i].activeSelf)
            {
                BulletEnemy0[i].SetActive(false);
            }
        }
        for (int i = 0; i < BulletEnemy1.Length; i++)
        {
            if (BulletEnemy1[i].activeSelf)
            {
                BulletEnemy1[i].SetActive(false);
            }
        }
        for (int i = 0; i < BulletEnemy2.Length; i++)
        {
            if (BulletEnemy2[i].activeSelf)
            {
                BulletEnemy2[i].SetActive(false);
            }
        }
        for (int i = 0; i < BulletEnemy3.Length; i++)
        {
            if (BulletEnemy3[i].activeSelf)
            {
                BulletEnemy3[i].SetActive(false);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Border")
        {
            switch (other.gameObject.name)
            {
                case "Top":
                    isTouchTop = true;
                    break;
                case "Right":
                    isTouchRight = true;
                    break;
                case "Left":
                    isTouchLeft = true;
                    break;
                case "Bottom":
                    isTouchBottom = true;
                    break;
            }
        }
        else if (other.tag == "Enemy" || other.tag == "EnemyBullet")
        {
            if (isRespawned)
                return;

            if (godMode)
                return;

            if (canHit)
            {
                life--;
            }
            canHit = false;
            GM.UpdateLifeIcon(life);
            GM.MakeExplosionEffect(transform.position, "Player");//폭발이펙트

            if(life == 0)
            {
                GM.GameOver();
            }
            else
            {
                
                GM.StartCoroutine("ReSpawnM");
            }
            gameObject.SetActive(false);
            
        }
        else if(other.tag == "Item")
        {
            Item itemScript = other.GetComponent<Item>();
            switch (itemScript.type)
            {
                case "Coin":
                    score += 1000;
                    break;
                    /*
                case "Pow":
                    if (power == maxPower)
                    {
                        score += 500;
                    }
                    else
                    {
                        power++;
                        AddFollower();
                    }
                    break;*/
                case "Boom":
                    if (boom == maxBoom)
                    {
                        score += 500;
                    }
                    else
                    {
                        boom++;
                        GM.UpdateBoomIcon(boom);
                    }
                    break;
            }
            other.gameObject.SetActive(false);
        }
    }

    public void AddFollower(int type)
    {
        for (int i = 0; i < followers.Length; i++)
        {
            if (!followers[i].activeSelf)
            {
                followers[i].SetActive(true);
                Follower followerScript = followers[i].GetComponent<Follower>();
                SpriteRenderer followersprite = followers[i].GetComponent<SpriteRenderer>();
                switch (type)
                {
                    case 1:
                        followerScript.maxShotCoolTime = 0.2f;
                        followerScript.bulletType = 1;
                        followersprite.sprite = followerSprites[0];
                        break;
                    case 2:
                        followerScript.maxShotCoolTime = 2f;
                        followerScript.bulletType = 2;
                        followersprite.sprite = followerSprites[1];
                        break;
                }
                break;
            }
        }
    }

    void BoomFalse()
    {
        boomEffect.SetActive(false);
        isBoomActive = false;
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Border")
        {
            switch (other.gameObject.name)
            {
                case "Top":
                    isTouchTop = false;
                    break;
                case "Right":
                    isTouchRight = false;
                    break;
                case "Left":
                    isTouchLeft = false;
                    break;
                case "Bottom":
                    isTouchBottom = false;
                    break;
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
