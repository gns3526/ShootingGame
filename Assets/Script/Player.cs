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
    [SerializeField] Text lifeText;
    public int maxLife;
    public int score;

    public float moveSpeed;
    public int power;
    public int maxPower;
    public int damage;
    public int increaseDamagePer;
    public int bossDamagePer;
    public int normalMonsterDamagePer;
    public int criticalPer;
    public int criticalDamagePer;
    public int followerDamagePer;
    public int followerShotCoolReduce;
    public int penetratePer;
    public int finalDamagePer;
    public int goldAmountPer;
    public int expAmountPer;

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
    public float maxShotCoolTime;
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
    public PhotonView codyPv;

    [Header("Skill")]
    public BarrierScript skillC;

    [Header("Managers")]
    public GameManager GM;
    public ObjectPooler OP;
    [SerializeField] AbilityManager AM;
    public JobManager JM;

    [Header("Others")]
    [SerializeField] GameObject playerPoint;

    [SerializeField] GameObject boomEffect;

    [SerializeField] Animator ani;
    [SerializeField] SpriteRenderer mainSprite;
    [SerializeField] Rigidbody2D rigid;

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

    GameObject bullet;
    BulletScript bs;

    private void Awake()
    {
        GM = FindObjectOfType<GameManager>();
        NM = FindObjectOfType<NetworkManager>();
        OP = FindObjectOfType<ObjectPooler>();
        AM = FindObjectOfType<AbilityManager>();
        JM = FindObjectOfType<JobManager>();

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

            codyPv.RPC("CodyRework", RpcTarget.All, GM.codyMainCode, GM.codyBodyCode, GM.codyParticleCode);

            pv.RPC("ChangeColorRPC", RpcTarget.All, GM.playerColors[0], GM.playerColors[1], GM.playerColors[2]);

            StartCoroutine(StartDelay());
        }
    }


    private void OnEnable()
    {

        Unbeatable();

        if(pv.IsMine)
        Invoke("Unbeatable", 0.1f);//무적시간


    }
    IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(0.1f);
        JM.JobApply();
        AM.AbilityApply();
    }

    void Unbeatable()
    {

    }

    private void Update()
    {
        lifeText.text = life.ToString();
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

            if (JM.curSkillCool < JM.skillCoolC)
            {
                JM.curSkillCool += Time.deltaTime;
                JM.skillGuage.fillAmount = JM.curSkillCool / JM.skillCoolC;

                if (JM.curSkillCool > JM.skillCoolC)
                {
                    JM.skillBtn.interactable = true;
                    skillC.pv.RPC(nameof(skillC.BarrierOn), RpcTarget.All, false);
                }
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
            GameObject bullet = OP.PoolInstantiate("BulletBasic", transform.position, Quaternion.identity, 2, -1, 8, true);
            bullet.GetComponent<BulletScript>().dmgPer = 200;
            curShotCoolTime = 0;
            return;
        }
        if (isSpecialBulletAbility2 && (60 > randomNum))
        {
            GameObject bullet = OP.PoolInstantiate("BulletBasic", transform.position, Quaternion.identity, 1, -1, 8, true);
            bullet.GetComponent<BulletScript>().dmgPer = 130;
            curShotCoolTime = 0;
            return;
        }

        JM.Shot();
        curShotCoolTime = 0;
    }

    void WeaponFire()
    {
        if (gotSpecialWeaponAbility && weaponFire)
        {
            if (weaponCode == 1 && curBulletAmount > 0 && curWeaponShotCoolTime > weaponTotalShotCoolTime)
            {
                GameObject bullet = OP.PoolInstantiate("BulletBasic", transform.position, Quaternion.identity, 3, -1, 9, true);
                bullet.GetComponent<BulletScript>().dmgPer = 2000;
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
        mainSprite.color = new Color(r/255f, g/255f, b/255f, 1);
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
            stream.SendNext(life);
        }
        else
        {
            curPosPv = (Vector3)stream.ReceiveNext();
            life = (int)stream.ReceiveNext();
        }
    }
}
