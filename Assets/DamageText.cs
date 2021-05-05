using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class DamageText : MonoBehaviour
{
    public PhotonView pv;

    [SerializeField] GameManager GM;
    [SerializeField] ObjectPooler objectManager;

    [SerializeField] Text damageText;

    [SerializeField] Color damageColor;
    [SerializeField] Color healColor;
    [SerializeField] Color criticalColor;

    Color color;

    [SerializeField] Font[] damageSkins;

    private void Awake()
    {
        GM = FindObjectOfType<GameManager>();
        objectManager = GM.OP;

        if(pv.IsMine)
            transform.parent = GameObject.Find("MyDamageDummy").gameObject.transform;
        else
            transform.parent = GameObject.Find("OtherDamageDummy").gameObject.transform;
        gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 1);
    }

    [PunRPC]
    public void ChangeTextRPC(int damageAmount, int damageSkinCode, bool IsCritical)
    {
        if (IsCritical)
            damageText.color = criticalColor;

        else
            damageText.color = damageColor;
        
        damageText.font = damageSkins[damageSkinCode];
    }

    public void PoolDestroy()
    {
        objectManager.PoolDestroy(gameObject);
    }
}
