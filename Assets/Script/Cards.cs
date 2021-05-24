using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class Cards : MonoBehaviourPunCallbacks,IPunObservable
{
    public Player myPlayerScript;

    [SerializeField] ObjectPooler OP;
    [SerializeField] GameManager GM;
    [SerializeField] PlayerState ps;
    public PhotonView pv;

    [Header("Cards Info")]
    [SerializeField] List<GameObject> cards;
    [SerializeField] List<GameObject> cardsSave;

    [SerializeField] List<GameObject> rare;
    [SerializeField] List<GameObject> epic;
    [SerializeField] List<GameObject> unique;
    [SerializeField] List<GameObject> legendary;

    List<GameObject> rareSave;
    List<GameObject> epicSave;
    List<GameObject> uniqueSave;
    List<GameObject> legendarySave;

    public GameObject cardPanel;

    [SerializeField] int rarePer;
    [SerializeField] int epicPer;
    [SerializeField] int uniquePer;
    [SerializeField] int legendaryPer;

    [Header("Selecting Cards")]
    [SerializeField] int readyAmount;

    public int curMin;
    public float curSec;

    public bool isCellectingTime;


    [SerializeField] Text curTimeText;
    [SerializeField] Text curConditionText;
    [SerializeField] int cTInt;
    public bool isReady;
    [SerializeField] float cTTime;

    private void Start()
    {
        cardsSave = new List<GameObject>(cards);

        rareSave = new List<GameObject>(rare);
        epicSave = new List<GameObject>(epic);
        uniqueSave = new List<GameObject>(unique);
        legendarySave = new List<GameObject>(legendary);
    }

    public void SelectCard()
    {
        isReady = false;
        curMin = 1;
        curSec = 0;
        isCellectingTime = true;

        Player myplayerScript = GM.myplayer.GetComponent<Player>();

        GM.getExpAmountText.text = "";
        GM.getGoldAmountText.text = "";
        GM.goldAmountText3.text = GM.money.ToString();

        GM.gamePlayExpPanel.SetActive(true);
        GM.expGIveOnce = true;
        StartCoroutine(GM.ExpGiveDelay(GM.mapExpAmount[GM.mapCode]/ PhotonNetwork.PlayerList.Length));
        StartCoroutine(GM.GiveGold(GM.mapCoinAmount[GM.mapCode] / PhotonNetwork.PlayerList.Length));

        if (myplayerScript.isDie) return;

        if (myplayerScript.pets.Length == myplayerScript.petAmount)
        {
            epic.RemoveAt(1);
            epic.RemoveAt(2);
        }


        for (int i = 0; i < 3; i++)
        {
            int a = Random.Range(0, 101);
            Debug.Log(a);
            if (0 <= a && a < rarePer)
            {
                int randomR = Random.Range(0, rare.Count);

                rare[randomR].SetActive(true);
                rare.RemoveAt(randomR);
                Debug.Log("레어");
            }
            else if (rarePer <= a && a < rarePer + epicPer)
            {
                int randomR = Random.Range(0, epic.Count);

                epic[randomR].SetActive(true);
                epic.RemoveAt(randomR);
                Debug.Log("에픽");
            }
            else if (rarePer + epicPer <= a && a < rarePer + epicPer + uniquePer)
            {
                int randomR = Random.Range(0, unique.Count);

                unique[randomR].SetActive(true);
                unique.RemoveAt(randomR);
                Debug.Log("유니크");
            }
            else if (rarePer + epicPer + uniquePer <= a && a <= rarePer + epicPer + uniquePer + legendaryPer)
            {
                int randomR = Random.Range(0, legendary.Count);

                legendary[randomR].SetActive(true);
                legendary.RemoveAt(randomR);
                Debug.Log("레전");
            }
        }
        cardPanel.SetActive(true);
    }
    public void SelectComplete()
    {
        GM.gamePlayExpPanel.SetActive(false);
        GM.pv.RPC("StageStart", RpcTarget.All);
    }
    public void ClearCards()
    {
        cards = new List<GameObject>(cardsSave);

        rare = new List<GameObject>(rareSave);
        epic = new List<GameObject>(epicSave);
        unique = new List<GameObject>(uniqueSave);
        legendary = new List<GameObject>(legendarySave);
        Debug.Log("카드 없앰1");
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].SetActive(false);
        }

        Debug.Log("카드 없앰2");
    }


    public void CardS(int num)
    {

        switch (num)
        {
            case 1:
                ps.attackSpeedPer += 50;

                break;
            case 2:
                ps.moveSpeed += 20;
                break;
            case 3:
                ps.increaseDamagePer += 50;
                break;
            case 4:
                if (myPlayerScript.power != myPlayerScript.maxPower)
                {
                    myPlayerScript.power++;
                }
                break;
            case 5:
                if (ps.maxLife != 10)
                {
                    ps.maxLife++;
                    ps.life++;
                    GM.UpdateLifeIcon(ps.life);
                }
                break;
            case 6:
                ps.life = ps.maxLife;
                GM.UpdateLifeIcon(ps.life);
                break;
            case 7:
                OP.PoolInstantiate("Pet", myPlayerScript.transform.position, Quaternion.identity, -3, 0, -1, 0, true);
                break;
            case 8:
                OP.PoolInstantiate("Pet", myPlayerScript.transform.position, Quaternion.identity, -3, 1, -1, 0, true);
                break;
            case 9:
                ps.godTime += 2;
                break;
            case 10:
                ps.missPercentage += 10;
                break;
            case 11:
                GM.pv.RPC("ReviveTeam", RpcTarget.All, 1);
                break;
            case 12:
                myPlayerScript.isDamageStack = true;
                break;
            case 13:
                myPlayerScript.isAttackSpeedStack = true;
                break;
            case 14:
                ps.bossDamagePer += 60;
                break;
            case 15:
                ps.criticalPer += 10;
                break;
            case 16:
                ps.criticalDamagePer += 50;
                break;
            case 17:
                myPlayerScript.isSpecialBulletAbility1 = true;
                myPlayerScript.isSpecialBulletAbility2 = false;
                break;
            case 18:
                myPlayerScript.isSpecialBulletAbility1 = false;
                myPlayerScript.isSpecialBulletAbility2 = true;
                break;
            case 19:
                GM.canShotWeapon = true;

                if (GM.isAndroid)
                    GM.mobileWeaponLockOb.SetActive(false);
                else
                    GM.deskTopWeaponLockOb.SetActive(false);

                myPlayerScript.gotSpecialWeaponAbility = true;
                myPlayerScript.weaponCode = 1;
                myPlayerScript.weaponDmg = 25;
                myPlayerScript.toTalChargeTime = 2.5f;
                myPlayerScript.curChargeTime = myPlayerScript.toTalChargeTime;
                myPlayerScript.curBulletAmount = 0;
                myPlayerScript.maxSpecialBullet = 20;
                myPlayerScript.weaponTotalShotCoolTime = 0.5f;
                myPlayerScript.curWeaponShotCoolTime = -1;

                GM.raderOb.SetActive(false);

                GM.WeaponButtonUpdate();
                break;
            case 20:
                GM.canShotWeapon = true;

                if (GM.isAndroid)
                    GM.mobileWeaponLockOb.SetActive(false);
                else
                    GM.deskTopWeaponLockOb.SetActive(false);

                myPlayerScript.gotSpecialWeaponAbility = true;
                myPlayerScript.weaponCode = 2;
                myPlayerScript.weaponDmg = 2;
                myPlayerScript.toTalChargeTime = 1f;
                myPlayerScript.curChargeTime = myPlayerScript.toTalChargeTime;
                myPlayerScript.curBulletAmount = 0;
                myPlayerScript.maxSpecialBullet = 50;
                myPlayerScript.weaponTotalShotCoolTime = 0.1f;
                myPlayerScript.curWeaponShotCoolTime = -1;

                GM.raderOb.SetActive(false);

                GM.WeaponButtonUpdate();
                break;
            case 21:
                ps.skillCooldownPer += 50;
                break;
            case 22:
                ps.damage += 2;
                break;
            case 23:
                ps.finalDamagePer += 50;
                break;
            case 24:
                ps.normalMonsterDamagePer += 50;
                break;
            case 25:
                ps.penetratePer += 30;
                break;
            case 26:
                ps.petDamagePer += 50;
                break;
            case 27:
                ps.petAttackSpeedPer += 50;
                break;
            case 28:
                GM.canShotWeapon = false;

                if (GM.isAndroid)
                    GM.mobileWeaponLockOb.SetActive(true);
                else
                    GM.deskTopWeaponLockOb.SetActive(true);

                myPlayerScript.gotSpecialWeaponAbility = true;
                myPlayerScript.weaponCode = 3;
                myPlayerScript.weaponDmg = 1;
                myPlayerScript.toTalChargeTime = 0.8f;
                myPlayerScript.curChargeTime = myPlayerScript.toTalChargeTime;
                myPlayerScript.curBulletAmount = 0;
                myPlayerScript.maxSpecialBullet = 15;
                myPlayerScript.weaponTotalShotCoolTime = 0.2f;
                myPlayerScript.curWeaponShotCoolTime = -1;

                GM.raderScript.myPlayerScript = myPlayerScript;
                GM.raderOb.SetActive(true);

                GM.WeaponButtonUpdate();
                break;
        }
        if (!GM.isGameStart) return;
        StartCoroutine(SelectDelay());
        ClearCards();
    }

    IEnumerator SelectDelay()
    {
        if (curSec > 4)
            yield return new WaitForSeconds(4);
        else
            yield return new WaitForSeconds(curSec);
        isReady = true;
        pv.RPC("ReadyAmountReset", RpcTarget.All);
    }

    [PunRPC]
    void ReadyAmountReset()
    {
        readyAmount++;
        Debug.Log("aaa");
        if(PhotonNetwork.PlayerList.Length == readyAmount)
        {
            curSec = -11;
            SelectComplete();
            readyAmount = 0;
        }
    }

    [PunRPC]
    void ReadyAmountSetZero()
    {
        readyAmount = 0;
    }


    private void Update()
    {
        if (isCellectingTime)
        {
            curSec -= Time.deltaTime;
            if (curSec < 0 && curMin > 0)
            {
                curMin--;
                curSec = 59;
            }
            if (curSec > 0)
            {
                curTimeText.text = string.Format("{0:00}:{1:00}", curMin, curSec);
            }
            else if(curSec < -10)
            {
                return;
            }
            else
            {
                isCellectingTime = false;
                ClearCards();
                SelectComplete();
                pv.RPC("ReadyAmountSetZero", RpcTarget.All);
            }

            if (isReady)
            {
                cTTime -= Time.deltaTime;
                if(cTTime < 0)
                {
                    cTTime = 0.3f;
                    if(cTInt == 0)
                    {
                        curConditionText.text = "Waiting for another myPlayerScript.";
                        cTInt = 1;
                    }
                    else if (cTInt == 1)
                    {
                        curConditionText.text = "Waiting for another myPlayerScript..";
                        cTInt = 2;
                    }
                    else if (cTInt == 2)
                    {
                        curConditionText.text = "Waiting for another myPlayerScript...";
                        cTInt = 0;
                    }
                }
            }
            else
            {
                curConditionText.text = "";
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(readyAmount);
        }
        else
        {
            readyAmount = (int)stream.ReceiveNext();
        }
    }
}
