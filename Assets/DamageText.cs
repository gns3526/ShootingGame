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
    public void ChangeTextRPC(int damageAmount, int damageSkinCode, bool IsCritical)
    {
        if (IsCritical)
            damageText.color = dtm.criticalColor;

        else
            damageText.color = dtm.damageColor;
        
        damageText.font = dtm.damageSkins[damageSkinCode];
    }

    public void PoolDestroy()
    {
        objectManager.PoolDestroy(gameObject);
    }
}
