using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class DamageText : MonoBehaviour
{
    public PhotonView pv;

    [SerializeField] GameManager GM;
    ObjectPooler objectManager;
    DamageTextManager dtm;
    OptionManager option;

    [SerializeField] Text damageText;
    [SerializeField] Outline damageOutline;

    [SerializeField] Animator ani;

    Color color;

    

    private void Awake()
    {
        GM = FindObjectOfType<GameManager>();
        objectManager = GM.OP;
        dtm = GM.DTM;
        option = GM.option;

        if (pv.IsMine)
            transform.parent = GameObject.Find("MyDamageDummy").gameObject.transform;
            
        else
            transform.parent = GameObject.Find("OtherDamageDummy").gameObject.transform;
            
        gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 1);
    }

    private void OnEnable()
    {
        ani.SetTrigger("Trigger");
    }

    [PunRPC]
    public void ChangeTextRPC(int damageAmount ,int colorCode, int damageSkinCode, bool IsPlus)
    {
        if (IsPlus)
            damageText.text = "+" + damageAmount;
        else
            damageText.text = "-" + damageAmount;

        if (pv.IsMine)
        {
            switch (colorCode)
            {
                case 0:
                    damageText.color = dtm.damageColor_M;
                    break;
                case 1:
                    damageText.color = dtm.criticalColor_M;
                    break;
                case 2:
                    damageText.color = dtm.healColor_M;
                    break;
                case 3:
                    damageText.color = dtm.weaponColor_M;
                    break;
                case 4:
                    damageText.color = dtm.weaponCriticalColor_M;
                    break;
            }
            damageOutline.effectColor = dtm.outline_M;
        }

        else
        {
            switch (colorCode)
            {
                case 0:
                    damageText.color = dtm.damageColor_Y;
                    break;
                case 1:
                    damageText.color = dtm.criticalColor_Y;
                    break;
                case 2:
                    damageText.color = dtm.healColor_Y;
                    break;
                case 3:
                    damageText.color = dtm.weaponColor_Y;
                    break;
                case 4:
                    damageText.color = dtm.weaponCriticalColor_Y;
                    break;
            }
            damageOutline.effectColor = dtm.outline_Y;
        }
            


        damageText.font = dtm.damageSkins[damageSkinCode];
    }

    public void PoolDestroy()
    {
        objectManager.PoolDestroy(gameObject);
    }
}
