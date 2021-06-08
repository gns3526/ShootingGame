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

    public bool isTouchTop;
    public bool isTouchLeft;
    public bool isTouchRight;
    public bool isTouchBottom;

    public int score;

    
    public int power;
    public int maxPower;
    

    public bool gotSpecialWeaponAbility;
    public int weaponCode;
    public int weaponDmg;
    public float toTalChargeTime;
    public float curChargeTime;
    public int curBulletAmount;
    public int maxSpecialBullet;
    public float weaponTotalShotCoolTime;
    public float curWeaponShotCoolTime;

    public bool isSpecialBulletAbility1;
    public bool isSpecialBulletAbility2;





    public bool isAttackSpeedStack;
    public int attackSpeedStackint;
    public float attackSpeedStack;
    public bool isDamageStack;
    public int damageStackint;
    public float damageStack;

    [Header("PlayerCody")]
    public PhotonView codyPv;

    [Header("Skill")]
    public BarrierScript skillC;

    [Header("Managers")]
    public GameManager GM;
    public ObjectPooler OP;
    public AbilityManager AM;
    public JobManager JM;
    public ReinForceManager RFM;
    public NetworkManager NM;
    public PlayerState ps;
    public PlayerColorManager colorManager;

    [Header("Others")]
    [SerializeField] GameObject playerPoint;

    public Animator ani;
    [SerializeField] SpriteRenderer mainSprite;
    [SerializeField] SpriteRenderer boosterSprite;
    [SerializeField] Rigidbody2D rigid;

    public GameObject[] pets;
    public int petAmount;

    public bool canHit;
    public bool isRespawned;

    public bool[] joyControl;
    public bool isControl;

    public bool isFire;
    public bool weaponFire;
    
    PhotonView NMPV;
    [SerializeField] Text nickNameText;
    public PhotonView pv;

    [SerializeField] Vector3 curPosPv;

    GameObject bullet;
    BulletScript bs;

    private void Awake()
    {

        nickNameText.text = pv.IsMine ? PhotonNetwork.NickName : pv.Owner.NickName;//NickName Setting

        GM = FindObjectOfType<GameManager>();

        colorManager = GM.colorManager;
        if (pv.IsMine)
        {
            playerPoint.SetActive(true);

            

            StartCoroutine(StartDelay());
        }
    }

    private void Start()
    {
        //GM.CM.CardS(19);
        if (pv.IsMine)
        {
            codyPv.RPC("CodyRework", RpcTarget.All, GM.codyMainCode, GM.codyBodyCode, GM.codyParticleCode);

            pv.RPC("ChangeColorRPC", RpcTarget.All, colorManager.playerMainColors[0], colorManager.playerMainColors[1], colorManager.playerMainColors[2], colorManager.playerBoosterColors[0], colorManager.playerBoosterColors[1], colorManager.playerBoosterColors[2]);
        }
    }

    IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(0.1f);

        GM.UpdateLifeIcon(ps.life);

        NMPV = NM.GetComponent<PhotonView>();
    }

    private void Update()
    {
        
        if (pv.IsMine)
        {
            if (!NM.isChating && GM.canControll)
            {
                Move();
                WeaponFire();
                Fire();
            }
            else
                rigid.velocity = new Vector2(0, 0);

            Reload();

            if (Input.GetKeyDown(KeyCode.S))
            {
                if(JM.jobCode == 1)
                {
                    if (JM.desktopSkillBPanel.activeSelf)
                    {
                        JM.SkillOnClick(false);
                    }
                    else
                    {
                        JM.SkillOnClick(true);
                    }
                }
                else
                {
                    JM.SkillOnClick(true);
                }
            }


            if (!GM.isAndroid)
            {
                if (Input.GetKey(KeyCode.A))
                {
                    isFire = true;
                }
                else
                    isFire = false;
                if (Input.GetKey(KeyCode.W))
                {
                    weaponFire = true;
                }
                else
                    weaponFire = false;
            }

            if (maxSpecialBullet > curBulletAmount && GM.isPlaying)
            {
                curChargeTime -= Time.deltaTime;
                if(curChargeTime < 0)
                {
                    curChargeTime = toTalChargeTime;
                    curBulletAmount++;
                    if (GM.isAndroid)
                        GM.weaponBulletText_M.text = curBulletAmount.ToString() + "/" + maxSpecialBullet.ToString();
                    else
                        GM.weaponBulletText_D.text = curBulletAmount.ToString() + "/" + maxSpecialBullet.ToString();
                    if (curBulletAmount == maxSpecialBullet)
                        GM.bulletMaxUi.SetActive(true);
                }
            }
            if(curWeaponShotCoolTime < weaponTotalShotCoolTime && GM.isPlaying)
            {
                curWeaponShotCoolTime += Time.deltaTime * ((ps.attackSpeedPer + attackSpeedStack) / 100);
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

        if ((isTouchRight && h == 1) || (isTouchLeft && h == -1)) h = 0;
        if ((isTouchTop && v == 1) || (isTouchBottom && v == -1)) v = 0;

        if (Input.GetKey(KeyCode.Space))
            rigid.velocity = new Vector2(h * 1.5f, v * 1.5f);
        else
            rigid.velocity = new Vector2((ps.moveSpeed / 100) * 4 * h, (ps.moveSpeed / 100) * 4 * v);

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

        if (ps.curShotCoolTime < ps.maxShotCoolTime) return;

        JM.Shot();
        ps.curShotCoolTime = 0;
    }

    bool left;
    void WeaponFire()
    {
        if (isDie) return;

        if (!GM.isPlaying) return;

        if (gotSpecialWeaponAbility && weaponFire)
        {
            if (curBulletAmount > 0 && curWeaponShotCoolTime > weaponTotalShotCoolTime)
            {
                switch (weaponCode)
                {
                    case 1:
                        BulletScript bullet = OP.PoolInstantiate("BulletBasic", transform.position, Quaternion.identity, 3, -1, 15, 1, true).GetComponent<BulletScript>();
                        bullet.dmg = weaponDmg;
                        bullet.attackAmount = 100;
                        break;
                    case 2:
                        float randomAngle = Random.Range(-7f, 7f);
                        BulletScript bullet1 = OP.PoolInstantiate("BulletBasic", transform.position, Quaternion.Euler(0,0,randomAngle), 2, -1, 10, 0, true).GetComponent<BulletScript>();
                        bullet1.dmg = weaponDmg;
                        break;
                    case 3:
                        if (GM.raderScript.enemys.Count == 0) return;

                        left = !left;

                        int randomEnemyNum = Random.Range(0, GM.raderScript.enemys.Count);

                        BulletScript bullet2 = OP.PoolInstantiate("BulletBasic", transform.position, Quaternion.Euler(0, 0, left == true ? 140 : 220), 2, -1, 7, 0, true).GetComponent<BulletScript>();

                        bullet2.homingPower = 400;
                        bullet2.dmg = weaponDmg;
                        bullet2.isFollowTarget = true;
                        bullet2.target = GM.raderScript.enemys[randomEnemyNum];

                        
                        break;
                }
                
                curBulletAmount--;
                curWeaponShotCoolTime = 0;

                pv.RPC(nameof(SoundRPC), RpcTarget.All, 5);
            }
            if(GM.isAndroid)
                GM.weaponBulletText_M.text = curBulletAmount.ToString() + "/" + maxSpecialBullet.ToString();
            else
                GM.weaponBulletText_D.text = curBulletAmount.ToString() + "/" + maxSpecialBullet.ToString();
            GM.bulletMaxUi.SetActive(false);
            return;
        }
    }

    void Reload()
    {
        ps.curShotCoolTime += Time.deltaTime * ((ps.attackSpeedPer + attackSpeedStack) / 100);
    }

    [PunRPC]
    public void SoundRPC(int SoundNum)
    {
        switch (SoundNum)
        {
            case 1:
                SoundManager.Play("Gun_1");
                break;
            case 2:
                SoundManager.Play("Gun_2");
                break;
            case 3:
                SoundManager.Play("Gun_3");
                break;
            case 4:
                SoundManager.Play("Gun_4");
                break;
            case 5:
                SoundManager.Play("Gun_5");
                break;
            case 6:
                SoundManager.Play("Gun_6");
                break;
            case 7:
                SoundManager.Play("Laser_Ready");
                break;
            case 8:
                SoundManager.Play("Laser_1");
                break;
            case 9:
                SoundManager.Play("Laser_2");
                break;

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
        
    }

    

    [PunRPC]
    void petSpriteChangeRPC(int i, int type)
    {
        petAmount++;
        if(type == 1)
            pets[i].GetComponent<SpriteRenderer>().sprite = Resources.Load("PetSprite" + "/" + "Num1", typeof(Sprite)) as Sprite;

        else if(type == 2)
            pets[i].GetComponent<SpriteRenderer>().sprite = Resources.Load("PetSprite" + "/" + "Num2", typeof(Sprite)) as Sprite;
    }


    [PunRPC]
    void ChangeColorRPC(float r,float g, float b, float rr, float gg, float bb)
    {
        mainSprite.color = new Color(r/255f, g/255f, b/255f, 1);
        boosterSprite.color = new Color(rr / 255f, gg / 255f, bb / 255f, 1);
    }

    [PunRPC]
    void PlayerIsDie()
    {
        isDie = true;
        godMode = true;
    }

    [PunRPC]
    void PlayerIsAlive()
    {
        isDie = false;
        godMode = false;
    }

    /*[PunRPC]
    public void LifeUpdate(int life)
    {
        lifeText.text = life.ToString();
    }*/
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
            //stream.SendNext(ps.life);
        }
        else
        {
            curPosPv = (Vector3)stream.ReceiveNext();
            //ps.life = (int)stream.ReceiveNext();
        }
    }
}
