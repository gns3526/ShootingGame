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
    public int petDamagePer;
    public int petAttackSpeedPer;
    public int penetratePer;
    public int finalDamagePer;

    public int skillCooldownPer;

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

    public float maxShotCoolTime;
    public float attackSpeedPer;
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
    public AbilityManager AM;
    public JobManager JM;
    public ReinForceManager RFM;
    public NetworkManager NM;

    [Header("Others")]
    [SerializeField] GameObject playerPoint;

    public Animator ani;
    [SerializeField] SpriteRenderer mainSprite;
    [SerializeField] Rigidbody2D rigid;

    public GameObject[] pets;
    public int petAmount;

    public bool canHit;
    public bool isRespawned;

    public bool[] joyControl;
    public bool isControl;

    public bool isFire;
    public bool weaponFire;

    public bool specialShot;
    
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
        if (pv.IsMine)
        {
            playerPoint.SetActive(true);

            

            StartCoroutine(StartDelay());
        }
    }

    private void Start()
    {
        
        if (pv.IsMine)
        {
            codyPv.RPC("CodyRework", RpcTarget.All, GM.codyMainCode, GM.codyBodyCode, GM.codyParticleCode);

            pv.RPC("ChangeColorRPC", RpcTarget.All, GM.playerColors[0], GM.playerColors[1], GM.playerColors[2]);
        }
    }

    IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(0.1f);
        JM.JobApply();
        AM.AbilityApply();
        RFM.ReinForceApply();

        GM.UpdateLifeIcon(life);

        NMPV = NM.GetComponent<PhotonView>();
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
            if (Input.GetKeyDown(KeyCode.Y))
            {
                OP.PoolInstantiate("Pet", transform.position, Quaternion.identity, -3, 0, -1, true);
            }
            if (Input.GetKeyDown(KeyCode.U))
            {
                OP.PoolInstantiate("Pet", transform.position, Quaternion.identity, -3, 1, -1, true);
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                GM.pv.RPC("ReviveTeam", RpcTarget.All, 5);
            }
            if(Input.GetKeyDown(KeyCode.P))
            {
                life = 0;
                GM.UpdateLifeIcon(life);
                pv.RPC("PlayerIsDie", RpcTarget.All);
                GM.PlayerDie();
            }
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
                    if (GM.isAndroid)
                        GM.weaponBulletText_M.text = curBulletAmount.ToString();
                    else
                        GM.weaponBulletText_D.text = curBulletAmount.ToString();
                    if (curBulletAmount == maxSpecialBullet)
                        GM.bulletMaxUi.SetActive(true);
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

        if ((isTouchRight && h == 1) || (isTouchLeft && h == -1)) h = 0;
        if ((isTouchTop && v == 1) || (isTouchBottom && v == -1)) v = 0;

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

            pv.RPC(nameof(SoundRPC), RpcTarget.All, 1);
        }
        if (isSpecialBulletAbility2 && (60 > randomNum))
        {
            GameObject bullet = OP.PoolInstantiate("BulletBasic", transform.position, Quaternion.identity, 1, -1, 8, true);
            bullet.GetComponent<BulletScript>().dmgPer = 130;
            curShotCoolTime = 0;

            pv.RPC(nameof(SoundRPC), RpcTarget.All, 2);
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

                pv.RPC(nameof(SoundRPC), RpcTarget.All, 5);
            }
            if(GM.isAndroid)
                GM.weaponBulletText_M.text = curBulletAmount.ToString();
            else
                GM.weaponBulletText_D.text = curBulletAmount.ToString();
            GM.bulletMaxUi.SetActive(false);
            return;
        }
    }

    void Reload()
    {
        curShotCoolTime += Time.deltaTime * ((attackSpeedPer + attackSpeedStack) / 100);
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
    void ChangeColorRPC(float r,float g, float b)
    {
        mainSprite.color = new Color(r/255f, g/255f, b/255f, 1);
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
