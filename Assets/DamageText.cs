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

    [SerializeField] Text damageText;



    Color color;

    

    private void Awake()
    {
        GM = FindObjectOfType<GameManager>();
        objectManager = GM.OP;
        dtm = GM.DTM;

        //if(pv.IsMine)
            transform.parent = GameObject.Find("MyDamageDummy").gameObject.transform;
        //else
        //    transform.parent = GameObject.Find("OtherDamageDummy").gameObject.transform;
        gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 1);
    }

    [PunRPC]
    public void ChangeTextRPC(int damageAmount ,int colorCode, int damageSkinCode, bool IsPlus)
    {
        if (IsPlus)
            damageText.text = "+" + damageAmount;
        else
            damageText.text = "-" + damageAmount;

        switch (colorCode)
        {
            case 0:
                damageText.color = dtm.damageColor;
                break;
            case 1:
                damageText.color = dtm.criticalColor;
                break;
            case 2:
                damageText.color = dtm.healColor;
                break;
        }

        damageText.font = dtm.damageSkins[damageSkinCode];
    }

    public void PoolDestroy()
    {
        objectManager.PoolDestroy(gameObject);
    }
}
