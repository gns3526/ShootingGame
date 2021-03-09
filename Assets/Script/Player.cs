using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class Player : MonoBehaviourPunCallbacks, IPunObservable
{
    public bool godMode;
    public bool isDie;

    [SerializeField] bool isTouchTop;
    [SerializeField] bool isTouchLeft;
    [SerializeField] bool isTouchRight;
    [SerializeField] bool isTouchBottom;

    [Header("Player Stats")]
    public int life;
    public int maxLife;
    [SerializeField] int maximumLife;
    public int score;

    public float moveSpeed;
    public int power;
    public int maxPower;
    public int increaseDamage;
    public int bossDamagePer;
    public int criticalPer;
    public int criticalDamagePer;

    public bool gotSpecialWeaponAbility;
    public int weaponCode;
    public float toTalChargeTime;
    public float curChargeTime;
    public int curBulletAmount;
    public int maxSpecialBullet;
    public float weaponTotalShotCoolTime;
    public float curWeaponShotCoolTime;

    public bool isSpecialBulletAbility1;
    public bool isSpecialBulletAbility2;

    [SerializeField] int boom;
    [SerializeField] int maxBoom;
    [SerializeField] float maxShotCoolTime;
    public float shotCoolTimeReduce;
    [SerializeField] float curShotCoolTime;
    public float godTime;
    public int missPercentage;

    public bool isAttackSpeedStack;
    public int attackSpeedStackint;
    public int attackSpeedStack;
    public bool isDamageStack;
    public int damageStackint;
    public int damageStack;

    [Header("PlayerCody")]
    [SerializeField] PhotonView codyPv;

    [Header("Others")]
    [SerializeField] GameObject playerPoint;

    [SerializeField] GameObject boomEffect;

    [SerializeField] Animator ani;
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] Rigidbody2D rigid;

    public GameManager GM;
    [SerializeField] ObjectPooler OP;

    [SerializeField] bool isBoomActive;

    public GameObject[] followers;
    public int followerAmount;

    public bool canHit;
    public bool isRespawned;

    public bool[] joyControl;
    public bool isControl;

    public bool isFire;
    public bool weaponFire;

    public bool specialShot;

    //public float[] playerColor;

    //Photon Panel

    NetworkManager NM;
    PhotonView NMPV;
    [SerializeField] Text nickNameText;
    public PhotonView pv;

    [SerializeField] Vector3 curPosPv;


    private void Awake()
    {
        GM = FindObjectOfType<GameManager>();
        NM = FindObjectOfType<NetworkManager>();
        OP = FindObjectOfType<ObjectPooler>();

        NMPV = NM.GetComponent<PhotonView>();
        nickNameText.text = pv.IsMine ? PhotonNetwork.NickName : pv.Owner.NickName;//NickName Setting

        for (int i = 0; i < followers.Length; i++)
        {
            followers[i].GetComponent<Follower>().OP = OP;
            followers[i].GetComponent<Follower>().player = this;
            followers[i].SetActive(false);
        }


        if (pv.IsMine)
        {
            playerPoint.SetActive(true);

            pv.RPC("ChangeColorRPC", RpcTarget.All, GM.playerColors[0], GM.playerColors[1], GM.playerColors[2]);
            codyPv.RPC("CodyRework", RpcTarget.All, GM.codyBodyCode);
        }


        /*
        maxLife = 5;
        life = maxLife;
        maximumLife = 10;
        score = 0;
        moveSpeed = 3;
        power = 1;
        maxPower = 6;
        increaseDamage = 100;
        bossDamagePer = 100;

        criticalPer = 20;
        criticalDamagePer = 100;

        gotSpecialWeaponAbility = false;
        weaponCode = 0;
        toTalChargeTime = 0;
        curChargeTime = toTalChargeTime;
        curBulletAmount = 0;
        maxSpecialBullet = 0;
        weaponTotalShotCoolTime = 0;
        curWeaponShotCoolTime = -1;

        isSpecialBulletAbility1 = false;
        isSpecialBulletAbility2 = false;

        maxShotCoolTime = 0.15f;
        shotCoolTimeReduce = 100;
        curShotCoolTime = maxShotCoolTime;

        godTime = 2;
        missPercentage = 0;

        isAttackSpeedStack = false;
        attackSpeedStackint = 0;
        attackSpeedStack = 0;
        isDamageStack = false;
        damageStackint = 0;
        damageStack = 0;
        */

        //pv.ViewID = 1000 + NM.playerInfoGroupInt;
        
    }
    private void OnEnable()
    {

        Unbeatable();

        if(pv.IsMine)
        Invoke("Unbeatable", 0.1f);//무적시간


    }
    

    void Unbeatable()
    {

    }

    private void Update()
    {
        if (pv.IsMine)
        {
            if (!NM.isChating)
            {
                Move();
                WeaponFire();
                Fire();
            }
            else
                rigid.velocity = new Vector2(0, 0);

            Reload();
            UseBoom();
            if (Input.GetKeyDown(KeyCode.Y))
            {
                pv.RPC("AddFollower", RpcTarget.All, 1);
                //AddFollower(1);
            }
            if (Input.GetKeyDown(KeyCode.U))
            {
                pv.RPC("AddFollower", RpcTarget.All, 2);
                //AddFollower(2);
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                GM.pv.RPC("ReviveTeam", RpcTarget.All, 5);
                //AddFollower(2);
            }
            if(Input.GetKeyDown(KeyCode.P))
            {
                life = 0;
                GM.UpdateLifeIcon(life);
                pv.RPC("PlayerIsDie", RpcTarget.All);
                GM.PlayerDie();
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                Cards card = FindObjectOfType<Cards>();
                card.CardS(19);
            }

            if (!GM.isAndroid)
            {
                if (Input.GetKey(KeyCode.A))
                {
                    weaponFire = false;
                    isFire = true;
                }
                else if (Input.GetKey(KeyCode.Z))
                {
                    isFire = false;
                    weaponFire = true;
                }
                else
                {
                    isFire = false;
                    weaponFire = false;
                }
            }

            if (maxSpecialBullet > curBulletAmount && GM.isPlaying)
            {
                curChargeTime -= Time.deltaTime;
                if(curChargeTime < 0)
                {
                    curChargeTime = toTalChargeTime;
                    curBulletAmount++;
                    GM.weaponBulletText.text = curBulletAmount.ToString();
                }
            }
            if(curWeaponShotCoolTime < weaponTotalShotCoolTime && GM.isPlaying)
            {
                curWeaponShotCoolTime += Time.deltaTime;
                GM.weaponShotButtonImage.fillAmount = curWeaponShotCoolTime / weaponTotalShotCoolTime;
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
        if (Input.GetKey(KeyCode.Space))
            rigid.velocity = new Vector2(h * 1.5f, v * 1.5f);
        else
            rigid.velocity = new Vector2((moveSpeed / 100) * 4 * h, (moveSpeed / 100) * 4 * v);

        if (Input.GetButtonDown("Horizontal") || Input.GetButtonUp("Horizontal"))
        {
            ani.SetInteger("AxisX", (int)h);
        }
    }


 
    void Fire()
    {
        if (!isFire) return;

        if (!GM.isPlaying) return;

        if (isDie) return;

        if (curShotCoolTime < maxShotCoolTime) return;

        int randomNum = Random.Range(0,101);

        if(isSpecialBulletAbility1 && (20 > randomNum))
        {
            GameObject bullet = OP.PoolInstantiate("AbilityBullet1", transform.position, Quaternion.identity);
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
            curShotCoolTime = 0;
            return;
        }
        if (isSpecialBulletAbility2 && (60 > randomNum))
        {
            GameObject bullet = OP.PoolInstantiate("AbilityBullet2", transform.position, Quaternion.identity);
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
            curShotCoolTime = 0;
            return;
        }

        switch (power)
        {
            case 1:
                GameObject bullet = OP.PoolInstantiate("PlayerBullet1", transform.position, Quaternion.identity);
                Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
                rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
            case 2:
                
                GameObject bulletR = OP.PoolInstantiate("PlayerBullet1", transform.position, Quaternion.identity);
                GameObject bulletL = OP.PoolInstantiate("PlayerBullet1", transform.position, Quaternion.identity);
                bulletR.transform.position = transform.position + Vector3.right * 0.1f;
                bulletL.transform.position = transform.position + Vector3.left * 0.1f;

                Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
                rigidR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

                Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
                rigidL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                
                break;
            case 3:
                
                GameObject bulletRR = OP.PoolInstantiate("PlayerBullet1", transform.position, Quaternion.identity);
                bulletRR.transform.position = transform.position + Vector3.right * 0.35f;

                GameObject bulletCC = OP.PoolInstantiate("PlayerBullet2", transform.position, Quaternion.identity);
                bulletCC.transform.position = transform.position;

                GameObject bulletLL = OP.PoolInstantiate("PlayerBullet1", transform.position, Quaternion.identity);
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
    void WeaponFire()
    {
        if (gotSpecialWeaponAbility && weaponFire)
        {
            if (weaponCode == 1 && curBulletAmount > 0 && curWeaponShotCoolTime > weaponTotalShotCoolTime)
            {
                GameObject bullet = OP.PoolInstantiate("AbilityBullet2", transform.position, Quaternion.identity);
                Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
                rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                curBulletAmount--;
                curWeaponShotCoolTime = 0;
            }
            GM.weaponBulletText.text = curBulletAmount.ToString();
            return;
        }
    }

    void Reload()
    {
        curShotCoolTime += Time.deltaTime * ((shotCoolTimeReduce + attackSpeedStack) / 100);
    }
    void UseBoom()//폭탄사용
    {
        //if (!Input.GetButton("Fire2"))
        //    return;


        if (isBoomActive)
            return;

        if (boom == 0)
            return;

        boom--;
        isBoomActive = true;
        GM.UpdateBoomIcon(boom);

        boomEffect.SetActive(true);
        Invoke("BoomFalse", 4);

        /*
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
        */
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

    [PunRPC]
    public void AddFollower(int type)
    {
        
        for (int i = 0; i < followers.Length; i++)
        {
            if (!followers[i].activeSelf)
            {
                followers[i].SetActive(true);
                Follower followerScript = followers[i].GetComponent<Follower>();
                pv.RPC("FollowerSpriteChangeRPC", RpcTarget.All,i,type);
                switch (type)
                {
                    case 1:
                        followerScript.maxShotCoolTime = 0.2f;
                        followerScript.bulletType = 1;
                        break;
                    case 2:
                        followerScript.maxShotCoolTime = 2f;
                        followerScript.bulletType = 2;
                        break;
                }
                break;
            }
        }
    }

    [PunRPC]
    void FollowerSpriteChangeRPC(int i, int type)
    {
        followerAmount++;
        if(type == 1)
            followers[i].GetComponent<SpriteRenderer>().sprite = Resources.Load("PetSprite" + "/" + "Num1", typeof(Sprite)) as Sprite;

        else if(type == 2)
            followers[i].GetComponent<SpriteRenderer>().sprite = Resources.Load("PetSprite" + "/" + "Num2", typeof(Sprite)) as Sprite;
    }


    [PunRPC]
    void ChangeColorRPC(float r,float g, float b)
    {
        GetComponent<SpriteRenderer>().color = new Color(r, g, b, 1);
    }



    void BoomFalse()
    {
        boomEffect.SetActive(false);
        isBoomActive = false;
    }

    [PunRPC]
    void PlayerIsDie()
    {
        isDie = true;
    }

    [PunRPC]
    void PlayerIsAlive()
    {
        isDie = false;
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
            //stream.SendNext(playerColor);
        }
        else
        {
            curPosPv = (Vector3)stream.ReceiveNext();
            //playerColor = (float[])stream.ReceiveNext();
        }
    }
}
